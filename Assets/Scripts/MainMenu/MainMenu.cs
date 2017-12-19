using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    bool freshGame;
    int progressPt;
    public Button mainButton;

    public void SDKLoaded(int _progressPt)
    {
        progressPt = _progressPt;
        freshGame = (_progressPt == 1);
        mainButton.GetComponentInChildren<Text>().text = (freshGame) ? LangDict.Instance.GetText("Start") : LangDict.Instance.GetText("Continue");
        mainButton.gameObject.SetActive(true);
    }

	public void StartPressed()
    {
        if (progressPt <= GV.LastTutorialProgressPoint)
            GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Tutorial);
        else
            GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Game);
    }
    
}
