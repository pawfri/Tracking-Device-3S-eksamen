from socket import *
import requests # install this library: $ python -m pip install requests
import json

PORT = 17001

sock_receiver = socket(AF_INET, SOCK_DGRAM)
sock_receiver.bind(("", PORT))

print("Proxy UDP Receiver started")
print(f'Listening for incoming UDP messages on port {PORT}')

REST_API_URL = 'http://localhost:5226/api/TrackingDevice'

while True:
    msg, clientAdr = sock_receiver.recvfrom(3000)
    message_str = msg.decode()
    print(f'Message from UDP broadcaster {clientAdr}: {message_str}')

    message_dictionary = json.loads(message_str)
    print(f'Converted to dictionary: {message_dictionary}')

    response = requests.post(REST_API_URL, json=message_dictionary)
    # json=... automatically serializes the dictionary to JSON
    # json=... automatically sets the Content-Type header to application/json
    print(f'Response from REST API: {response.status_code} - {response.text}')  