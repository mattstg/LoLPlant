using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : Flow
{
    public override void Initialize(int progressNumber)
    {
        GameObject.FindObjectOfType<WS>().LinkToGV(this);
        GV.ws.dnc.Initialize();
        GV.ws.plant.Initialize();
        GV.ws.pc.Initialize();
        GV.ws.cameraManager.Initialize();
        GV.ws.platformManager.Initialize();
        GV.ws.dm.Initialize();
        GV.ws.popupManager.Initialize();
        GV.ws.cloudManager.Initialize();
        GV.ws.shadowManager.Initialize();
		GV.ws.aphidManager.Initialize();   
    }

    public override void Update(float dt)
    {
        if (!GV.Paused)
        {
            GV.ws.dnc.Refresh(dt);
            GV.ws.plant.Refresh(dt);
            GV.ws.pc.Refresh(dt);
            GV.ws.cameraManager.Refresh(dt);
            GV.ws.platformManager.Refresh(dt);
            GV.ws.dm.Refresh(dt);
            GV.ws.popupManager.Refresh(dt);
            foreach (Parallax p in GV.ws.parallaxes)
                p.Refresh(dt);
			GV.ws.cloudManager.Refresh(dt);
            GV.ws.shadowManager.Refresh();
			GV.ws.aphidManager.Refresh(dt); 
        }
    }
}
