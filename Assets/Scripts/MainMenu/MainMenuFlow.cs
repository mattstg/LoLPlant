using System.Collections;
using System.Collections.Generic;

public class MainMenuFlow : Flow {

    public override void Initialize(int progressNumber)
    {
        UnityEngine.GameObject.FindObjectOfType<MainMenu>().SDKLoaded(progressNumber);
        UnityEngine.GameObject.FindObjectOfType<WS>().LinkToGV(this);
        GV.ws.cloudManager.Initialize(true);
        GV.ws.raincloudManager.Initialize(true);
		GV.ws.pc.Initialize ();
        initialized = true;
    }

    public override void Update(float dt)
    {
        GV.ws.cloudManager.Refresh(dt);
        GV.ws.raincloudManager.Refresh(dt);
		GV.ws.pc.Refresh (dt);
    }
}
