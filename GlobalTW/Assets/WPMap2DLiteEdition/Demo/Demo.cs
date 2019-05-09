using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WPMF;

public class Demo : MonoBehaviour {

	WorldMap2D map;
	GUIStyle labelStyle, labelStyleShadow, buttonStyle, sliderStyle, sliderThumbStyle;
	ColorPicker colorPicker;
	bool changingFrontiersColor;
	bool minimizeState = false;

	void Start () {
		// Get a reference to the World Map API:
		map = WorldMap2D.instance;

		// UI Setup - non-important, only for this demo
		labelStyle = new GUIStyle ();
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.normal.textColor = Color.white;
		labelStyleShadow = new GUIStyle (labelStyle);
		labelStyleShadow.normal.textColor = Color.black;
        /*
        buttonStyle = new GUIStyle (labelStyle);
		buttonStyle.alignment = TextAnchor.MiddleLeft;
		buttonStyle.normal.background = Texture2D.whiteTexture;
		buttonStyle.normal.textColor = Color.white;
		colorPicker = gameObject.GetComponent<ColorPicker> ();
		sliderStyle = new GUIStyle ();
		sliderStyle.normal.background = Texture2D.whiteTexture;
		sliderStyle.fixedHeight = 4.0f;
		sliderThumbStyle = new GUIStyle ();
		sliderThumbStyle.normal.background = Resources.Load<Texture2D> ("thumb");
		sliderThumbStyle.overflow = new RectOffset (0, 0, 8, 0);
		sliderThumbStyle.fixedWidth = 20.0f;
		sliderThumbStyle.fixedHeight = 12.0f;*/

		// setup GUI resizer - only for the demo
		GUIResizer.Init (800, 500); 

		map.CenterMap(); // Center map on the screen

//			map.ToggleCountrySurface("Brazil", true, Color.green);
//			map.ToggleCountrySurface(35, true, Color.green);
//			map.ToggleCountrySurface(33, true, Color.green);
//			map.FlyToCountry(33);
//			map.FlyToCountry("Brazil");

//			map.navigationTime = 0;
//			map.FlyToCountry ("India");

	}
	
