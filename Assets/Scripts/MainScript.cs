using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrentState { MainMenu, Tutorial, Game, PostGame}

public class MainScript : MonoBehaviour
{
    public static int progressPoint = 0;

    CurrentState currentState;
    Flow curFlow;
    
    public void Initialize(CurrentState cs)
    {
        //THIS IS THE FIRST POINT EVER ENTERED BY THIS PROGRAM. (Except for MainScriptInitializer.cs, who creates this script and runs this function for the game to start)
        GV.ms = this; //set link so rest can access this script easily
        currentState = cs; //Initial scene to load
        curFlow = InitializeFlowScript(currentState, progressPoint);  //initial flow initialize
        SDKLoader.StartLoader(); //Will link SDK calls from LoL to us (to recieve)
    }


	void Update ()
    {
        if (curFlow == null)
            return; //This means Initialize hasnt been called yet, can happen in weird Awake/Update way (should though, but be safe)
        float dt = Time.deltaTime;
        curFlow.Update(dt);
	}

    private Flow InitializeFlowScript(CurrentState flowType, int progressPoint)
    {
        Flow newFlow;
        switch (flowType)
        {
            case CurrentState.Game:
                newFlow = new GameFlow();
                break;
            case CurrentState.Tutorial:
                newFlow = new TutorialFlow();
                break;
            default:
                return null;
        }
        newFlow.Initialize(progressPoint);
        return newFlow;
    }

    public void GoToNextFlow(CurrentState cs)
    {
        //Assume Flow called Clean already
        
        //Load the next scene        
        switch (cs)
        {
            case CurrentState.MainMenu:
                SceneManager.LoadScene("Scenes/menuScene");
                break;
            case CurrentState.Game:
                SceneManager.LoadScene("Scenes/gameScene");
                break;
            case CurrentState.Tutorial:
                SceneManager.LoadScene("Scenes/tutorialScene");
                break;
            case CurrentState.PostGame:
                SceneManager.LoadScene("Scenes/postGameScene");
                break;
            default:
                Debug.LogError("Unhandled Switch: " + cs);
                return;
        }

        //Initialize the flow script for the scene
        curFlow = InitializeFlowScript(currentState, progressPoint);
    }
}
