using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CurrentState { Lab, Game }

public class MainScript : MonoBehaviour
{
    CurrentState currentState;
    Flow gameFlow;
    Flow labFlow;
    Flow curFlow;
	// Update is called once per frame
    void Start()
    {
        currentState = CurrentState.Game;
        curFlow = gameFlow = InitializeFlowScript(currentState, 0);
    }


	void Update ()
    {
        float dt = Time.deltaTime;
        curFlow.Update(dt);
		//switch(currentState)
        //{
        //    case CurrentState.Game:
        //        break;
        //    case CurrentState.Lab:
        //        break;
        //}
	}

    private Flow InitializeFlowScript(CurrentState flowType, int progressPoint)
    {
        Flow newFlow;
        switch (flowType)
        {
            case CurrentState.Game:
                newFlow = new GameFlow();
                break;
            case CurrentState.Lab:
                newFlow = new LabFlow();
                break;
            default:
                return null;
        }
        newFlow.Initialize(progressPoint);
        return newFlow;
    }
}
