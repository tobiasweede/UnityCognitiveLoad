import socket
import random
from time import sleep

def send_random_value(host='127.0.0.1', port=65432):
    '''Create a random RGBA color and send it to a socket.''' 
    value = (str(random.randint(0,255)) + ","+ 
             str(random.randint(0,255)) + ","+ 
             str(random.randint(0,255)) + ","+ 
             str(random.randint(0,255))
            ).encode('utf-8')
    s = socket.socket()
    s.connect((host, port))
    s.send(value)
    s.close()

if __name__ == '__main__':
    while True:
        send_random_value()
        sleep(1)
