using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrentState { MainMenu, Tutorial, Game, PostGame}

public class MainScript : MonoBehaviour
{
    public static int progressPoint = 0;
    public static int score = 0;
    bool lolsdkFinishedLoading = false;
    bool flowInitialized = false;

    CurrentState currentState;
    Flow curFlow;
    
    public void Initialize(CurrentState cs)
    {
        //THIS IS THE FIRST POINT EVER ENTERED BY THIS PROGRAM. (Except for MainScriptInitializer.cs, who creates this script and runs this function for the game to start)
        GV.ms = this; //set link so rest can access this script easily
        SDKLoader.StartLoader(); //Will link SDK calls from LoL to us (to recieve)
        currentState = cs;
    }

	void Update ()
    {
        if (!lolsdkFinishedLoading)
            lolsdkFinishedLoading = SDKLoader.CheckIfEverythingLoaded();  //If have not recieved lols jsons yet

        if (lolsdkFinishedLoading && !flowInitialized)  //Once recieved jsons, initialize the flow
        {             
            curFlow = InitializeFlowScript(currentState, progressPoint, true);  //initial flow initialize
            flowInitialized = true;
        }

        if (flowInitialized)
        {
            if (curFlow == null)
                return; //This means Initialize hasnt been called yet, can happen in weird Awake/Update way (should not though, but be safe)
            float dt = Time.deltaTime;
            curFlow.Update(dt);
        }
	}

    private Flow InitializeFlowScript(CurrentState flowType, int progressPoint, bool sceneAlreadyLoaded)
    {
        Flow newFlow;
        switch (flowType)
        {
            case CurrentState.MainMenu:
                newFlow = new MainFlow();
                break;
            case CurrentState.Game:
                newFlow = new GameFlow();
                break;
            case CurrentState.Tutorial:
                newFlow = new TutorialFlow();
                break;
            default:
                return null;
        }

        if(!sceneAlreadyLoaded)
            SceneManager.sceneLoaded += OnSceneLoaded; //Delay flow initialization until 
        else
            newFlow.Initialize(progressPoint);

        return newFlow;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Old scenes might still be listening, so doublecheck
        bool verified = false;
        switch(scene.name)
        {
            case "menuScene":
                verified = (currentState == CurrentState.MainMenu);
                break;
            case "tutorialScene":
                verified = (currentState == CurrentState.Tutorial);
                break;
            case "gameScene":
                verified = (currentState == CurrentState.Game);
                break;
            default:
                Debug.Log("Switch case not found: " + scene.name);
                break;
        }

        if(verified)
            curFlow.Initialize(progressPoint);
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
        currentState = cs;
        //Initialize the flow script for the scene
        curFlow = InitializeFlowScript(cs, progressPoint, false);
    }
}
