using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    bool freshGame;
    int progressPt;
    public Button mainButton;
    public Button lessonAgainBtn;
    public Text mainTitleText;

    public void SDKLoaded(int _progressPt)
    {
        progressPt = _progressPt;
        freshGame = (_progressPt == 1);
        mainButton.GetComponentInChildren<Text>().text = (freshGame) ? LangDict.Instance.GetText("StartButton") : LangDict.Instance.GetText("ContinueButton");
        mainButton.gameObject.SetActive(true);
        lessonAgainBtn.gameObject.SetActive(!freshGame);
        lessonAgainBtn.GetComponentInChildren<Text>().text = LangDict.Instance.GetText("LessonButton");
        mainTitleText.text = LangDict.Instance.GetText("ColoredMenuTitle");
        mainTitleText.gameObject.SetActive(true);
    }

	public void StartPressed()
    {
        if (progressPt <= GV.LastTutorialProgressPoint)
        {
            ProgressTracker.Instance.SubmitProgress(1);
            GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Tutorial);
        }
        else
            GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Game);
    }

    public void LessonAgainPressed()
    {
        GameObject.FindObjectOfType<MainScript>().GoToNextFlow(CurrentState.Tutorial);
    }

}
