using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    public Transform RadialBar;
	public Transform TextValue;
	int typeofGauge = 1; // 1 for temperature, 2 for humidity
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void setTypeofGauge (int typeValue)
	{
		typeofGauge = typeValue;
	}
	
	public void updateGauge(float valueUpdated)
	{
		if (typeofGauge == 1)
			TextValue.GetComponent<Text>().text = valueUpdated.ToString() + "°C";
		else if (typeofGauge == 2)
			TextValue.GetComponent<Text>().text = valueUpdated.ToString() + "%";
		if (valueUpdated <= 100)
			RadialBar.GetComponent<Image>().fillAmount = valueUpdated / 100;
		else
			RadialBar.GetComponent<Image>().fillAmount = 1;
	}
}
