using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizedText : MonoBehaviour {

    TextMeshProUGUI TextM;
    public string key, ending;
	// Use this for initialization
	void Start () {
        if(ending != "True")
        {
            TextM = GetComponent<TextMeshProUGUI>();
            TextM.text = LocalizationManager.instance.GetLocalizedValue(key);
        }
        else
        {
            TextM = GetComponent<TextMeshProUGUI>();
            int temp = PlayerPrefs.GetInt(key);
            TextM.text = LocalizationManager.instance.GetLocalizedValue(key + temp.ToString());
            //Debug.Log(key + temp.ToString());
        }
        
    }
	
	
}
