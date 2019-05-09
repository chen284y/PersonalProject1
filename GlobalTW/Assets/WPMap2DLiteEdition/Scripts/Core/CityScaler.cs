using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WPMF
{
	/// <summary>
	/// City scaler. Checks the city icons' size is always appropiate
	/// </summary>
	public class CityScaler : MonoBehaviour
	{

		const int CITY_SIZE_ON_SCREEN = 10;
		Vector3 lastCamPos, lastPos;
		float lastIconSize;
		float lastCustomSize;

		[NonSerialized]
		public WorldMap2D map;

		void Start ()
		{
			ScaleCities ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (lastPos == transform.position && lastCamPos == Camera.main.transform.position && lastIconSize == map.cityIconSize)
				return;
			ScaleCities ();
		}

		public void ScaleCities ()
		{
			lastPos = transform.position;
			lastCamPos = Camera.main.transform.position;
			lastIconSize = map.cityIconSize;

			Vector3 a =Camera.main.WorldToScreenPoint(transform.position);
			Vector3 b = new Vector3(a.x, a.y + CITY_SIZE_ON_SCREEN, a.z);
			if (Camera.main.pixelWidth==0) return; // Camera pending setup
			Vector3 aa = Camera.main.ScreenToWorldPoint(a);
			Vector3 bb = Camera.main.ScreenToWorldPoint(b);
			float scale = (aa - bb).magnitude * map.cityIconSize;
			scale /= 1 + (lastCamPos - lastPos).sqrMagnitude * (0.1f / map.transform.localScale.x);
			Vector3 newScale = new Vector3 (scale / WorldMap2D.mapWidth, scale / WorldMap2D.mapHeight, 1.0f);
			foreach (Transform t in transform)
				t.localScale = newScale;
		}

		public void ScaleCities(float customSize) {
			if (customSize==lastCustomSize) return;
			lastCustomSize = customSize;
			Vector3 newScale = new Vector3(customSize/ WorldMap2D.mapWidth, customSize / WorldMap2D.mapHeight, 1);
			foreach (Transform t in transform)
				t.localScale = newScale;
		}
	}

}