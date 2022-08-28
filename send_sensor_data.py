import socket
import random
from time import sleep

def get_sensor_data():
    '''Work to do here...'''
    return (str(random.randint(30, 200))).encode('utf-8')

def send_random_value(host='127.0.0.1', port=65431):
    '''Send sensor data to a socket.''' 
    value = get_sensor_data()
    s = socket.socket()
    s.connect((host, port))
    s.send(value)
    s.close()

if __name__ == '__main__':
    while True:
        send_random_value()
        sleep(1)
