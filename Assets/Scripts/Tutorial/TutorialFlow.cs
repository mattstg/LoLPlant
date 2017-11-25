using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlow : Flow {

    public override void Initialize(int progressNumber)
    {
        GameObject.FindObjectOfType<WS>().LinkToGV(this); //Force the link of WS into GV so can set the links below
        GV.ws.dnc.Initialize();                 //Initialize day night cycle
        GV.ws.plant.Initialize();               //Initialize plant
        GV.ws.pc.Initialize();                  //Initialize player controller
        // GV.ws.cameraManager.Initialize();  Cam is locked for tut, dont update
        GV.ws.platformManager.Initialize();     //Initialize platform manager
        GV.ws.dm.Initialize();                  //Initialize dashboard manager
        GV.ws.popupManager.Initialize();        //Initialize popup manager
        GV.ws.cloudManager.Initialize();        //Initialize cloud manager
        GV.ws.shadowManager.Initialize();      //Shadow Manager
    }

    public override void Update(float dt)
    { //refresh, ew
        if (!GV.Paused)
        {
            GV.ws.dnc.Refresh(dt);                //Refresh day night cycle
            GV.ws.plant.Refresh(dt);              //Refresh plant
            GV.ws.pc.Refresh(dt);                 //Refresh player controller
            // GV.ws.cameraManager.Refresh(dt);  Cam is locked for tut, dont update
            GV.ws.platformManager.Refresh(dt);    //Refresh platform manager
            GV.ws.dm.Refresh(dt);                 //Refresh dashboard manager
            GV.ws.popupManager.Refresh(dt);
            //foreach (Parallax p in GV.ws.parallaxes)  //No parallax in tutorial
            //    p.Refresh(dt);
            GV.ws.cloudManager.Refresh(dt);
            GV.ws.shadowManager.Refresh();        //Shadow Manager       

        }
    }
}
