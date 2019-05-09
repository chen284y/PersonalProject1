using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GTW
{

    public partial class GlobalTW : MonoBehaviour
    {
        
        
        Dictionary<string, int> _countryLookup;
        int lastCountryLookupCount = -1;

        Dictionary<string, int> countryLookup
        {
            get
            {
                if (_countryLookup != null && countries.size == lastCountryLookupCount)
                    return _countryLookup;
                if (_countryLookup == null)
                {
                    _countryLookup = new Dictionary<string, int>();
                }
                else
                {
                    _countryLookup.Clear();
                }
                for (int k = 0; k < countries.size; k++)
                    //_countryLookup.Add(countries.name[k], k);
                lastCountryLookupCount = _countryLookup.Count;
                return _countryLookup;
            }

        }

        public void ReadCountries()
        {
            string s;
            int intemp;
            string[] data2;
            lastCountryLookupCount = -1;
            string filename;
            if (PlayerN == -1)
                filename = "saving/saves";
            else
                filename = "GameData";
            TextAsset ta = Resources.Load<TextAsset>(filename);
            if (ta == null)
            {
                Debug.Log("no text read!");
            }
            
        
            s = ta.text;
            string[] data = s.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            data2 = data[0].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            intemp = int.Parse(data2[0]);
            if (countries == null)
            {
                if (PlayerN == -1)
                    countries = new Countries(intemp);
                else
                    countries = new Countries(PlayerN);
            }
            else
            {
                if (PlayerN != -1)
                    countries.player = PlayerN;
            }
            countries.WorldTension = int.Parse(data2[1]);
            countries.Atax = int.Parse(data2[2]);
            countries.Ctax = int.Parse(data2[3]);
            countries.turnsN = int.Parse(data2[4]);
            countries.AP = int.Parse(data2[5]);

            //Debug.Log(filename + " " + PlayerN.ToString() + " " + countries.player.ToString() + " " + countries.turnsN.ToString());

            countries.FactionPN = new float[4,6];

            countries.unemployment = new float();
            if (countries.player == 0)
            {
                data2 = data[1].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                countries.production = new float[6];
                for (int i = 0; i < 6; i++)
                {
                    countries.production[i] = float.Parse(data2[i]);
                }

                data2 = data[2].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                countries.population = new float[6];
                for (int i = 0; i < 6; i++)
                {
                    countries.population[i] = float.Parse(data2[i]);
                }

                data2 = data[3].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                countries.needs = new float[6];
                for (int i = 0; i < 6; i++)
                {
                    countries.needs[i] = float.Parse(data2[i]);
                }
                countries.unemployment = float.Parse(data2[6]);
            }
            else if(countries.player == 1)
            {
                data2 = data[4].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                countries.production = new float[6];
                for (int i = 0; i < 6; i++)
                {
                    countries.production[i] = float.Parse(data2[i]);
                }

                data2 = data[5].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                countries.population = new float[6];
                for (int i = 0; i < 6; i++)
                {
                    countries.population[i] = float.Parse(data2[i]);
                }

                data2 = data[6].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                countries.needs = new float[6];
                for (int i = 0; i < 6; i++)
                {
                    countries.needs[i] = float.Parse(data2[i]);
                }
                countries.unemployment = float.Parse(data2[6]);
            }

            //IsPass150, event1, event2, event3, event4
            //floatemp = float.Parse(data[7]);
            //countries.unemployment = floatemp;
            data2 = data[7].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            countries.gevents = new List<gevent>();
            for(int i = 0; i < data2.Length; i++)
            {
                //Debug.Log(data2[i]);
                string[] data3 = data2[i].Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                //Debug.Log(data3[0].ToString() + " " + data3[1].ToString() + " " + data3[2].ToString() + " ");
                gevent temp = new gevent();
                temp.key = int.Parse(data3[0]);
                temp.status = int.Parse(data3[1]);
                temp.size = int.Parse(data3[2]);
                countries.gevents.Add(temp);
            }



            countries.size = new int();
            countries.size = int.Parse(data[8]);

            

            

            
            countries.index = new int[countries.size];
            countries.names = new string[countries.size];
            countries.details = new cdetail[countries.size];
            countries.standing = new float[countries.size];
            
            //data2 = data[7].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < countries.size; i++)
            {
                string[] data3 = data[i+9].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                countries.index[i] = int.Parse(data3[0]);
                countries.names[i] = data3[1];
                countries.standing[i] = float.Parse(data3[2]);
                countries.details[i].a = new int[4];
                for(int j = 0; j < 4; j++)
                {
                    countries.details[i].a[j] = int.Parse(data3[j + 3]);
                }

            }

            countries.ready = true;
            /*for(int i = 6; i < 8; i++)
            {
                string[] data3 = data[i].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                Debug.Log(data3[0] + data3[1]);
            }*/
        }

    }
}
