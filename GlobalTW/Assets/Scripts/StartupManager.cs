using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour {

    public Texture2D cursor;
    public CursorMode CurMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
	// Use this for initialization
	private IEnumerator Start () {
        LocalizationManager.instance.LoadLocalizedText("localizedText_cn.json");

		while(!LocalizationManager.instance.GetIsReady())
        {
            yield return null;
        }
        Debug.Log("text Load Successfully!");
        float fadeTime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("Menu");

        Cursor.SetCursor(cursor, hotSpot, CurMode);
	}
	
}
