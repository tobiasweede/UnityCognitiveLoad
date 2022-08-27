import socket
import random
from time import sleep

def send_random_value(host='127.0.0.1', port=65432):
    value = random.random() * 100
    value = (str(random.randint(0,255)) + ","+ 
             str(random.randint(0,255)) + ","+ 
             str(random.randint(0,255)) + ","+ 
             str(random.randint(0,255))
            ).encode()
    s = socket.socket()
    s.connect((host, port))
    s.send(str(value).encode())
    s.close()

if __name__ == '__main__':
    while True:
        send_random_value()
        sleep(1)
