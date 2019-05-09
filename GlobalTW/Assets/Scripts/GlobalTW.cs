using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;

namespace GTW
{
    public partial class GlobalTW : MonoBehaviour
    {
        public Countries countries;

        public int PlayerN;

        static GlobalTW _instance;

        #region Gameloop events

        void OnEnable()
        {
            if (countries == null)
            {
                PlayerN = PlayerPrefs.GetInt("player");
                Init();
            }
        }

        #endregion


        #region System initialization
        public void Init()
        {
            ReadCountries();

            

            /*
            Debug.Log("player : " + countries.player);

            
            string output = "Production : ";
            for (int i = 0; i < 7; i++)
            {
                output += countries.production[i];
                output += "  ";
            }
            Debug.Log(output);

            output = "Population : ";
            for (int i = 0; i < 7; i++)
            {
                output += countries.population[i];
                output += "  ";
            }
            Debug.Log(output);

            output = "Needs : ";
            for (int i = 0; i < 7; i++)
            {
                output += countries.needs[i];
                output += "  ";
            }
            Debug.Log(output);

            Debug.Log("Unemployment : " + countries.unemployment);

            Debug.Log("Income : " + countries.income);
            Debug.Log(countries.player + " "+ countries.production[0] + " " + countries.population[6] + " " + countries.needs[0]);
            Debug.Log(countries.unemployment + " " + countries.size + " " + countries.names[0] + " " + countries.standing[1]);*/
        }
        #endregion

        public static GlobalTW instance
        {
            get
            {
                if(_instance == null)
                {
                    GameObject obj = GameObject.Find("GlobalTW");
                    if (obj == null)
                    {
                        Debug.LogWarning("'GlobalTW' GameObject could not be found in the scene. Make sure it's created with this name before using any GlobalTW functionality.");
                    }
                    else
                    {
                        _instance = obj.GetComponent<GlobalTW>();
                    }
                }
                return _instance;
            }
        }
        
        public void setPlayer(int select)
        {
            countries.player = select;
        }
    }

    
}