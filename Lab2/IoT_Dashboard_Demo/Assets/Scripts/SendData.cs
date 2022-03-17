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
	public bool led; 
	public bool pump;
	public string ts;
}

[System.Serializable]
public class JSONDevice
{
    public string method;
	public Device param;
}

public class SendData : MonoBehaviour
{
	string messageReceived = "", prevMessageReceived = "", tsGlobal = "";
	public Text _temperature;
	public Text	_humidity;
	public SwitchUI _led;
	public SwitchUI _pump;
	public GaugeUI _temperatureGauge;
	public GaugeUI _humidityGauge;

	// Algorithm compare 2 string ts and ts_global
	// It return true if ts > ts_global, else return false
	
	bool isLarger(string ts)
	{
		int numchar_ts = ts.Length, numchar_tsGlobal = tsGlobal.Length;
		if (numchar_ts > numchar_tsGlobal)
			return true;
		if (numchar_ts < numchar_tsGlobal)
			return false;
		for (int i = 0; i < numchar_ts; i++)
		{
			if (ts[i] > tsGlobal[i])
				return true;
			if (ts[i] < tsGlobal[i])
				return false;
		}
		return false;
	}
	
	// Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello scene2");
		Login.instanceLogin.mqtt.setMessage(null);
		Debug.Log(Login.instanceLogin.mqtt.checkclientConnected());
		_pump.setTypeofButton(2);
		_humidityGauge.setTypeofGauge(2);
		Task.Run(() => Login.instanceLogin.mqtt.PersistConnectionAsync());
		//string tmp = "{\"method\":\"TempHumid\",\"params\":{\"temperature\":30,\"humidity\":50,\"ts\":\"1645157532852\"}}";
		//string tmp_param = tmp.Replace("params", "param");
		//JSONdata myObject = JsonConvert.DeserializeObject<JSONdata>(tmp_param);
		//Debug.Log(myObject.param.ts);
		//_temperature.text = myObject.param.temperature + "°C";
		//_humidity.text = myObject.param.humidity + "%";
    }

    // Update is called once per frame
    void Update()
    {
		Login.instanceLogin.mqtt.Update();
		List<string> msgReceived = Login.instanceLogin.mqtt.getEventMessage();
		for (int i = 0; i < msgReceived.Count; i++)
		{
			messageReceived = msgReceived[i];
			if (!string.Equals(messageReceived, prevMessageReceived) && messageReceived != "")
			{
				if (messageReceived.Contains("getDevice"))
				{
					string messageReceived_param = messageReceived.Replace("params", "param");
					JSONDevice dataDevice = JsonConvert.DeserializeObject<JSONDevice>(messageReceived_param);
					if (isLarger(dataDevice.param.ts))
					{
						Debug.Log(messageReceived);
						Debug.Log("Doing modified all!");
						_temperature.text = dataDevice.param.temperature + "°C";
						_humidity.text = dataDevice.param.humidity + "%";
						_temperatureGauge.updateGauge(dataDevice.param.temperature);
						_humidityGauge.updateGauge(dataDevice.param.humidity);
						if (_led.getDirectionSending() == 0 &&
							(dataDevice.param.led == false && _led.getSwitchState() == -1 || 
							dataDevice.param.led == true && _led.getSwitchState() == 1))
							_led.toggleDirectionSending();
						if (_pump.getDirectionSending() == 0 &&
							(dataDevice.param.pump == false && _pump.getSwitchState() == -1 || 
							dataDevice.param.pump == true && _pump.getSwitchState() == 1))
							_pump.toggleDirectionSending();
						if (string.Equals(tsGlobal, ""))
						{
							if (_led.getDirectionSending() == 1)
							{
								if (dataDevice.param.led == false)
									_led.toggleSwitchInitState();
							}
							if (_pump.getDirectionSending() == 1)
							{
								if (dataDevice.param.pump == false)
									_pump.toggleSwitchInitState();
							}
						}
						else 
						{	
							if (_led.getDirectionSending() == 1)
							{
								if (dataDevice.param.led == false)
									_led.toggleSwitchState(-1);
								else
									_led.toggleSwitchState(1);
							}
							
							if (_pump.getDirectionSending() == 1)
							{
								if (dataDevice.param.pump == false)
									_pump.toggleSwitchState(-1);
								else
									_pump.toggleSwitchState(1);
							}
						}
						tsGlobal = dataDevice.param.ts;
					}
					else if (string.Equals(dataDevice.param.ts, tsGlobal))
					{
						Debug.Log("Doing modified only continouous data!");
						_temperature.text = dataDevice.param.temperature + "°C";
						_humidity.text = dataDevice.param.humidity + "%";
						_temperatureGauge.updateGauge(dataDevice.param.temperature);
						_humidityGauge.updateGauge(dataDevice.param.humidity);
					}
					
				}
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
