using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlow : Flow
{
    public override void Initialize(int progressNumber)
    {
        if(progressNumber > GV.LastTutorialProgressPoint)
        {
            Debug.Log("Progress point retrieved from JSON too high for tutorial scene, reseting progress to 0");
            progressNumber = MainScript.progressPoint = 0;
        }

        GameObject.FindObjectOfType<WS>().LinkToGV(this); //Force the link of WS into GV so can set the links below
        GV.ws.dnc.Initialize();
        GV.ws.plant.Initialize();
        GV.ws.pc.Initialize();
        // GV.ws.cameraManager.Initialize();    //Cam is locked for tut, don't update
        GV.ws.platformManager.Initialize();
        GV.ws.dm.Initialize();
        GV.ws.popupManager.Initialize();
        GV.ws.cloudManager.Initialize();
        GV.ws.shadowManager.Initialize();
        GV.ws.aphidManager.Initialize();        //Create all aphids, must be called after platform manager
        TAEventManager.Instance.Initialize(progressNumber);
        initialized = true;
    }

    public override void Update(float dt)
    {
        if (!GV.Paused && initialized)
        {
            GV.ws.dnc.Refresh(dt);
            GV.ws.plant.Refresh(dt);
            GV.ws.pc.Refresh(dt);
            // GV.ws.cameraManager.Refresh(dt);     //Cam is locked for tut, don't update
            GV.ws.platformManager.Refresh(dt);
            GV.ws.dm.Refresh(dt);
            GV.ws.popupManager.Refresh(dt);
            //foreach (Parallax p in GV.ws.parallaxes)  //No parallax in tutorial
            //    p.Refresh(dt);
            GV.ws.cloudManager.Refresh(dt);
            GV.ws.shadowManager.Refresh();
            GV.ws.aphidManager.Refresh(dt);
        }
    }
}
