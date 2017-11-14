using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : Flow {

    
    // Update is called once per frame
    public override void Initialize(int progressNumber)
    {
        GV.ws.dnc.Initialize();                 //Initialize day night cycle
        GV.ws.pc.Initialize();                  //Initialize player controller
        GV.ws.mui.Initialize();                 //Initialize meter manager
        GV.ws.platformManager.Initialize();     //Initialize platform manager
    }

    public override void Update(float dt)
    { //refresh, ew
        GV.ws.dnc.Refresh(dt);                 //Refresh day night cycle
        GV.ws.pc.Refresh(dt);                  //Refresh player controller
        GV.ws.mui.Refresh(dt);                 //Refresh meter manager
        GV.ws.platformManager.Refresh(dt);     //Refresh platform manager
    }
}
