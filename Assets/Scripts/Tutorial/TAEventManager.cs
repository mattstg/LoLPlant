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
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Sliders, false));
        taQueue.Enqueue(new TASetDNC(false, GV.defaultTutorialHour, 0));
        taQueue.Enqueue(new TAFreezeChar(true));
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardAll, true));
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, false));
        taQueue.Enqueue(new TADelegate(delegate () { GV.ws.plant.water = 0; }));

        switch (progressPoint)
        {
            case 1:  //intro, science: food/photosynthesis
                string intro1 = "<size=30>" +
                                LangDict.Instance.GetText("Intro1a") + "</size>\n\n" +
                                LangDict.Instance.GetText("Intro1b") + "\n   " +
                                LangDict.Instance.GetText("Intro1c") + "\n   " +
                                LangDict.Instance.GetText("Intro1d") + "\n   " +
                                LangDict.Instance.GetText("Intro1e");
                string intro2 = LangDict.Instance.GetText("Intro2a");
                string intro3 = LangDict.Instance.GetText("Intro3a") + "\n\n" +
                                LangDict.Instance.GetText("Intro3b") + "\n\n" +
                                LangDict.Instance.GetText("Intro3c");
                string intro4 = LangDict.Instance.GetText("Intro4a") + "\n\n" +
                                LangDict.Instance.GetText("Intro4b");
                string intro5 = LangDict.Instance.GetText("Intro5a") + "\n\n" +
                                LangDict.Instance.GetText("Intro5b");

                string food1 = LangDict.Instance.GetText("Food1a") + "\n" +
                               LangDict.Instance.GetText("Food1b") + "\n\n" +
                               LangDict.Instance.GetText("Food1c") + "\n\n" +
                               LangDict.Instance.GetText("Food1d");
                string food2 = LangDict.Instance.GetText("Food2a") + "\n\n" +
                               LangDict.Instance.GetText("Food2b") + "\n\n" +
                               LangDict.Instance.GetText("Food2c") + "\n\n" +
                               LangDict.Instance.GetText("Food2d");

                string photo1 = LangDict.Instance.GetText("Photo1a") + "\n\n" +
                                LangDict.Instance.GetText("Photo1b");
                string photo2 = LangDict.Instance.GetText("Photo2a") + "\n\n" +
                                LangDict.Instance.GetText("Photo2b");
                string photo3 = LangDict.Instance.GetText("Photo3a") + "\n\n" +
                                LangDict.Instance.GetText("Photo3b");
                string photo4 = LangDict.Instance.GetText("Photo4a");
                string photo5 = LangDict.Instance.GetText("Photo5a") + "\n\n" +
                                LangDict.Instance.GetText("Photo5b");
                string photo6 = LangDict.Instance.GetText("Photo6a");
                string photo7 = LangDict.Instance.GetText("Photo7a");

                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardNone, true));
                taQueue.Enqueue(new TATimer("Timer", 2));

                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", intro1),
                                                                      new Message("", intro2),
                                                                      new Message("", intro3),
                                                                      new Message("", intro4),
                                                                      new Message("", intro5) }));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", food1),
                                                                      new Message("", food2) }));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", photo1),
                                                                      new Message("", photo2),
                                                                      new Message("", photo3),
                                                                      new Message("", photo4),
                                                                      new Message("", photo5),
                                                                      new Message("", photo6),
                                                                      new Message("", photo7) }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 2));
                goto case 2;

            case 2:  //sun, controls
                string sun1 = LangDict.Instance.GetText("Sun2a") + "\n\n" +
                              LangDict.Instance.GetText("Sun2b");
                string sun2 = LangDict.Instance.GetText("Sun3a") + "\n\n" +
                              LangDict.Instance.GetText("Sun3b");
                string sun3 = LangDict.Instance.GetText("Sun4a") + "\n\n" +
                              LangDict.Instance.GetText("Sun4b");
                string sun4 = LangDict.Instance.GetText("Sun5a") + "\n\n" +
                              LangDict.Instance.GetText("Sun5b");
                string sun5 = LangDict.Instance.GetText("Sun6a") + "\n\n" +
                              LangDict.Instance.GetText("Sun6b");
                string sun6 = LangDict.Instance.GetText("Sun7a") + "\n\n" +
                              LangDict.Instance.GetText("Sun7b");
                string sun7 = LangDict.Instance.GetText("Sun8a") + "\n\n" +
                              LangDict.Instance.GetText("Sun8b") + "\n\n" +
                              LangDict.Instance.GetText("Sun8c");

                string controls1 = LangDict.Instance.GetText("Controls1a") + "\n\n" +
                                   LangDict.Instance.GetText("Controls1b") + "\n    " +
                                   LangDict.Instance.GetText("Controls1c") + "\n    " +
                                   LangDict.Instance.GetText("Controls1d") + "\n    " +
                                   LangDict.Instance.GetText("Controls1e") + "\n\n" +
                                   LangDict.Instance.GetText("Controls1f") + "\n" +
                                   LangDict.Instance.GetText("Controls1g");
                string controls2 = LangDict.Instance.GetText("Controls2a") + "\n\n" +
                                   LangDict.Instance.GetText("Controls2b") + "\n    " +
                                   LangDict.Instance.GetText("Controls2c") + "\n    " +
                                   LangDict.Instance.GetText("Controls2d") + "\n    " +
                                   LangDict.Instance.GetText("Controls2e") + "\n\n" +
                                   LangDict.Instance.GetText("Controls2f") + "\n" +
                                   LangDict.Instance.GetText("Controls2g");

                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardNone, true));
                taQueue.Enqueue(new TATimer("Timer", 0.5f));

                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardSun, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", sun1),
                                                                      new Message("", sun2),
                                                                      new Message("", sun3),
                                                                      new Message("", sun4),
                                                                      new Message("", sun5) }));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", controls1),
                                                                      new Message("", controls2) }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("SunPrompt", sun6, Message.Type.Prompt, Message.Position.TopRight) }));

                taQueue.Enqueue(new TATrigger("Sun"));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", sun7) }));
                taQueue.Enqueue(new TAPromptSuccess("SunPrompt"));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(true));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 3));
                goto case 3;

            case 3:  //water
                string water1 = LangDict.Instance.GetText("Water1a") + "\n\n" +
                                LangDict.Instance.GetText("Water1b");
                string water2 = LangDict.Instance.GetText("Water2a") + "\n\n" +
                                LangDict.Instance.GetText("Water2b");
                string water3 = LangDict.Instance.GetText("Water3a") + "\n\n" +
                                LangDict.Instance.GetText("Water3b");
                string water4 = LangDict.Instance.GetText("Water4a") + "\n\n" +
                                LangDict.Instance.GetText("Water4b");
                string water5 = LangDict.Instance.GetText("Water5a") + "\n\n" +
                                LangDict.Instance.GetText("Water5b");
                string water6 = LangDict.Instance.GetText("Water6a") + "\n\n" +
                                LangDict.Instance.GetText("Water6b");
                string water7 = LangDict.Instance.GetText("Water7a") + "\n\n    " +
                                LangDict.Instance.GetText("Water7b") + "\n\n    " +
                                LangDict.Instance.GetText("Water7c");
                string water8 = LangDict.Instance.GetText("Water8a") + "\n\n" +
                                LangDict.Instance.GetText("Water8b") + "\n\n" +
                                LangDict.Instance.GetText("Water8c");

                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardSun, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", water1) }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardWater, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", water2),
                                                                      new Message("", water3),
                                                                      new Message("", water4) }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Clouds, true));
                taQueue.Enqueue(new TADelegate(LOLAudio.Instance.SetBGLevel, 1f));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("WaterPrompt", water5, Message.Type.Prompt, Message.Position.TopRight) }));

                taQueue.Enqueue(new TATrigger("Water"));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", water6),
                                                                      new Message("", water7),
                                                                      new Message("", water8) }));
                taQueue.Enqueue(new TAPromptSuccess("WaterPrompt"));
                taQueue.Enqueue(new TATimer("Timer", 3f));

                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TAFreezeChar(true));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 4));
                goto case 4;

            case 4:  //photosynthesis, food
                string photoMeter1 = LangDict.Instance.GetText("PhotoMeter1a") + "\n\n" +
                                     LangDict.Instance.GetText("PhotoMeter1b");
                string photoMeter2 = LangDict.Instance.GetText("PhotoMeter2a");
                string photoMeter3 = LangDict.Instance.GetText("PhotoMeter3a") + "\n\n" +
                                     LangDict.Instance.GetText("PhotoMeter3b") + "\n\n" +
                                     LangDict.Instance.GetText("PhotoMeter3c");

                string foodMeter1 = LangDict.Instance.GetText("FoodMeter1a") + "\n\n" +
                                    LangDict.Instance.GetText("FoodMeter1b") + "\n\n" +
                                    LangDict.Instance.GetText("FoodMeter1c");
                string foodMeter2 = LangDict.Instance.GetText("FoodMeter2a") + "\n\n" +
                                    LangDict.Instance.GetText("FoodMeter2b");

                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardWater, true));
                taQueue.Enqueue(new TADelegate(GV.ws.plant.SetFoodTutorial, 500f));
                taQueue.Enqueue(new TATimer("Timer", 0.5f));

                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TADelegate(delegate () { GV.ws.plant.water = 0.75f; }));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardPhotosynthesis, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", photoMeter1),
                                                                      new Message("", photoMeter2),
                                                                      new Message("", photoMeter3) }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardFood, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", foodMeter1) }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("ExperimentPrompt", foodMeter2, Message.Type.Prompt, Message.Position.TopRight) }));
                taQueue.Enqueue(new TATimer("Timer", 30f));

                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TAPromptSuccess("ExperimentPrompt"));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(true));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 5));
                goto case 5;

            case 5:  //aphids, growth, pregame
                string aphids1 = LangDict.Instance.GetText("Aphids1a") + "\n\n" +
                                 LangDict.Instance.GetText("Aphids1b") + "\n\n" +
                                 LangDict.Instance.GetText("Aphids1c");
                string aphids2 = LangDict.Instance.GetText("Aphids2a") + "\n\n" +
                                 LangDict.Instance.GetText("Aphids2b") + " " +
                                 LangDict.Instance.GetText("Aphids2c");

                string growth1 = LangDict.Instance.GetText("Growth1a") + "\n\n" +
                                 LangDict.Instance.GetText("Growth1b") + "\n\n" +
                                 LangDict.Instance.GetText("Growth1c");
                string growth2 = LangDict.Instance.GetText("Growth2a") + "\n\n" +
                                 LangDict.Instance.GetText("Growth2b");
                string growth3 = LangDict.Instance.GetText("Growth3a") + "\n\n" +
                                 LangDict.Instance.GetText("Growth3b");
                string growth4 = LangDict.Instance.GetText("Growth4a") + "\n\n" +
                                 LangDict.Instance.GetText("Growth4b");
                string growth5 = LangDict.Instance.GetText("Growth5a") + "\n\n" +
                                 LangDict.Instance.GetText("Growth5b");
                string growth6 = LangDict.Instance.GetText("Growth6a") + "\n\n" +
                                 LangDict.Instance.GetText("Growth6b") + "\n\n" +
                                 LangDict.Instance.GetText("Growth6c");
                string growth7 = LangDict.Instance.GetText("Growth7a") + "\n\n" +
                                 LangDict.Instance.GetText("Growth7b") + "\n\n" +
                                 LangDict.Instance.GetText("Growth7c");
                string growth8 = LangDict.Instance.GetText("Growth8a") + "\n\n" +
                                 LangDict.Instance.GetText("Growth8b");
                string growth9 = LangDict.Instance.GetText("Growth9a") + "\n\n" +
                                 LangDict.Instance.GetText("Growth9b");
                string growth10 = LangDict.Instance.GetText("Growth10a");

                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardFood, true));
                taQueue.Enqueue(new TADelegate(GV.ws.plant.SetFoodTutorial, 500f));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Platforms, true));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", aphids1) }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TADelegate(delegate () { GameObject.Find("TempWall").SetActive(false); }));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("EscapePrompt", aphids2, Message.Type.Prompt, Message.Position.TopLeft) }));

                taQueue.Enqueue(new TATrigger("FinalPlatform"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardAll, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", growth1),
                                                                      new Message("", growth2),
                                                                      new Message("", growth3),
                                                                      new Message("", growth4),
                                                                      new Message("", growth5),
                                                                      new Message("", growth6),
                                                                      new Message("", growth7),
                                                                      new Message("", growth8),
                                                                      new Message("", growth9),
                                                                      new Message("", growth10) }));
                taQueue.Enqueue(new TAPromptSuccess("EscapePrompt"));
                taQueue.Enqueue(new TAFreezeChar(true));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TADelegate(GV.ws.dnc.JumpToSunset));

                taQueue.Enqueue(new TATrigger("JumpComplete"));
                NightSequence();
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("PreGame1"),
                                                                      new Message("PreGame2"),
                                                                      new Message("PreGame3"),
                                                                      new Message("PreGame4"),
                                                                      new Message("PreGame5") }));
                taQueue.Enqueue(new TATrigger("ClosePopup"));

                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SetScore, 0));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 6));
                taQueue.Enqueue(new TADelegate(delegate () { ProgressTracker.Instance.maxGrowthHeight = 50; }));
                taQueue.Enqueue(new TAChangeFlow(CurrentState.Game));
                break;

            case 6:  //game day 1
                //Initial day popup
                InitiateDay();
                NightSequence();
                taQueue.Enqueue(new TASubmitScore());
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 7));
                goto case 7;
            case 7:  //game day 2+
                InitiateDay();
                NightSequence();
                taQueue.Enqueue(new TASubmitScore());
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 8));
                goto case 8;
            case 8:  //game over
                taQueue.Enqueue(new TACreatePopup(new Message("GameOver", Message.Type.Endgame, Message.Position.Center)));
                break;
        }
        ProcessStack();
    }

    private void InitiateDay()
    {
        taQueue.Enqueue(new TASetDNC(true, DayNightCycle.sunriseHour));
        taQueue.Enqueue(new TADelegate(GV.ws.dnc.BeginDay));
        taQueue.Enqueue(new TAFreezeChar(false, true));
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, true));
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardAll, true));
        taQueue.Enqueue(new TADelegate(delegate () { GV.ws.raincloudManager.rainCloudsPaused = false; }));
        taQueue.Enqueue(new TADelegate(delegate () { GV.ws.plant.water = 0f; }));
        taQueue.Enqueue(new TADelegate(delegate () { GV.ws.plant.food = 0f; }));
        //reposition player
    }

    private void NightSequence()
    {
        taQueue.Enqueue(new TATrigger("BeginNight"));
        taQueue.Enqueue(new TASetDNC(false, DayNightCycle.sunsetHour));
        taQueue.Enqueue(new TAFreezeChar(true, true));
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, false));
        taQueue.Enqueue(new TADelegate(delegate () { GV.ws.raincloudManager.rainCloudsPaused = true; }));

        taQueue.Enqueue(new TADelegate(GV.ws.dnc.BeginZoomIn));
        taQueue.Enqueue(new TATrigger("ZoomComplete"));

        taQueue.Enqueue(new TAGrowthSequence(true));
        taQueue.Enqueue(new TATrigger("GrowthComplete"));

        taQueue.Enqueue(new TADelegate(GV.ws.popupManager.LoadScorePopup));
        taQueue.Enqueue(new TATrigger("ClosePopup"));

        taQueue.Enqueue(new TADelegate(GV.ws.dnc.BeginZoomOut));
        taQueue.Enqueue(new TATrigger("ZoomComplete"));

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

    }

}
