using System.Collections;
using System.Collections.Generic;

public class MainMenuFlow : Flow {

    public override void Initialize(int progressNumber)
    {
        LOLAudio.Instance.AddDisabledSound(LOLAudio.collectRain);
        LOLAudio.Instance.AddDisabledSound(LOLAudio.land);

        UnityEngine.GameObject.FindObjectOfType<MainMenu>().SDKLoaded(progressNumber);
        UnityEngine.GameObject.FindObjectOfType<WS>().LinkToGV(this);

        GV.ws.cloudManager.Initialize(true);
        GV.ws.raincloudManager.gameObject.SetActive(true);
        GV.ws.raincloudManager.Initialize(true);
        GV.ws.pc.gameObject.SetActive(true);
        GV.ws.pc.Initialize ();

        GV.ws.platformManager.Initialize();
        GV.ws.shadowManager.Initialize();
        GV.ws.aphidManager.Initialize();
        GV.ws.dnc.Initialize(false);
        initialized = true;
    }

    public override void Update(float dt)
    {
        GV.ws.cloudManager.Refresh(dt);
        GV.ws.raincloudManager.Refresh(dt);
		GV.ws.pc.Refresh (dt);
        GV.ws.shadowManager.Refresh();
        GV.ws.aphidManager.Refresh(dt);
        GV.ws.dnc.Refresh(dt);
        GV.ws.platformManager.Refresh(dt);
    }
}
