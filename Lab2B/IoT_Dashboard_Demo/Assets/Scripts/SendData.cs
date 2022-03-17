using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Newtonsoft.Json;

[System.Serializable]
public class Device
{
    public int temperature;
    public int humidity;
}

public class SendData : MonoBehaviour
{
	string messageReceived = "", prevMessageReceived = "";
	public Text _temperature;
	public Text	_humidity;
	public SwitchUI _led;
	public SwitchUI _pump;
	public GaugeUI _temperatureGauge;
	public GaugeUI _humidityGauge;

	
	// Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello scene2");
		Login.instanceLogin.mqtt.setMessage(null);
		Debug.Log(Login.instanceLogin.mqtt.checkclientConnected());
		_pump.setTypeofButton(2);
		_humidityGauge.setTypeofGauge(2);
		/*
		string msg = "{\"temperature\":30,\"humidity\":50}";
		Device myDevice = JsonConvert.DeserializeObject<Device>(msg);
		_temperature.text = myDevice.temperature + "°C";
		_humidity.text = myDevice.humidity + "%";
		*/
    }

    // Update is called once per frame
    void Update()
    {
		Login.instanceLogin.mqtt.Update();
		List<string> msgReceived = Login.instanceLogin.mqtt.getEventMessage();
		for (int i = 0; i < msgReceived.Count; i++)
		{
			Debug.Log(msgReceived[i]);
			messageReceived = msgReceived[i];
			if (!string.Equals(messageReceived, prevMessageReceived) && messageReceived != "")
			{
				Device myDevice = JsonConvert.DeserializeObject<Device>(messageReceived);
				_temperature.text = myDevice.temperature + "°C";
				_humidity.text = myDevice.humidity + "%";
				_temperatureGauge.updateGauge(myDevice.temperature);
				_humidityGauge.updateGauge(myDevice.humidity);

				prevMessageReceived = messageReceived;
			}

		}
		Login.instanceLogin.mqtt.clearEventMessage();
    }
	
	void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
