using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class Login : MonoBehaviour
{
    public Button _button;
	public Text _message;
	public InputField _brokerURI;
	public InputField _username;
	public InputField _password;
	public string brokerURI = "", userName = "", passWord = "";
	
	public M2MqttUnityClient mqtt;
	
	public static Login instanceLogin;
	
	// Start is called before the first frame update
    void Start()
    {
		Debug.Log("Hello scene1");
		_brokerURI.text = brokerURI;
		_username.text = userName;
		_password.text = passWord;
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	
	public void manageLogin()
	{
	    brokerURI = _brokerURI.text;
		userName = _username.text;
		passWord = _password.text;
		mqtt.setBrokerAddress(brokerURI);
		mqtt.setUserName(userName);
		mqtt.setPassWord(passWord);
		mqtt.setMessage(_message);
		mqtt.Connect();
		//Debug.Log(mqtt.checkConnectedServer());
		//if (mqtt.checkConnectedServer())
			//_message.text = "Connect successfully!";
		//else
			//_message.text = "Your account is not valid!";	
	}
	
	void Awake()
	{
		instanceLogin = this;
		DontDestroyOnLoad(this.gameObject);
	}
}
