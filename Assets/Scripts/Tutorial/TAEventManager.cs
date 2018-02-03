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
        taQueue.Enqueue(new TAFreezeChar(true));
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardAll, true));
        taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, false));
        taQueue.Enqueue(new TADelegate(delegate () { GV.ws.plant.water = 0; }));

        switch (progressPoint)
        {
            case 1:  //intro, science: food/photosynthesis
                string intro1 = "<size=30>" +
                                LangDict.Instance.GetText("Intro1a") + "</size>\r\n\r\n" +
                                LangDict.Instance.GetText("Intro1b") + "\r\n   " +
                                LangDict.Instance.GetText("Intro1c") + "\r\n   " +
                                LangDict.Instance.GetText("Intro1d") + "\r\n   " +
                                LangDict.Instance.GetText("Intro1e");
                string intro2 = LangDict.Instance.GetText("Intro2a");
                string intro3 = LangDict.Instance.GetText("Intro3a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Intro3b");
                string intro4 = LangDict.Instance.GetText("Intro4a");
                string intro5 = LangDict.Instance.GetText("Intro5a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Intro5b");
                string intro6 = LangDict.Instance.GetText("Intro6a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Intro6b");
                string intro7 = LangDict.Instance.GetText("Intro7a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Intro7b");

                taQueue.Enqueue(new TASetDNC(false, GV.defaultTutorialHour, 0));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardNone, true));
                taQueue.Enqueue(new TATimer("Timer", 2));

                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", intro1, "TTSIntro1"),
                                                                      new Message("", intro2, "TTSIntro2"),
                                                                      new Message("", intro3, "TTSIntro3"),
                                                                      new Message("", intro4, "TTSIntro4"),
                                                                      new Message("", intro5, "TTSIntro5"),
                                                                      new Message("", intro6, "TTSIntro6"),
                                                                      new Message("", intro7, "TTSIntro7") }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 2));
                goto case 2;

            case 2:  //sun, controls
                string sun1 = LangDict.Instance.GetText("Sun1a") + "\r\n\r\n" +
                              LangDict.Instance.GetText("Sun1b");
                string sun2 = LangDict.Instance.GetText("Sun2a") + "\r\n\r\n" +
                              LangDict.Instance.GetText("Sun2b");
                string sun3 = LangDict.Instance.GetText("Sun3a") + "\r\n\r\n" +
                              LangDict.Instance.GetText("Sun3b");
                string sun4 = LangDict.Instance.GetText("Sun4a") + "\r\n\r\n" +
                              LangDict.Instance.GetText("Sun4b");

                string controls1 = LangDict.Instance.GetText("Controls1a") + "\r\n\r\n" +
                                   LangDict.Instance.GetText("Controls1b") + "\r\n    " +
                                   LangDict.Instance.GetText("Controls1c") + "\r\n    " +
                                   LangDict.Instance.GetText("Controls1d") + "\r\n    " +
                                   LangDict.Instance.GetText("Controls1e") + "\r\n\r\n" +
                                   LangDict.Instance.GetText("Controls1f") + "\r\n" +
                                   LangDict.Instance.GetText("Controls1g");
                string controls2 = LangDict.Instance.GetText("Controls2a") + "\r\n\r\n" +
                                   LangDict.Instance.GetText("Controls2b") + "\r\n    " +
                                   LangDict.Instance.GetText("Controls2c") + "\r\n    " +
                                   LangDict.Instance.GetText("Controls2d") + "\r\n    " +
                                   LangDict.Instance.GetText("Controls2e") + "\r\n\r\n" +
                                   LangDict.Instance.GetText("Controls2f") + "\r\n" +
                                   LangDict.Instance.GetText("Controls2g");

                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardNone, true));
                taQueue.Enqueue(new TATimer("Timer", 0.5f));

                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardSun, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", sun1, "TTSSun1"),
                                                                      new Message("", sun2, "TTSSun2") }));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", controls1, "TTSControls1"),
                                                                      new Message("", controls2, "TTSControls2") }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("SunPrompt", sun3, "TTSSun3", Message.Type.Prompt, Message.Position.TopRight) }));

                taQueue.Enqueue(new TATrigger("Sun"));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", sun4, "TTSSun4") }));
                taQueue.Enqueue(new TAFreezeChar(true));
                taQueue.Enqueue(new TAPromptSuccess("SunPrompt"));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 3));
                goto case 3;

            case 3:  //water
                string water1 = LangDict.Instance.GetText("Water1a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Water1b");
                string water2 = LangDict.Instance.GetText("Water2a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Water2b");
                string water3 = LangDict.Instance.GetText("Water3a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Water3b");
                string water4 = LangDict.Instance.GetText("Water4a");
                string water5 = LangDict.Instance.GetText("Water5a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Water5b");
                string water6 = LangDict.Instance.GetText("Water6a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Water6b");
                string water7 = LangDict.Instance.GetText("Water7a") + "\r\n\r\n    " +
                                LangDict.Instance.GetText("Water7b") + "\r\n\r\n    " +
                                LangDict.Instance.GetText("Water7c");

                taQueue.Enqueue(new TASetDNC(false, GV.defaultTutorialHour, 0));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardSun, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", water1, "TTSWater1") }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardWater, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", water2, "TTSWater2"),
                                                                      new Message("", water3, "TTSWater3"),
                                                                      new Message("", water4, "TTSWater4") }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Clouds, true));
                taQueue.Enqueue(new TADelegate(LOLAudio.Instance.PlayBackgroundAudio, LOLAudio.heavyRain));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("WaterPrompt", water5, "TTSWater5", Message.Type.Prompt, Message.Position.TopRight) }));

                taQueue.Enqueue(new TATrigger("Water"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Clouds, false));
                taQueue.Enqueue(new TADelegate(LOLAudio.Instance.StopAudio, LOLAudio.heavyRain));
                taQueue.Enqueue(new TAFreezeChar(true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", water6, "TTSWater6"),
                                                                      new Message("", water7, "TTSWater7") }));
                taQueue.Enqueue(new TAPromptSuccess("WaterPrompt"));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 4));
                goto case 4;

            case 4:  //photosynthesis, food
                string photo1 = LangDict.Instance.GetText("Photo1a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Photo1b");

                string photo2 = LangDict.Instance.GetText("Photo2a") + "\r\n\r\n" +
                                LangDict.Instance.GetText("Photo2b");
                string photo3 = LangDict.Instance.GetText("Photo3a");
                string photo4 = LangDict.Instance.GetText("Photo4a");

                string food1 = LangDict.Instance.GetText("Food1a") + "\r\n\r\n" +
                               LangDict.Instance.GetText("Food1b") + "\r\n\r\n" +
                               LangDict.Instance.GetText("Food1c");
                string food2 = LangDict.Instance.GetText("Food2a") + "\r\n\r\n" +
                               LangDict.Instance.GetText("Food2b");

                taQueue.Enqueue(new TASetDNC(false, GV.defaultTutorialHour, 0));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardWater, true));
                taQueue.Enqueue(new TADelegate(GV.ws.plant.SetFoodTutorial, 500f));
                taQueue.Enqueue(new TATimer("Timer", 0.5f));

                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TADelegate(delegate () { GV.ws.plant.water = 0.75f; }));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardPhotosynthesis, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", photo1, "TTSPhoto1"),
                                                                      new Message("", photo2, "TTSPhoto2"),
                                                                      new Message("", photo3, "TTSPhoto3"),
                                                                      new Message("", photo4, "TTSPhoto4") }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardFood, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", food1, "TTSFood1") }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Clouds, true));
                taQueue.Enqueue(new TADelegate(LOLAudio.Instance.PlayBackgroundAudio, LOLAudio.heavyRain));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("ExperimentPrompt", food2, "TTSFood2", Message.Type.Prompt, Message.Position.TopLeft) }));
                taQueue.Enqueue(new TATimer("Timer", 30f));

                taQueue.Enqueue(new TATrigger("Timer"));
                taQueue.Enqueue(new TAPromptSuccess("ExperimentPrompt"));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(true));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 5));
                goto case 5;

            case 5:  //aphids, growth, pregame
                string aphids1 = LangDict.Instance.GetText("Aphids1a") + "\r\n\r\n" +
                                 LangDict.Instance.GetText("Aphids1b") + "\r\n\r\n" +
                                 LangDict.Instance.GetText("Aphids1c");
                string aphids2 = LangDict.Instance.GetText("Aphids2a") + "\r\n\r\n" +
                                 LangDict.Instance.GetText("Aphids2b") + " " +
                                 LangDict.Instance.GetText("Aphids2c");

                string growth1 = LangDict.Instance.GetText("Growth1a") + "\r\n\r\n" +
                                 LangDict.Instance.GetText("Growth1b") + "\r\n\r\n" +
                                 LangDict.Instance.GetText("Growth1c");
                string growth2 = LangDict.Instance.GetText("Growth2a") + "\r\n\r\n" +
                                 LangDict.Instance.GetText("Growth2b");
                string growth3 = LangDict.Instance.GetText("Growth3a");
                string growth4 = LangDict.Instance.GetText("Growth4a") + "\r\n\r\n" +
                                 LangDict.Instance.GetText("Growth4b");
                string growth5 = LangDict.Instance.GetText("Growth5a");
                string growth6 = LangDict.Instance.GetText("Growth6a");

                taQueue.Enqueue(new TASetDNC(false, GV.defaultTutorialHour, 0));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardFood, true));
                taQueue.Enqueue(new TADelegate(GV.ws.plant.SetFoodTutorial, 500f));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Platforms, true));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.Aphids, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", aphids1, "TTSAphids1") }));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TAFreezeChar(false));
                taQueue.Enqueue(new TADelegate(delegate () { GameObject.Find("TempWall").SetActive(false); }));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("EscapePrompt", aphids2, "TTSAphids2", Message.Type.Prompt, Message.Position.TopLeft) }));

                taQueue.Enqueue(new TATrigger("FinalPlatform"));
                taQueue.Enqueue(new TAActivate(TAActivate.ActivateType.DashboardAll, true));
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("", growth1, "TTSGrowth1"),
                                                                      new Message("", growth2, "TTSGrowth2"),
                                                                      new Message("", growth3, "TTSGrowth3"),
                                                                      new Message("", growth4, "TTSGrowth4"),
                                                                      new Message("", growth5, "TTSGrowth5"),
                                                                      new Message("", growth6, "TTSGrowth6") }));
                taQueue.Enqueue(new TAPromptSuccess("EscapePrompt"));
                taQueue.Enqueue(new TAFreezeChar(true));

                taQueue.Enqueue(new TATrigger("ClosePopup"));
                taQueue.Enqueue(new TADelegate(GV.ws.dnc.JumpToSunset));

                taQueue.Enqueue(new TATrigger("JumpComplete"));
                NightSequence(true);
                taQueue.Enqueue(new TACreatePopup(new List<Message> { new Message("PreGame1", "TTSPreGame1"),
                                                                      new Message("PreGame2", "TTSPreGame2"),
                                                                      new Message("PreGame3", "TTSPreGame3"),
                                                                      new Message("PreGame4", "TTSPreGame4") }));
                taQueue.Enqueue(new TATrigger("ClosePopup"));

                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SetScore, 0));
                taQueue.Enqueue(new TADelegate(ProgressTracker.Instance.SubmitProgress, 6));
                taQueue.Enqueue(new TADelegate(delegate () { ProgressTracker.Instance.maxGrowthHeight = 50; }));
                taQueue.Enqueue(new TAChangeFlow(CurrentState.Game));
                break;

            case 6:  //game day 1
                //Initial day popup
                taQueue.Enqueue(new TADelegate(delegate () { DayNightCycle.time = GV.ws.dnc.GetTime(1, DayNightCycle.sunriseHour); }));
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
                taQueue.Enqueue(new TACreatePopup(new Message("GameOver", "TTSGameOver", Message.Type.Endgame, Message.Position.Center)));
                break;
        }
        ProcessStack();
    }

    private void InitiateDay()
    {
        taQueue.Enqueue(new TADelegate(LOLAudio.Instance.StopAudio, LOLAudio.heavyRain));
        taQueue.Enqueue(new TADelegate(LOLAudio.Instance.PlayBackgroundAudio, LOLAudio.heavyRain));
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

    private void NightSequence(bool isTutorial = false)
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

        taQueue.Enqueue(new TADelegate(GV.ws.popupManager.LoadScorePopup, isTutorial));
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
