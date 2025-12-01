from socket import *
import time
import json
import gps

BROADCAST_IP = "192.168.103.255"
PORT = 17000

print("her er jeg 1...")

# Setup UDP Broadcast socket
sock_sender = socket(AF_INET, SOCK_DGRAM)
sock_sender.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)
sock_sender.bind(("", 0))

print("her er jeg 2...")

#Setup GPSD
session = gps.gps(mode=gps.WATCH_ENABLE | gps.WATCH_NEWSTYLE)

print("Starting GPS broadcast...")

while True:
    try:
        report = session.next()

        if report.get("class") == "TPV":
            lat = getattr(report, "lat", None)
            lon = getattr(report, "lon", None)
            timestamp = getattr(report, "time", None)

            if lat is not None and lon is not None:
                data = {
                    "latitude": lat,
                    "longitude": lon,
                    "timestamp": timestamp
                }

                message = json.dumps(data)

                print(f"Broadcasting: {message}")
                sock_sender.sendto(message.encode(), (BROADCAST_IP, PORT))

        time.sleep(10)

    except KeyboardInterrupt:
        print("Stopping GPS broadcast.")
        break
    except StopIteration:
        print("Lost connection to gpsd.")
        break