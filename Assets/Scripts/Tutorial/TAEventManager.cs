using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAEventManager
{
    #region Singleton
    private static TAEventManager instance;

    private TAEventManager() { taStack = new Stack<TAEvent>(); }

    public static TAEventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TAEventManager();
            }
            return instance;
        }
    }
    #endregion

    Stack<TAEvent> taStack;
    string currentLock = "";

    public void Initialize(int progressPoint)
    {
        taStack.Clear();
        //Setup the entire stack here
        switch(progressPoint)
        {
            case 0:
                goto case 1;
            case 1:
                goto case 2;
            case 2:
                goto case 3;
            case 3:
                goto case 4;
            case 4:
                goto case 5;
            case 5:
                break;
        }
    }

    private void ProcessStack()
    {
        if (taStack.Peek().eventType == TAEvent.TAEventType.Trigger)
        {
            currentLock = taStack.Pop().eventName;
            return; //Triggers end process, need to wait till next lock given to proccess stack again
        }
        else
        {
            taStack.Pop().PerformEvent();
            ProcessStack(); //If is an action, can do multiple actions in a row, continues till finds a trigger
        }
    }

    public void RecieveActionTrigger(string triggerName)
    {
        if (triggerName == currentLock)
            ProcessStack();
    }
	
}
