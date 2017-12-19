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
        Debug.Log("setup for p point: " + progressPoint);
        //Setup the entire stack here
        switch (progressPoint)
        {
            case 1:
                //
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Sliders, false));
                taQueue.Enqueue(new TASetDNC(false, 12, 0));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardNone, true));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, false));
                //taQueue.Enqueue(new TAActivate("Platforms", false));
                taQueue.Enqueue(new TAFreezeChar(true,false));
                taQueue.Enqueue(new TATimer("Timer", 2));
                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TACreatePopup(new Message("SunReq")));
                taQueue.Enqueue(new TACreatePopup(new Message("GatherSun", Message.Type.Prompt)));
                taQueue.Enqueue(new TACreatePopup(new Message("ControlsKey", Message.Type.Info)));
                taQueue.Enqueue(new TACreatePopup(new Message("ControlsTouch", Message.Type.Info)));
                taQueue.Enqueue(new TATrigger("Sun"));
                taQueue.Enqueue(new TAPromptSuccess("GatherSun"));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardSun, true));
                taQueue.Enqueue(new TATrigger("Sun"));
                taQueue.Enqueue(new TACreatePopup(new Message("SunMeter")));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitAndIncrementProgress));
                goto case 2;
            case 2:
                taQueue.Enqueue(new TACreatePopup(new Message("H20Req", Message.Type.Prompt)));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardWater, true));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Clouds, true));
                taQueue.Enqueue(new TADelegate(LOLAudio.Instance.PlayBackgroundAudio, "lightRain"));
                taQueue.Enqueue(new TATrigger("Water"));
                taQueue.Enqueue(new TAPromptSuccess("H20Req"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Clouds, false));
                taQueue.Enqueue(new TADelegate(LOLAudio.Instance.SetBGLevel, 0));
                taQueue.Enqueue(new TACreatePopup(new Message("Evaporation")));
                taQueue.Enqueue(new TACreatePopup(new Message("ReqBothForSugar")));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitAndIncrementProgress));
                goto case 3;
            case 3:
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardFood, true));
                taQueue.Enqueue(new TACreatePopup(new Message("Sugar")));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(true));
                
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitAndIncrementProgress));
                goto case 4;
            case 4:
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Sliders, true));
                //Special lab popup
                //Close button for special lab pressed
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Sliders, false));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitAndIncrementProgress));
                goto case 5;
            case 5:
                taQueue.Enqueue(new TACreatePopup(new Message("Escape", Message.Type.Prompt)));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Platforms, true));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, true));
                taQueue.Enqueue(new TATrigger("FinalPlatform"));
                taQueue.Enqueue(new TAPromptSuccess("Escape"));
                // TAPromptSuccess
                taQueue.Enqueue(new TAFreezeChar(true));
                taQueue.Enqueue(new TATimer("Timer", 2));
                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TACreatePopup(new Message("SugarsToGrowth")));
                taQueue.Enqueue(new TATrigger("ClosePopup"));
                //Zoom to sunset
                AddEndOfDayScene();
                // HERE< SPECIAL END LEVEL POPUP
                //ENDS LEVEL WHEN CLOSES
                break;
            case 6:
                //Initial day popup
                AddEndOfDayScene();
                goto case 7;
            case 7:
                AddEndOfDayScene();
                goto case 8;
            case 8:
                //Game end reached
                taQueue.Enqueue(new TAFreezeChar(false, true));
                taQueue.Enqueue(new TASetDNC(false));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, false));
                taQueue.Enqueue(new TACreatePopup(new Message("GameOver", Message.Type.Endgame, Message.Position.Right)));
                break;
        }
        ProcessStack();
    }

    private void AddEndOfDayScene()
    { //
        taQueue.Enqueue(new TATrigger("NightTimeStart"));
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids,false));
        taQueue.Enqueue(new TAFreezeChar(true, true));
        taQueue.Enqueue(new TACamZoom(GV.cameraGrowthZoom));
        taQueue.Enqueue(new TASetDNC(false,DayNightCycle.sunsetHour));
        taQueue.Enqueue(new TAPlayerTinter(1));
        taQueue.Enqueue(new TAGrowthSequence(true));
        taQueue.Enqueue(new TATrigger("GrowthComplete"));
        //taQueue.Enqueue(new TACreatePopup(new Message("TheGrowthPanel", Message.Type.Info, Message.Position.Right)));
        //trigger 
        taQueue.Enqueue(new TASetDNC(true, DayNightCycle.sunriseHour));
        taQueue.Enqueue(new TAFreezeChar(false, true));
        taQueue.Enqueue(new TACamZoom(GV.cameraDefaultZoom));
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, true));
        taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitAndIncrementProgress));
        //Reposition character
    }



    public void RecieveLock(string newLock)
    {
        currentLock = newLock;
    }

    private void ProcessStack()
    {
        if (taQueue.Count > 0)
        {
            TAEvent nextEvent = taQueue.Dequeue();
            nextEvent.PerformEvent();
            if (nextEvent.eventType == TAEvent.TAEventType.Action)  //If it was an action, then the lock is not set, so do next action
                ProcessStack();
        }
    }

    public void ReceiveActionTrigger(string triggerName)
    {
        if (triggerName == currentLock)
        {
            currentLock = "";
            ProcessStack();
        }
        Debug.Log("EventManager.ReceiveTrigger(): " + triggerName);
    }
	
}
