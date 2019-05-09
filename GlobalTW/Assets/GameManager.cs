using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using GTW;
using WPMF;
using TMPro;

public partial class GameManager : MonoBehaviour
{
    public Image CountryDetail;
    public TextMeshProUGUI worldTensionText;
    public TextMeshProUGUI CellText,ExplainText,MessageHistory;
    public Image Cell,CellImage,flag, explain;
    public Sprite[] eventImages;
    public Button[] actions;

    Queue EventList = new Queue();
    Queue History = new Queue();

    int ending;
    int mapC;
    WorldMap2D map;
    GlobalTW data;
    GUIStyle labelStyle, labelStyleShadow, buttonStyle, sliderStyle, sliderThumbStyle;
    ColorPicker colorPicker;
    public Slider Aslider, Cslider,Aparl;
    bool changingFrontiersColor;
    int currentEvent, lastvisit;
    

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        currentEvent = -1;
        map = WorldMap2D.instance;
        data = GlobalTW.instance;
        //data.Init();
        map.CenterMap(); // Center map on the screen

        FactionCount();
        MapChoice(-1);
        EventList.Enqueue(1);
        if (data.countries.player == 0)
        {
            EventList.Enqueue(32);
            EventList.Enqueue(33);
        }
        else if (data.countries.player == 1)
        {
            EventList.Enqueue(34);
            EventList.Enqueue(35);
        }
        nextEvent(false);

