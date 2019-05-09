using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GTW;

public class MainMenu : MonoBehaviour {

    GlobalTW data;

    int wait;
    

    public void PlayGame()
    {
        PlayerPrefs.SetInt("player", -1);
        StartCoroutine(NextScene());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void APlayGame()
    {

        PlayerPrefs.SetInt("player", 0);
        StartCoroutine(NextScene());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CPlayGame()
    {
        /*data = GlobalTW.instance;
        data.PlayerN = 1;
        data.ReadCountries();*/
        PlayerPrefs.SetInt("player", 1);
        StartCoroutine(NextScene());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator NextScene()
    {
        Debug.Log("4");
        float fadetime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        StartCoroutine(quitgame());
    }

    IEnumerator quitgame()
    {
        float fadeTime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(0.8f);
        Application.Quit();
    }

}
