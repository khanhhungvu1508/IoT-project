print("Hello Thingboard")
import paho.mqtt.client as mqttclient
import time
import json
import requests
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.common.by import By

BROKER_ADDRESS = "demo.thingsboard.io"
PORT = 1883
THINGS_BOARD_ACCESS_TOKEN = "dXltUeQUVFa5gxrCd3Ca"

def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")


def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    temp_data = {'value': True}
    try:
        jsonobj = json.loads(message.payload)
        if jsonobj['method'] == "setValue":
            temp_data['value'] = jsonobj['params']
            client.publish('v1/devices/me/attributes', json.dumps(temp_data), 1)
    except:
        pass


def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Thingsboard connected successfully!!")
        client.subscribe("v1/devices/me/rpc/request/+")
    else:
        print("Connection is failed")


client = mqttclient.Client("Gateway_Thingsboard")
client.username_pw_set(THINGS_BOARD_ACCESS_TOKEN)

client.on_connect = connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()

client.on_subscribe = subscribed
client.on_message = recv_message

def getLocationByChromeDriver():
    # All comment below is the code which older version, it easily deprecated
    options = Options()
    options.add_argument("--use-fake-ui-for-media-stream")
    options.add_experimental_option('excludeSwitches', ['enable-logging'])
    options.add_experimental_option('useAutomationExtension', False)
    timeout = 20
    s = Service(ChromeDriverManager().install())
    driver = webdriver.Chrome(service=s, options=options)
    #driver = webdriver.Chrome(executable_path = './chromedriver.exe', chrome_options=options)
    driver.get("https://mycurrentlocation.net/")
    wait = WebDriverWait(driver, timeout)
    time.sleep(3)
    #longitude = driver.find_elements_by_xpath('//*[@id="longitude"]')
    #longitude = [x.text for x in longitude]
    #longitude = str(longitude[0])

    longitude = driver.find_element(By.XPATH, '//*[@id="longitude"]').text

    #latitude = driver.find_elements_by_xpath('//*[@id="latitude"]')
    #latitude = [x.text for x in latitude]
    #latitude = str(latitude[0])

    latitude = driver.find_element(By.XPATH, '//*[@id="latitude"]').text

    driver.quit()
    return [latitude,longitude]

def getLocationByThirdParty():
    GPSInfo = requests.get('https://ipinfo.io/')
    GPS = GPSInfo.json()
    print(GPS)
    return GPS['loc'].split(',')

temp = 30
humi = 50
counter = 0
while True:
    # Find latitude and longitude by GPS location
    #[latitude, longitude] = getLocationByThirdParty()
    [latitude, longitude] = getLocationByChromeDriver()
    # Debug line
    print('longitude = ', longitude)
    print('latitude = ', latitude)
    collect_data = {'temperature': temp, 'humidity': humi, 'longitude': longitude, 'latitude': latitude}
    temp += 1
    humi += 1
    client.publish('v1/devices/me/telemetry', json.dumps(collect_data), 1)
    time.sleep(10)

