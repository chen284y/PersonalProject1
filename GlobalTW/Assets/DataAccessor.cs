using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using GTW;
using WPMF;
using TMPro;

public class DataAccessor : MonoBehaviour {

    public GameObject PieGraph;
    TextMeshProUGUI TextM;
    GlobalTW data;
    WorldMap2D map;
    // Use this for initialization
    public string keyname;
    public int key1, key2;

    private void Update()
    {
        float temp, zRotation;
        TextM = GetComponent<TextMeshProUGUI>();
        data = GlobalTW.instance;
        map = WorldMap2D.instance;

        switch (keyname)
        {
            case "myPro":
                TextM.text = data.countries.production[key1].ToString("F1");
                break;
            case "myPop":
                TextM.text = data.countries.population[key1].ToString("F1");
                break;
            case "myNeeds":
                TextM.text = data.countries.needs[key1].ToString("F1");
                break;
            case "facPro":
                zRotation = 0f;
                temp = data.countries.FactionPN[key1, key2] + data.countries.FactionPN[key1, key2 + 2] + data.countries.FactionPN[key1, key2 + 4];
                TextM.text = temp.ToString("F1");
                PieGraph.transform.GetChild(0).transform.GetComponent<Image>().fillAmount = data.countries.FactionPN[key1, key2] / temp;
                zRotation -= data.countries.FactionPN[key1, key2] / temp * 360f;
                PieGraph.transform.GetChild(1).transform.GetComponent<Image>().fillAmount = data.countries.FactionPN[key1, key2+2] / temp;
                PieGraph.transform.GetChild(1).transform.GetComponent<Image>().transform.rotation = Quaternion.Euler(new Vector3(0f,0f,zRotation));
                zRotation -= data.countries.FactionPN[key1, key2+2] / temp * 360f;
                PieGraph.transform.GetChild(2).transform.GetComponent<Image>().fillAmount = data.countries.FactionPN[key1, key2+4] / temp;
                PieGraph.transform.GetChild(2).transform.GetComponent<Image>().transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
                break;
            case "facNeeds":
                zRotation = 0f;
                if (data.countries.player == 0)
                    temp = data.countries.FactionPN[key1, 0];
                else
                    temp = data.countries.FactionPN[key1, 4];
                TextM.text = temp.ToString("F1");
                /*PieGraph.transform.GetChild(0).transform.GetComponent<Image>().fillAmount = 0.2f;
                zRotation -= 0.2f * 360f;
                PieGraph.transform.GetChild(1).transform.GetComponent<Image>().fillAmount = 0.5f;
                PieGraph.transform.GetChild(1).transform.GetComponent<Image>().transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
                zRotation -= 0.5f * 360f;
                PieGraph.transform.GetChild(2).transform.GetComponent<Image>().fillAmount = 0.3f;
                PieGraph.transform.GetChild(2).transform.GetComponent<Image>().transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));*/
                break;
            case "unemployment":
                TextM.text = data.countries.unemployment.ToString();
                break;
            case "unempRate":
                temp = data.countries.unemployment / (data.countries.unemployment + data.countries.population[0] + data.countries.population[1] + data.countries.population[2] + data.countries.population[3] + data.countries.population[4] + data.countries.population[5]);
                temp = temp * 100;
                TextM.text = temp.ToString("F2") + "% ";
                //TextM.text = "5%";
                break;
            case "WorldTension":
                TextM.text = data.countries.WorldTension.ToString();
                break;
            case "Atax":
                TextM.text = data.countries.Atax.ToString();
                break;
            case "Ctax":
                TextM.text = data.countries.Ctax.ToString();
                break;
            case "Date":
                double year = Math.Floor((double)data.countries.turnsN / 12) + 2018;
                string result = year + "-" + (data.countries.turnsN % 12 + 1);
                TextM.text = result;
                break;
            case "turnsN":
                TextM.text = data.countries.turnsN.ToString();
                break;
            case "LastCountryName":
                if (map.countryLastClicked != -1)
                    TextM.text = data.countries.names[map.countryLastClicked];
                break;
            case "LastCountryStanding":
                if (map.countryLastClicked != -1)
                {
                    int a = data.countries.player;
                    if (a == 0) a = -1;
                    TextM.text = (data.countries.standing[map.countryLastClicked] * a).ToString("F1");
                    if (data.countries.standing[map.countryLastClicked] <= -30)
                        TextM.color = new Color(0.1f, 0.1f, 0.8f);
                    else if (data.countries.standing[map.countryLastClicked] >= 30)
                        TextM.color = new Color(0.8f, 0.1f, 0.1f);
                    else
                        TextM.color = new Color(0.2f, 0.2f, 0.2f);
                }
                else
                    TextM.text = " ";
                break;
            case "LastCountryProduct":
                if (map.countryLastClicked != -1)
                    TextM.text = data.countries.details[map.countryLastClicked].a[key1].ToString();
                else
                    TextM.text = " ";
                break;
            case "incStanding":
                if (map.countryLastClicked != -1)
                {
                    int total = data.countries.details[map.countryLastClicked].a[0] + data.countries.details[map.countryLastClicked].a[1] + data.countries.details[map.countryLastClicked].a[2] + data.countries.details[map.countryLastClicked].a[3];
                    float relation;
                    if (total > 0)
                        relation = 60 / total;
                    else
                        relation = 60;
                    float b;
                    b = (data.countries.turnsN % 30) + 10;
                    b = ((map.countryLastClicked + 20) % b + 1) / b;
                    if (b >= 0.5)
                        TextM.color = new Color(0.0f, 0.8f, 0.0f);
                    else
                        TextM.color = new Color(0.8f, 0.0f, 0.0f);
                    relation = relation * (b*0.8f+0.5f);
                    if (relation > 30) relation = 30;
                    TextM.text = "+" + relation.ToString("F1");
                }
               // Debug.Log(map.countryLastClicked + " " + data.countries.turnsN + " ");
                break;
           /* default:
                TextM.text = data.countries.production[key1].ToString();
                break;*/
        }

    }
}
