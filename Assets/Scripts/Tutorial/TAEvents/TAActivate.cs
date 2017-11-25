using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAActivate : TAEvent
{
    string toActivate;
    bool setActive;
    //activate meters, platforms, aphid
    public TAActivate(TAEventType _eventType, string _eventName, string _toActivate, bool _setActive) : base(_eventType, _eventName)
    {
        toActivate = _toActivate;
        setActive = _setActive;
    }

    public override void PreformEvent()
    {
        switch(toActivate)
        {
            case "Platforms":
                GV.ws.platformManager.SetPlatformsActive(setActive);
                break;
            case "Aphids":
                GV.ws.aphidManager.SetAphidsActive(setActive);
                break;
            case "Panel_Water":
                GV.ws.dm.SetMeterActive(DashboardManager.MeterType.Water, setActive);
                break;
            case "Panel_Sun":
                GV.ws.dm.SetMeterActive(DashboardManager.MeterType.Sun, setActive);
                break;
            case "Panel_Growth":
                GV.ws.dm.SetMeterActive(DashboardManager.MeterType.Growth, setActive);
                break;
            case "Panel_Sugar":
                GV.ws.dm.SetMeterActive(DashboardManager.MeterType.Sugar, setActive);
                break;
        }
        //Grab the thing on the scene and set it active or not
    }
}
