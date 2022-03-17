using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SwitchUI : MonoBehaviour
{
    private int switchState = 1;
	public GameObject switchBtn;
	public GameObject bgImg;
	private int typeofButton = 1; // 1 is LED, 2 is PUMP

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void setTypeofButton(int val)
	{
		typeofButton = val;
	}
	
	public void OnSwitchButtonClicked(){
		Debug.Log("OnSwitchButtonClicked() is called");
		switchBtn.transform.DOLocalMoveX(-switchBtn.transform.localPosition.x,0.2f);
		switchState = Math.Sign(-switchBtn.transform.localPosition.x);
		Color color = bgImg.GetComponent<Image>().color;
		if (switchState == 1)
		{
			color.r = 0f/255f;
			color.g = 127f/255f;
			color.b = 234f/255f;
		}
		else
		{
			color.r = 255f/255f;
			color.g = 0f/255f;
			color.b = 0f/255f;
		}
		bgImg.GetComponent<Image>().color = color;
		if (typeofButton == 1) // LED
		{
			if (switchState == 1)
				Login.instanceLogin.mqtt.PublishTopics(0, "{\"device\":\"LED\",\"status\":\"ON\"}");
			else
				Login.instanceLogin.mqtt.PublishTopics(0, "{\"device\":\"LED\",\"status\":\"OFF\"}");
		}
		else if (typeofButton == 2) // PUMP
		{
			if (switchState == 1)
				Login.instanceLogin.mqtt.PublishTopics(1, "{\"device\":\"PUMP\",\"status\":\"ON\"}");
			else
				Login.instanceLogin.mqtt.PublishTopics(1, "{\"device\":\"PUMP\",\"status\":\"OFF\"}");
		}
	}
}
