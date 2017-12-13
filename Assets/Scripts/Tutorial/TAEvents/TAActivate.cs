using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAActivate : TAEvent
{
    string toActivate;
    bool setActive;
    //activate meters, platforms, aphid
    public TAActivate(string _toActivate, bool _setActive) : base(TAEventType.Action)
    {
        toActivate = _toActivate;
        setActive = _setActive;
    }

    public override void PerformEvent()
    {
        switch(toActivate)
        {
            case "Dashboard":
                GV.ws.dm.ShowNextElementSet();
                break;
            case "DashboardNone":
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.None);
                break;
            case "DashboardSun":
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Sun);
                break;
            case "DashboardWater":
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Water);
                break;
            case "DashboardPhotosynthesis":
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Photosynthesis);
                break;
            case "DashboardFood":
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Food);
                break;
            case "DashboardHeight":
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Height);
                break;
            case "DashboardSundial":
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.All);
                break;
            case "Platforms":
                GV.ws.platformManager.SetPlatformsActive(setActive);
                break;
            case "Clouds":
                GV.ws.raincloudManager.gameObject.SetActive(setActive);
                break;
            case "Aphids":
                GV.ws.aphidManager.SetAphidsActive(setActive);
                break;
        }
        //Grab the thing on the scene and set it active or not
        //enum DashboardElementSet represents growing set of elements; see DashboardManager.cs
    }
}
