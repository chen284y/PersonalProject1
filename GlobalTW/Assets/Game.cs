using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WPMF;

public class Game : MonoBehaviour {

    WorldMap2D map;

    Text CountryName;

	// Use this for initialization
	void Start () {
        map = WorldMap2D.instance;

        map.CenterMap();



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
