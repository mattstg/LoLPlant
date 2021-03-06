﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : Flow
{
    public override void Initialize(int progressNumber)
    {
        LOLAudio.Instance.ClearDisabledSounds();

        if (progressNumber <= GV.LastTutorialProgressPoint)
        {
            progressNumber = GV.LastTutorialProgressPoint + 1;
        }
        GameObject.FindObjectOfType<WS>().LinkToGV(this);
        GV.ws.dnc.Initialize();
        GV.ws.pc.Initialize();
        GV.ws.plant.Initialize(ProgressTracker.Instance.maxGrowthHeight);
        GV.ws.cameraManager.Initialize();
        GV.ws.platformManager.Initialize();
        GV.ws.dm.Initialize();
        GV.ws.popupManager.Initialize();
        GV.ws.cloudManager.Initialize();
        GV.ws.raincloudManager.Initialize(false);
        GV.ws.shadowManager.Initialize();
		GV.ws.aphidManager.Initialize();
        initialized = true;
        if (progressNumber >= 8)
            TAEventManager.Instance.Initialize(7);
        else
            TAEventManager.Instance.Initialize(progressNumber);
    }

    public override void Update(float dt)
    {
        if (initialized)
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
            GV.ws.raincloudManager.Refresh(dt);
            GV.ws.shadowManager.Refresh();
			GV.ws.aphidManager.Refresh(dt);            
        }
    }
}
