using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTW;
using WPMF;
using TMPro;
using System;

public partial class GameManager : MonoBehaviour
{


    void gameEvent(int key, bool choice)
    {
        string[] value;
        string[] value2, value3;
        int listIndex = data.countries.gevents.FindIndex(x => x.key == key);
        gevent gtemp = new gevent();
        if (key >= 100 && key < 200)
        {
            value = LocalizationManager.instance.GetLocalizedValue(key.ToString()).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            value2 = value[1].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < 6; i++)
            {
                if (i < 4)
                {
                    data.countries.production[i] += float.Parse(value2[i]);
                    data.countries.population[i] += float.Parse(value2[i]) / data.countries.production[i] * data.countries.population[i];
                    data.countries.unemployment -= float.Parse(value2[i]) / data.countries.production[i] * data.countries.population[i];
                }
                else
                {
                    data.countries.population[i] += float.Parse(value2[i]);
                    data.countries.unemployment -= float.Parse(value2[i]);
                }
            }
            data.countries.WorldTension += int.Parse(value2[6]);
            if (data.countries.unemployment < 0)
                data.countries.unemployment = 0;
            if (value.Length > 3)
                data.countries.gevents.Remove(new gevent() { key = int.Parse(value[3]) });
            data.countries.gevents.Remove(new gevent() { key = key });
            Debug.Log(data.countries.gevents.Count);
        }
        else if(key < 225)
        {
            value = LocalizationManager.instance.GetLocalizedValue((key + data.countries.gevents[listIndex].status).ToString()).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (key < 210)
            {
                value2 = value[1].Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                if (choice)
                {
                    if (data.countries.gevents[listIndex].status >= data.countries.gevents[listIndex].size)
                        data.countries.gevents.Remove(new gevent() { key = key });
                    else
                    {
                        gtemp = data.countries.gevents[listIndex];
                        gtemp.status += 1;
                        data.countries.gevents[listIndex] = gtemp;
                    }
                    value3 = value2[0].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    data.countries.gevents.Remove(new gevent() { key = key });
                    value3 = value2[1].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                }
                for (int i = 0; i < 6; i++)
                {
                    if (i < 4)
                    {
                        data.countries.production[i] += float.Parse(value3[i]);
                        data.countries.population[i] += float.Parse(value3[i]) / data.countries.production[i] * data.countries.population[i];
                        data.countries.unemployment -= float.Parse(value3[i]) / data.countries.production[i] * data.countries.population[i];
                    }
                    else
                    {
                        data.countries.population[i] += float.Parse(value3[i]);
                        data.countries.unemployment -= float.Parse(value3[i]);
                    }
                }
                data.countries.WorldTension += int.Parse(value3[6]);
            }
            else if(key == 211)
            {
                if (int.Parse(value[3]) == 2)
                    ending = 5;
                else
                {
                    if (choice)
                    {
                        gtemp = data.countries.gevents[listIndex];
                        gtemp.status += 1;
                        data.countries.gevents[listIndex] = gtemp;
                    }
                    else
                    {
                        data.countries.gevents.Remove(new gevent() { key = key });
                    }
                }
            

            }
            else if(key == 215)
            {
                if (int.Parse(value[3]) == 2)
                    ending = 6;
                else
                {
                    if (choice)
                    {
                        gtemp = data.countries.gevents[listIndex];
                        gtemp.status += 1;
                        data.countries.gevents[listIndex] = gtemp;
                    }
                    else
                    {
                        data.countries.gevents.Remove(new gevent() { key = key });
                    }
                }
            }
            else if(key == 220)
            {
                if (int.Parse(value[3]) == 0 && choice)
                {
                    gtemp = data.countries.gevents[listIndex];
                    gtemp.status += int.Parse(UnityEngine.Random.Range(1f,4f).ToString());
                    data.countries.gevents[listIndex] = gtemp;
                }
                else if(int.Parse(value[3]) != 0)
                {
                    data.countries.gevents.Remove(new gevent() { key = key });
                    data.countries.WorldTension += int.Parse(value[1]);
                }
            }

        }
        else
            Debug.Log("event excuted : " + key.ToString());
    }

    void boolEvent(int key, bool choice)
    {
        string[] value = LocalizationManager.instance.GetLocalizedValue(key.ToString()).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        string[] value2;
        if (key >= 100 && key < 200)
        {
            value2 = value[1].Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < 6; i++) {
                if (i < 4)
                {
                    data.countries.production[i] += float.Parse(value2[i]);
                    data.countries.population[i] += float.Parse(value2[i]) / data.countries.production[i] * data.countries.population[i];
                    data.countries.unemployment -= float.Parse(value2[i]) / data.countries.production[i] * data.countries.population[i];
                }
                else
                {
                    data.countries.population[i] += float.Parse(value2[i]);
                    data.countries.unemployment -= float.Parse(value2[i]);
                }
            }
            data.countries.WorldTension += int.Parse(value2[6]);
        }
    }
    
    
}
