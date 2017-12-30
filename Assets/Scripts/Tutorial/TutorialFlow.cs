using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlow : Flow
{
    public override void Initialize(int progressNumber)
    {
        LOLAudio.Instance.ClearDisabledSounds();
        LOLAudio.Instance.StopAudio(LOLAudio.heavyRain);
        LOLAudio.Instance.PlayBackgroundAudio(LOLAudio.heavyRain);
        LOLAudio.Instance.SetBGLevel(0);

        if (progressNumber > GV.LastTutorialProgressPoint)
            progressNumber = 1; //If playing tut for second time or force loading scene

        GameObject.FindObjectOfType<WS>().LinkToGV(this); //Force the link of WS into GV so can set the links below
        GV.ws.dnc.Initialize();
        GV.ws.pc.Initialize();
        GV.ws.plant.Initialize(60);
        GV.ws.cameraManager.Initialize();
        GV.ws.platformManager.Initialize();
        GV.ws.dm.Initialize();
        GV.ws.popupManager.Initialize();
        GV.ws.cloudManager.Initialize();
        GV.ws.shadowManager.Initialize();
        GV.ws.aphidManager.Initialize();        //Create all aphids, must be called after platform manager
        GV.ws.raincloudManager.Initialize(false);
        TAEventManager.Instance.Initialize(progressNumber);
        initialized = true;
    }

    public override void Update(float dt)
    {
        if (initialized)
        {
            GV.ws.dnc.Refresh(dt);
            GV.ws.plant.Refresh(dt);
            GV.ws.pc.Refresh(dt);
            GV.ws.cameraManager.Refresh(dt);     //Cam is locked for tut, don't update
            GV.ws.platformManager.Refresh(dt);
            GV.ws.dm.Refresh(dt);
            GV.ws.popupManager.Refresh(dt);
            foreach (Parallax p in GV.ws.parallaxes)  
                p.Refresh(dt);
            GV.ws.cloudManager.Refresh(dt);
            GV.ws.shadowManager.Refresh();
            GV.ws.aphidManager.Refresh(dt);
            if(GV.ws.raincloudManager.gameObject.activeInHierarchy)
                GV.ws.raincloudManager.Refresh(dt);
        }
    }
}
