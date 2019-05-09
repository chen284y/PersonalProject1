using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using GTW;
using WPMF;
using TMPro;

public class SliderReader : MonoBehaviour {

    TextMeshProUGUI TextM;
    public Slider taxslider;


	// Update is called once per frame
	void Update () {
        TextM = GetComponent<TextMeshProUGUI>();
        TextM.text = "+" + taxslider.value.ToString();
    }
}
