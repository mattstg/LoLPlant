using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAEventManager
{
    #region Singleton
    private static TAEventManager instance;

    private TAEventManager() { taQueue = new Queue<TAEvent>(); }

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

    Queue<TAEvent> taQueue;
    string currentLock = "";

    public void Initialize(int progressPoint)
    {
        taQueue.Clear();
        //Setup the entire stack here
        switch (progressPoint)
        {
            case 0:
                taQueue.Enqueue(new TAFreezeChar(true));
                taQueue.Enqueue(new TATimer("Timer", 2));
                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TACreatePopup("SunReq"));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TACreatePopup("GatherSun"));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TACreatePopup("ControlsKey"));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TACreatePopup("ControlsTouch"));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TATrigger("PlatformTouch"));
                taQueue.Enqueue(new TACreatePopup("SunMeter"));
                taQueue.Enqueue(new TAActivate("DashboardSun",true));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                goto case 1;
            case 1:
                taQueue.Enqueue(new TACreatePopup("H20Req"));
                taQueue.Enqueue(new TAActivate("DashboardWater", true));
                taQueue.Enqueue(new TATrigger("Water"));
                taQueue.Enqueue(new TACreatePopup("Evaporation"));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TACreatePopup("ReqBothForSugar"));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                goto case 2;
            case 2:
                taQueue.Enqueue(new TAActivate("DashboardFood", true));
                taQueue.Enqueue(new TACreatePopup("Sugar"));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(true));
                taQueue.Enqueue(new TAActivate("Aphids", true));
                taQueue.Enqueue(new TATrigger("AphidDamage"));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TACreatePopup("SugarGrowthRate"));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                goto case 3;
            case 3:
                //Special lab popup
                //Close button for special lab pressed
                goto case 4;
            case 4:
                taQueue.Enqueue(new TACreatePopup("Escape"));
                taQueue.Enqueue(new TAActivate("Platforms", true));
                taQueue.Enqueue(new TATrigger("FinalPlatform"));
                taQueue.Enqueue(new TAFreezeChar(true));
                taQueue.Enqueue(new TATimer("Timer", 2));
                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TACreatePopup("SugarsToGrowth"));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                // HERE< SPECIAL END LEVEL POPUP
                //ENDS LEVEL WHEN CLOSES
                break;
        }
    }

    public void RecieveLock(string newLock)
    {
        currentLock = newLock;
    }

    private void ProcessStack()
    {
        TAEvent nextEvent = taQueue.Dequeue();
        nextEvent.PerformEvent();
        if (nextEvent.eventType == TAEvent.TAEventType.Action)  //If it was an action, then the lock is not set, so do next action
            ProcessStack();
    }

    public void RecieveActionTrigger(string triggerName)
    {
        if (triggerName == currentLock)
            ProcessStack();
    }
	
}
