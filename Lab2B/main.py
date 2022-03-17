print("Hello MQTTServer")
import paho.mqtt.client as mqttclient
import time
import json

BROKER_ADDRESS = "mqttserver.tk"
PORT = 1883
MQTT_SERVER_USERNAME = "bkiot"
MQTT_SERVER_PASSWORD = "12345678"
topic_publish = '/bkiot/1910232/status'
topic_subscribe = ['/bkiot/1910232/led', '/bkiot/1910232/pump']

def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")


def recv_message(client, userdata, message):
    print('Topic: ' + message.topic + '\nMessage: ' + str(message.payload))


def connected(client, usedata, flags, rc):
    if rc == 0:
        print("MQTTServer connected successfully!!")
        #client.subscribe("/bkiot/1910232/status")
        for topic in topic_subscribe:
            client.subscribe(topic)
    else:
        print("Connection is failed")


client = mqttclient.Client("Gateway_1910232_MQTTServer")
client.username_pw_set(username = MQTT_SERVER_USERNAME, password = MQTT_SERVER_PASSWORD)

client.on_connect = connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()

client.on_subscribe = subscribed
client.on_message = recv_message

temp = 30
humi = 50
while True:
    collect_data = {'temperature': temp, 'humidity': humi}
    temp += 1
    humi += 1
    client.publish(topic_publish, json.dumps(collect_data), 1)
    time.sleep(10)

