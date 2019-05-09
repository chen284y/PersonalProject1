using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager instance;
    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missing = "Localized text not found!";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void LoadLocalizedText(string fileName)
    {
        Debug.Log("1");
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            Debug.Log("2");
            localizedText.Clear();
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            Debug.Log("Data Loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
    }

    public void ChangeLocalizedText(string fileName)
    {
        Debug.Log("1");
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            Debug.Log("1");
            localizedText.Clear();
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
            Debug.Log("Data Loaded, dictionary contains: " + localizedText.Count + " entries");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("1");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    public string GetLocalizedValue(string key)
    {
        if (LocalizationManager.instance.GetIsReady())
        {
            string result = missing;
            if (localizedText.ContainsKey(key))
            {
                result = localizedText[key];
            }

            return result;
        }
        else
            return "Not Ready Yet!";
    }

    public bool GetIsReady()
    {
        return isReady;
    }

}
