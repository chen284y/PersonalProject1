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

    public void ExitExplain()
    {
        explain.enabled = false;
        ExplainText.enabled = false;
    }

    public void EnterExplain(string key)
    {
        explain.enabled = true;
        ExplainText.enabled = true;
        ExplainText.text = LocalizationManager.instance.GetLocalizedValue(key);
        Canvas.ForceUpdateCanvases();
        explain.GetComponent<HorizontalLayoutGroup>().enabled = false;
        explain.GetComponent<HorizontalLayoutGroup>().enabled = true;
    }

    public void StateVisit()
    {
        float b;
               
        if (map.countryLastClicked != -1)
        {
            int a = data.countries.player;
            if (a == 0) a = -1;
            int total = data.countries.details[map.countryLastClicked].a[0] + data.countries.details[map.countryLastClicked].a[1] + data.countries.details[map.countryLastClicked].a[2] + data.countries.details[map.countryLastClicked].a[3];
            float relation;
            if (total > 0)
                relation = 60 / total;
            else
                relation = 60;

            b = (data.countries.turnsN % 30) + 10;
            b = ((map.countryLastClicked + 20) % b + 1) / b;
            relation = relation * (b * 0.8f + 0.5f);
            if (relation > 30) relation = 30;
                    
            data.countries.standing[map.countryLastClicked] += relation * a;
            if (mapC == -1) MapChoice(-1);
            
        }

        EndTurn();
    }
    

    public void Fund(int button)
    {
        if (data.countries.unemployment == 0)
        {
            EventList.Enqueue(48);
            nextEvent(false);
        }
        else
        {
            if (button < 4)
            {
                if (data.countries.player == 0)
                {
                    data.countries.population[button] += (float)((1.3 * (data.countries.Atax * 0.533 + 60) / 100) / data.countries.production[button] * data.countries.population[button]);
                    data.countries.unemployment -= (float)( (1.3 * (data.countries.Atax * 0.533 + 60) / 100) / data.countries.production[button]  * data.countries.population[button]);
                    data.countries.production[button] += (float)(1.3 * (data.countries.Atax * 0.533 + 60) / 100);
                }
                else if (data.countries.player == 1)
                {
                    data.countries.population[button] += (float)((1.6 * (data.countries.Ctax * 0.533 + 60) / 100) / data.countries.production[button] * data.countries.population[button]);
                    data.countries.unemployment -= (float)((1.6 * (data.countries.Ctax * 0.533 + 60) / 100) / data.countries.production[button]  * data.countries.population[button]);
                    data.countries.production[button] += (float)(1.6 * (data.countries.Ctax * 0.533 + 60) / 100);
                }
            }
            else if (button == 4)
            {
                if (data.countries.player == 0)
                {
                    data.countries.population[4] += 4;
                    data.countries.unemployment -= 4;
                }
                else if (data.countries.player == 1)
                {
                    data.countries.population[4] += 5;
                    data.countries.unemployment -= 5;
                }
            }
            else if (button == 5)
            {
                if (data.countries.player == 0)
                {
                    data.countries.population[5] += 6;
                    data.countries.unemployment -= 6;
                }
                else if (data.countries.player == 1)
                {
                    data.countries.population[5] += 7;
                    data.countries.unemployment -= 7;
                }
            }
            if (data.countries.unemployment <= 0)
                data.countries.unemployment = 0;

            EndTurn();
        }
        
    }

    public void addTax(Slider a)
    {
        if (data.countries.player == 0)
        {
            data.countries.Atax += (int)a.value;
            if (data.countries.WorldTension < data.countries.Atax) data.countries.WorldTension = data.countries.Atax;
        }
        else
        {
            data.countries.Ctax += (int)a.value;
            if (data.countries.WorldTension < data.countries.Ctax) data.countries.WorldTension = data.countries.Ctax;
        }
        EndTurn();
    }

    bool IsGameEnd()
    {
        bool neutral = true;

        if (data.countries.WorldTension >= 300)
        {
            ending = 1;
            return true;
        }
        for (int i = 0; i < data.countries.size; i++)
        {
            if (data.countries.standing[i] >= -30 && data.countries.standing[i] <= 30)
            {
                neutral = false;
                break;
            }
        }
        if (neutral == true) ending = 2;
        return neutral;
    }

    void EnemyMove()
    {
        int choice = 0;
        int[] neutrals = new int[200];
        int[] valuable = new int[200];
        neutrals[0] = 0;
        valuable[0] = 0;

        if (data.countries.player == 1)
        {
            if(data.countries.turnsN%6 == 0 && data.countries.turnsN <= 90)
            {
                data.countries.Atax += 10;
                if (data.countries.WorldTension < data.countries.Atax) data.countries.WorldTension = data.countries.Atax;
                EventList.Enqueue(6);
            }
            else
            {
                if(lastvisit != -1)
                {
                    if(data.countries.standing[lastvisit] > -30 && data.countries.standing[lastvisit] < 1)
                    {
                        EventList.Enqueue(2);
                        int a = -data.countries.player;
                        if (a == 0) a = 1;
                        int total = data.countries.details[lastvisit].a[0] + data.countries.details[lastvisit].a[1] + data.countries.details[lastvisit].a[2] + data.countries.details[lastvisit].a[3];
                        float relation;
                        if (total > 0)
                            relation = 60 / total;
                        else
                            relation = 30;
                        if (relation > 30)
                            relation = 30;
                        data.countries.standing[lastvisit] += relation * a;
                        return;
                    }
                }
                lastvisit = -1;
                choice = UnityEngine.Random.Range(0, 100);
                if(choice < (55f + ( 300f - (float)data.countries.WorldTension) * 50f / 300f))
                {
                    for (int i = 0; i < data.countries.size; i++)
                        if (data.countries.standing[i] > -30 && data.countries.standing[i] < 15)
                        {
                            neutrals[0]++;
                            neutrals[neutrals[0]] = i;
                            if (data.countries.details[i].a[1] > 1 || data.countries.details[i].a[3] > 1)
                            {
                                valuable[0]++;
                                valuable[valuable[0]] = i;
                            }                               
                        }
                    if (valuable[0] > 0)
                        lastvisit = valuable[UnityEngine.Random.Range(1, valuable[0])];
                    else
                        lastvisit = neutrals[UnityEngine.Random.Range(1, neutrals[0])];
                    EventList.Enqueue(2);

                    int a = -data.countries.player;
                    if (a == 0) a = 1;
                    int total = data.countries.details[lastvisit].a[0] + data.countries.details[lastvisit].a[1] + data.countries.details[lastvisit].a[2] + data.countries.details[lastvisit].a[3];
                    float relation;
                    if (total > 0)
                        relation = 60 / total;
                    else
                        relation = 30;
                    if (relation > 30)
                        relation = 30;
                    data.countries.standing[lastvisit] += relation * a;
                }
                else
                {
                    EventList.Enqueue(4);
                }
                
            }
        }
        else
        {
            if (lastvisit != -1)
            {
                if (data.countries.standing[lastvisit] < 30 && data.countries.standing[lastvisit] > -1)
                {
                    EventList.Enqueue(2);
                    int a = -data.countries.player;
                    if (a == 0) a = 1;
                    int total = data.countries.details[lastvisit].a[0] + data.countries.details[lastvisit].a[1] + data.countries.details[lastvisit].a[2] + data.countries.details[lastvisit].a[3];
                    float relation;
                    if (total > 0)
                        relation = 60 / total;
                    else
                        relation = 30;
                    if (relation > 30)
                        relation = 30;
                    data.countries.standing[lastvisit] += relation * a;
                    return;
                }
            }
            lastvisit = -1;
            choice = UnityEngine.Random.Range(0, 100);
            if (choice < (25 + (300 - data.countries.WorldTension) * 50))
            {
                for (int i = 0; i < data.countries.size; i++)
                    if (data.countries.standing[i] > -15 && data.countries.standing[i] < 30)
                    {
                        neutrals[0]++;
                        neutrals[neutrals[0]] = i;
                        if (data.countries.details[i].a[1] > 1 || data.countries.details[i].a[3] > 1)
                        {
                            valuable[0]++;
                            valuable[valuable[0]] = i;
                        }
                    }
                if (valuable[0] > 0)
                    lastvisit = valuable[UnityEngine.Random.Range(1, valuable[0])];
                else
                    lastvisit = neutrals[UnityEngine.Random.Range(1, neutrals[0])];
                EventList.Enqueue(3);

                int a = -data.countries.player;
                if (a == 0) a = 1;
                int total = data.countries.details[lastvisit].a[0] + data.countries.details[lastvisit].a[1] + data.countries.details[lastvisit].a[2] + data.countries.details[lastvisit].a[3];
                float relation;
                if (total > 0)
                    relation = 60 / total;
                else
                    relation = 30;
                if (relation > 30)
                    relation = 30;
                data.countries.standing[lastvisit] += relation * a;
            }
            else
            {
                EventList.Enqueue(5);
            }
        }

    }

    

    public void Redraw()
    {/*
        if (map.countryLastClicked != -1)
        {
            //CountryName.text = map.countries[map.countryLastClicked].name;
            
            n1.text = data.countries.details[map.countryLastClicked].a[0].ToString("F2") + "\n" + data.countries.details[map.countryLastClicked].a[1].ToString("F2") + "\n" + data.countries.details[map.countryLastClicked].a[2].ToString("F2") + "\n" + data.countries.details[map.countryLastClicked].a[3].ToString("F2");
            
            if (data.countries.player == 0)
                n2.text = -data.countries.standing[map.countryLastClicked] + "";
            else
                n2.text = data.countries.standing[map.countryLastClicked] + "";

            if (data.countries.standing[map.countryLastClicked] <= -30)
                n2.color = new Color(0.1f, 0.1f, 0.8f);
            else if (data.countries.standing[map.countryLastClicked] >= 30)
                n2.color = new Color(0.8f, 0.1f, 0.1f);
            else
                n2.color = new Color(0.2f, 0.2f, 0.2f);

            CountryName.text = map.countryLastClicked+" ";
        }*/
        //CountryDetail.enabled = true;
        //CountryName.enabled = true;
        MessageHistory.text = "";
        foreach (string i in History.ToArray())
        {
            MessageHistory.text += i + '\n';
        }
       
    }

    void MapChoice(int key)
    {
        mapC = key;
        Color color;
        switch (key)
        {
            case -1:
                
                for (int colorizeIndex = 0; colorizeIndex < map.countries.Length; colorizeIndex++)
                {
                    if (colorizeIndex != 30 && colorizeIndex != 168)
                    {
                        if (data.countries.standing[colorizeIndex] <= -30f)
                        {
                            color = new Color(0.2f, 0.2f, 0.9f, 0.92f);
                        }
                        else if (data.countries.standing[colorizeIndex] >= 30f)
                        {
                            color = new Color(0.9f, 0.2f, 0.2f, 0.92f);
                        }
                        else
                        {
                            color = new Color(0.7f, 0.7f, 0.7f, 0.92f);
                        }
                        map.ToggleCountrySurface(map.countries[colorizeIndex].name, true, color);
                    }                    
                }
                color = new Color(0.15f, 0.15f, 1.0f, 0.92f);
                map.ToggleCountrySurface(map.countries[168].name, true, color);
                color = new Color(1.0f, 0.15f, 0.15f, 0.92f);
                map.ToggleCountrySurface(map.countries[30].name, true, color);
                break;
            case 0:
                for (int colorizeIndex = 0; colorizeIndex < map.countries.Length; colorizeIndex++)
                {
                    if (colorizeIndex != 30 && colorizeIndex != 168)
                    {
                        float a = (float)data.countries.details[colorizeIndex].a[0] / 18f;
                        color = new Color(1.0f-a, 1f, 1.0f-a);
                        map.ToggleCountrySurface(map.countries[colorizeIndex].name, true, color);
                    }
                }
                color = new Color(0.7f, 0.7f, 0.7f, 0.92f);
                map.ToggleCountrySurface(map.countries[168].name, true, color);
                map.ToggleCountrySurface(map.countries[30].name, true, color);
                break;
            case 1:
                for (int colorizeIndex = 0; colorizeIndex < map.countries.Length; colorizeIndex++)
                {
                    if (colorizeIndex != 30 && colorizeIndex != 168)
                    {
                        float a = (float)data.countries.details[colorizeIndex].a[1] / 26f;
                        color = new Color(1 - a, 1f, 1 - a);
                        map.ToggleCountrySurface(map.countries[colorizeIndex].name, true, color);
                    }
                }
                color = new Color(0.7f, 0.7f, 0.7f, 0.92f);
                map.ToggleCountrySurface(map.countries[168].name, true, color);
                map.ToggleCountrySurface(map.countries[30].name, true, color);
                break;
            case 2:
                for (int colorizeIndex = 0; colorizeIndex < map.countries.Length; colorizeIndex++)
                {
                    if (colorizeIndex != 30 && colorizeIndex != 168)
                    {
                        float a = (float)data.countries.details[colorizeIndex].a[2] / 28f;
                        color = new Color(1 - a, 1f, 1 - a);
                        map.ToggleCountrySurface(map.countries[colorizeIndex].name, true, color);
                    }
                }
                color = new Color(0.7f, 0.7f, 0.7f, 0.92f);
                map.ToggleCountrySurface(map.countries[168].name, true, color);
                map.ToggleCountrySurface(map.countries[30].name, true, color);
                break;
            case 3:
                for (int colorizeIndex = 0; colorizeIndex < map.countries.Length; colorizeIndex++)
                {
                    if (colorizeIndex != 30 && colorizeIndex != 168)
                    {
                        float a = (float)data.countries.details[colorizeIndex].a[3] / 25f;
                        color = new Color(1 - a, 1f, 1 - a);
                        map.ToggleCountrySurface(map.countries[colorizeIndex].name, true, color);
                    }
                }
                color = new Color(0.7f, 0.7f, 0.7f, 0.92f);
                map.ToggleCountrySurface(map.countries[168].name, true, color);
                map.ToggleCountrySurface(map.countries[30].name, true, color);
                break;
        }
    }


    // a 4x6 array, 6 for : Ally Production, Ally Need, Neutral Production, Neutral Need, Enemy Production, Enemy Need
    void FactionCount()
    {
        /*
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
                data.countries.FactionPN[i, j] = 0;
            data.countries.FactionPN[i, 0] += data.countries.production[i];
            data.countries.FactionPN[i, 1] += data.countries.needs[i];
        }*/
        float a, c, t;

        for(int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
                data.countries.FactionPN[j, i] = 0;
        }

        for (int i = 0; i < data.countries.size; i++)
        {

            for (int j = 0; j < 4; j++) 
            {
                if (data.countries.standing[i] <= -30)
                {
                    data.countries.FactionPN[j, 0] += data.countries.details[i].a[j];
                    data.countries.FactionPN[0, 1] += data.countries.details[i].a[j];
                }
                else if (data.countries.standing[i] >= 30)
                {
                    data.countries.FactionPN[j, 4] += data.countries.details[i].a[j];
                    data.countries.FactionPN[0, 5] += data.countries.details[i].a[j];
                }
                else
                {
                    data.countries.FactionPN[j, 2] += data.countries.details[i].a[j];
                    data.countries.FactionPN[0, 3] += data.countries.details[i].a[j];
                }
                data.countries.FactionPN[1, 1] += data.countries.details[i].a[j]; ;
            }
        }

        a = data.countries.FactionPN[0, 1] * 100 / 200;
        c = data.countries.FactionPN[0, 5] * 100 / 200;
        t = a + c;
        if(t > 100)
        {
            a = a / t;
            c = c / t;
        }
        Debug.Log("Aslider: " + data.countries.FactionPN[0, 1] + "/" + data.countries.FactionPN[1, 1] + " " + "Cslider: " + data.countries.FactionPN[0, 5] + "/" + data.countries.FactionPN[1, 1]);
    }
}