        flag.sprite = eventImages[data.countries.player];
        //WriteNewCountryList();
        explain.enabled = false;
        ExplainText.enabled = false;
        ending = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //worldTensionText.text = data.countries.player + " " + data.countries.WorldTension + " " + data.countries.Atax + " " + data.countries.Ctax + " " + data.countries.turnsN;
 
        
        if (Input.mousePosition.x >= (Screen.width - explain.rectTransform.rect.width))
            explain.transform.position = Input.mousePosition + new Vector3(-explain.rectTransform.rect.width / 2-10, -explain.rectTransform.rect.height / 2, 0f);
        else if (Input.mousePosition.y <= (explain.rectTransform.rect.height))
            explain.transform.position = Input.mousePosition + new Vector3(explain.rectTransform.rect.width / 2+10, explain.rectTransform.rect.height / 2, 0f);
        else
            explain.transform.position = Input.mousePosition + new Vector3(explain.rectTransform.rect.width / 2+10 , -explain.rectTransform.rect.height/2  , 0f);
    }


    public void DisplayDetail()
    {
        CountryDetail.gameObject.SetActive(false);
    }

    public void CenterMap()
    {
        map.CenterMap();
        CountryDetail.gameObject.SetActive(true);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void EndTurn()
    {
        IsGameEnd();

        if (ending != 0)
        {
            PlayerPrefs.SetInt("ending", ending);
            EndingCount();
            StartCoroutine(LoadNextScene());            
        }

        for (int i = 0; i < data.countries.size; i++)
        {
            if (data.countries.standing[i] >= 30)
            {
                data.countries.standing[i] += 0.3f;
            }
            else if (data.countries.standing[i] <= -30)
            {
                data.countries.standing[i] -= 0.3f;
            }
        }

        if(data.countries.WorldTension >= 150)
        {
            data.countries.WorldTension += 3;
        }

        if(data.countries.player == 0)
        {
            if (data.countries.turnsN < 49)
            {
                if (data.countries.Atax < 80) Aparl.value = (float)data.countries.turnsN / 49f;
                else Aparl.value = 0;
                Debug.Log(data.countries.turnsN + " " + data.countries.turnsN / 49);
            }
            else if (data.countries.turnsN < 97)
            {
                if (data.countries.turnsN >= 49 && data.countries.Atax < 80)
                    ending = 3;
                else if (data.countries.Atax < 160)
                    Aparl.value = data.countries.turnsN / 97;
                else
                    Aparl.value = 0;
            }
            else
            {
                if (data.countries.turnsN >= 97 && data.countries.Atax < 80)
                    ending = 3;
            }
        }


        EnemyMove();

        FactionCount();

        TurnEvents();

        data.countries.turnsN++;

        nextEvent(false);
    }

    bool Endgame()
    {
        if (data.countries.player == 0)
        {
            if (data.countries.turnsN == 49)
            {
                if (data.countries.WorldTension < 80)
                    ending = 5;
            }
            else if (data.countries.turnsN == 97)
                if (data.countries.WorldTension < 160)
                    ending = 5;
        }

            
        bool neutral = true;
        if (data.countries.WorldTension >= 300)
            return true;
        for(int i = 0; i < data.countries.size; i++)
        {
            if (data.countries.standing[i] > -30 && data.countries.standing[i] < 30)
                neutral = false;
        }
        return neutral;
    }

    IEnumerator LoadNextScene()
    {
        float fadetime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void nextEvent(bool choice)
    {
        string result;

        if (currentEvent >= 100)
        {
            gameEvent(currentEvent, choice);
        }
        if (EventList.Count != 0)
        {
            currentEvent = (int)EventList.Dequeue();
            if(currentEvent >= 200)
                currentEvent+= data.countries.gevents[data.countries.gevents.FindIndex(x => x.key == currentEvent)].status;
            string[] value = LocalizationManager.instance.GetLocalizedValue(currentEvent.ToString()).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            //string[] value = LocalizationManager.instance.GetLocalizedValue("1").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (Cell.IsActive() == false)
            {
                Cell.gameObject.SetActive(true);
            }

            if (currentEvent == 2 || currentEvent == 3)
            {
                CellImage.sprite = eventImages[int.Parse(value[0])];
                if (lastvisit != -1) result = value[2] + data.countries.names[lastvisit] + value[3];
                else result = "Error!";
                CellText.text = result;
            }
            else
            {
                CellImage.sprite = eventImages[int.Parse(value[0])];
                result = value[2];
                CellText.text = result;
            }
            if (result.Length >= 25)
                result = result.Substring(0, 25) + "...";
            History.Enqueue(result);
            if(History.Count >= 10)
            {
                for (int i = 0; i < (History.Count - 9); i++)
                    History.Dequeue();
            }
        }
        else
        {
            Cell.gameObject.SetActive(false);
            currentEvent = -1;
        }
        Redraw();
    }
    
    void TurnEvents()
    {
        /*
        int eventchoice = UnityEngine.Random.Range(101,122);
        EventList.Enqueue(eventchoice);*/
        
        /*gevent tempe = new gevent();
        bool got = false;
        for (int i = 0; i < data.countries.gevents.Count; i++)
        {
            if (data.countries.gevents[i].status > 0)
            {
                //Debug.Log(data.countries.gevents[i].key + " " + data.countries.gevents[i].status);
                EventList.Enqueue(data.countries.gevents[i].key);
                got = true;
                break;
            }
        }
        if (!got)
        {
            if (data.countries.gevents.Count != 0)
            {
                int eventchoice = UnityEngine.Random.Range(0, data.countries.gevents.Count);
                EventList.Enqueue(data.countries.gevents[eventchoice].key);
                /*if(data.countries.gevents[eventchoice].size > 0)
                {
                    tempe = data.countries.gevents[eventchoice];
                    tempe.status += 1;
                    data.countries.gevents[eventchoice] = tempe;
                }
                Debug.Log(data.countries.gevents[eventchoice].key + " " + data.countries.gevents[eventchoice].status + " " + data.countries.gevents[eventchoice].size);*/
        //    }
        //}

        // 
        /*
        for(int i = 0; i < 4; i++)
        {
            if((data.countries.FactionPN[i, 0] + data.countries.FactionPN[i, 2] + data.countries.FactionPN[i, 4])<= (data.countries.FactionPN[i, 1] + data.countries.FactionPN[i, 3] + data.countries.FactionPN[i, 5]))
            {
                int temp =8 + data.countries.player + i * 2;
                EventList.Enqueue(temp);
            }
            else if ((data.countries.FactionPN[i, 0] + data.countries.FactionPN[i, 2] + data.countries.FactionPN[i, 4]) <= (data.countries.FactionPN[i, 1] + data.countries.FactionPN[i, 3] + data.countries.FactionPN[i, 5]))
            {
                int temp = 24 + data.countries.player + i * 2;
                EventList.Enqueue(temp);
            }
        }*/


    }

    void EndingCount()
    {
        int result = 0, temp;
        for(int i = 0; i < 4; i++)
        {
            if(data.countries.production[i] >= data.countries.needs[i])
            {
                temp = 1;
                result += temp;
            }
            else if(data.countries.production[i] >= data.countries.needs[i]*0.9)
            {
                temp = 2;
                result += temp;
            }
            else if (data.countries.production[i] >= data.countries.needs[i] * 0.75)
            {
                temp = 3;
                result += temp;
            }
            else
            {
                temp = 4;
                result += temp;
            }
            switch (i)
            {
                case 0:
                    PlayerPrefs.SetInt("agriculture", temp);
                    break;
                case 1:
                    PlayerPrefs.SetInt("manufacture", temp);
                    break;
                case 2:
                    PlayerPrefs.SetInt("hightech", temp);
                    break;
                case 3:
                    PlayerPrefs.SetInt("mining", temp);
                    break;
            }
        }
        float rate;
        if (data.countries.player == 0) rate = 137.8f;
        else if (data.countries.player == 1) rate = 544f;
        else rate = 137.8f;
        rate = data.countries.unemployment / rate;
        if (rate < 0.04)
            temp = 1;
        else if (rate < 0.10)
            temp = 2;
        else if (rate < 0.18)
            temp = 3;
        else
            temp = 4;
        PlayerPrefs.SetInt("unemployment", temp);
        result += temp;
        if (result <= 8)
            PlayerPrefs.SetInt("result", 1);
        else
            PlayerPrefs.SetInt("result", 2);
    }


    
    
}

