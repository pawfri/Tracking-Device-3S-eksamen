from socket import *
import time
import json

BROADCAST_IP = '255.255.255.255' #special IP address for broadcast
PORT = 17000

sock_sender = socket(AF_INET, SOCK_DGRAM)
sock_sender.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)

MockLokationer = [  {
    "id": 1,
    "longitude": 10,
    "latitude": 11,
    "date": "2025-12-01T09:37:02.681Z"
  },
  {
    "id": 2,
    "longitude": 20,
    "latitude": 30,
    "date": "2025-12-01T09:37:02.681Z"
  }]

message: str = json.dumps(MockLokationer)
print(f'Broadcaster sending: {message}')
sock_sender.sendto(message.encode(), (BROADCAST_IP, PORT))
time.sleep(5) # sleep for 2 seconds between messages

sock_sender.close()