	// Update is called once per frame
	void OnGUI () {
		// Do autoresizing of GUI layer
		GUIResizer.AutoResize ();

		// Check whether a country or city is selected, then show a label
		if (map.countryHighlighted != null || map.cityHighlighted != null) {
//			Vector3 mousePos = Input.mousePosition;
			string text;
			if (map.cityHighlighted != null) {
				text = "City: " + map.cityHighlighted.name + " (" + map.cityHighlighted.country + ")";
			} else if (map.countryHighlighted != null) {
				text = map.countryHighlighted.name + " (" + map.countryHighlighted.continent + ")";
			} else {
				text = "";
			}
			float x ,y;
			if (minimizeState) {
				x = Screen.width - 130;
				y = Screen.height - 140;
			} else {
				x = Screen.width / 2.0f;
				y = Screen.height - 40;
			}
			GUI.Label (new Rect (x - 1, y - 1, 0, 10), text, labelStyleShadow);
			GUI.Label (new Rect (x + 1, y + 2, 0, 10), text, labelStyleShadow);
			GUI.Label (new Rect (x + 2, y + 3, 0, 10), text, labelStyleShadow);
			GUI.Label (new Rect (x + 3, y + 4, 0, 10), text, labelStyleShadow);
			GUI.Label (new Rect (x, y, 0, 10), text, labelStyle);
		}
        /*
		// Assorted options to show/hide frontiers, cities, Earth and enable country highlighting
		GUI.Box (new Rect(0,0,150,200), "");
		map.showFrontiers = GUI.Toggle (new Rect (10, 20, 150, 30), map.showFrontiers, "Toggle Frontiers");
		map.showEarth = GUI.Toggle (new Rect (10, 50, 150, 30), map.showEarth, "Toggle Earth");
		map.showCities = GUI.Toggle (new Rect (10, 80, 150, 30), map.showCities, "Toggle Cities");
		map.showCountryNames = GUI.Toggle (new Rect (10, 110, 150, 30), map.showCountryNames, "Toggle Labels");
		map.enableCountryHighlight = GUI.Toggle (new Rect (10, 140, 170, 30), map.enableCountryHighlight, "Enable Highlight");
        */
		GUI.backgroundColor = new Color (0.1f, 0.1f, 0.3f, 0.95f);
        /*
		// Add buttons to show the color picker and change colors for the frontiers or fill
		if (GUI.Button (new Rect (10, 210, 160, 30), "  Change Frontiers Color", buttonStyle)) {
			colorPicker.showPicker = true;
			changingFrontiersColor = true;
		}
		if (GUI.Button (new Rect (10, 250, 160, 30), "  Change Fill Color", buttonStyle)) {
			colorPicker.showPicker = true;
			changingFrontiersColor = false;
		}
		if (colorPicker.showPicker) {
			if (changingFrontiersColor) {
				map.frontiersColor = colorPicker.setColor;
			} else {
				map.fillColor = colorPicker.setColor;
			}
		}
        
		// Add a button which demonstrates the navigateTo functionality -- pass the name of a country
		// For a list of countries and their names, check map.Countries collection.
		if (GUI.Button (new Rect (10, 290, 180, 30), "  Fly to Australia (Country)", buttonStyle)) {
			FlyToCountry ("Australia");
		}
		if (GUI.Button (new Rect (10, 325, 180, 30), "  Fly to Mexico (Country)", buttonStyle)) {
			FlyToCountry ("Mexico");
		}
		if (GUI.Button (new Rect (10, 360, 180, 30), "  Fly to San Francisco (City)", buttonStyle)) {
			FlyToCity ("San Francisco");
		}
		if (GUI.Button (new Rect (10, 395, 180, 30), "  Fly to Madrid (City)", buttonStyle)) {
			FlyToCity ("Madrid");
		}

		// Add a button to colorize countries
		if (GUI.Button (new Rect (GUIResizer.authoredScreenWidth - 190, 20, 180, 30), "  Colorize Europe", buttonStyle)) {
			for (int colorizeIndex =0; colorizeIndex < map.countries.Length; colorizeIndex++) {
				if (map.countries [colorizeIndex].continent.Equals ("Europe")) {
					Color color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
					map.ToggleCountrySurface (map.countries [colorizeIndex].name, true, color);
				}
			}
		}

		// Colorize random country and fly to it
		if (GUI.Button (new Rect (GUIResizer.authoredScreenWidth - 190, 60, 180, 30), "  Colorize Random", buttonStyle)) {
			int countryIndex = Random.Range (0, map.countries.Length);
			Color color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			map.ToggleCountrySurface (countryIndex, true, color);
			map.BlinkCountry(countryIndex, Color.green, Color.black, 0.8f, 0.2f);
		}
			
		// Button to clear colorized countries
		if (GUI.Button (new Rect (GUIResizer.authoredScreenWidth - 190, 100, 180, 30), "  Reset countries", buttonStyle)) {
			map.HideCountrySurfaces ();
		}

		// Moving the Earth sample
		if (GUI.Button (new Rect (GUIResizer.authoredScreenWidth - 190, 220, 180, 30), "  Toggle Minimize", buttonStyle)) {
			ToggleMinimize ();
		}*/
	}

	void FlyToCountry(int countryIndex) {
		map.FlyToCountry(countryIndex, 2.0f);
		map.BlinkCountry(countryIndex, Color.green, Color.black, 3.0f, 0.2f);
	}

	void FlyToCountry(string countryName) {
		int countryIndex = map.GetCountryIndex(countryName);
		FlyToCountry(countryIndex);
	}

	void FlyToCity(string cityName) {
		map.FlyToCity(cityName, 2.0f);
	}


	// The globe can be moved and scaled at wish
	void ToggleMinimize() {
		minimizeState = !minimizeState;

		Camera.main.transform.position = Vector3.back * 1.1f;
		Camera.main.transform.rotation = Quaternion.Euler(Vector3.zero);
		if (minimizeState) {
			map.earthColor = Color.black;
			map.longitudeStepping = 4;
			map.latitudeStepping = 40;
			map.showCities = false;
			map.showCountryNames = false;
			map.gridLinesColor = new Color (0.06f, 0.23f, 0.398f);
			map.fitWindowWidth = false;
			map.fitWindowHeight = false;
			map.gameObject.transform.localScale = new Vector3(0.4f, 0.2f, 1);
			map.gameObject.transform.position = new Vector3(0.9f, -0.5f, 0);
		} else {
			map.gameObject.transform.localScale = new Vector3(200,100,0);
			map.CenterMap();
			map.longitudeStepping = 15;
			map.latitudeStepping = 15;
			map.showCities = true;
			map.showCountryNames = true;
			map.gridLinesColor = new Color (0.16f, 0.33f, 0.498f);
			map.fitWindowWidth = true;
			map.fitWindowHeight = true;
		}

	}

}

