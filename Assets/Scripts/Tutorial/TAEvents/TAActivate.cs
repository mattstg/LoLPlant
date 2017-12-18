using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAActivate : TAEvent
{
    public enum ActivateType { DashboardNext, DashboardNone, DashboardSun, DashboardWater, DashboardPhotosynthesis, DashboardFood, DashboardHeight, DashboardSundial, Platforms, Clouds, Aphids }
    ActivateType toActivate;
    bool setActive;
    //activate meters, platforms, aphid
    public TAActivate(ActivateType _toActivate, bool _setActive) : base(TAEventType.Action)
    {
        toActivate = _toActivate;
        setActive = _setActive;
    }

    public override void PerformEvent()
    {
        switch(toActivate)
        {
            case ActivateType.DashboardNext:
                GV.ws.dm.ShowNextElementSet();
                break;
            case ActivateType.DashboardNone:
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.None);
                break;
            case ActivateType.DashboardSun:
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Sun);
                break;
            case ActivateType.DashboardWater:
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Water);
                break;
            case ActivateType.DashboardPhotosynthesis:
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Photosynthesis);
                break;
            case ActivateType.DashboardFood:
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Food);
                break;
            case ActivateType.DashboardHeight:
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.Height);
                break;
            case ActivateType.DashboardSundial:
                GV.ws.dm.ShowElementSet(DashboardManager.DashboardElementSet.All);
                break;
            case ActivateType.Platforms:
                GV.ws.platformManager.SetPlatformsActive(setActive);
                break;
            case ActivateType.Clouds:
                GV.ws.raincloudManager.gameObject.SetActive(setActive);
                break;
            case ActivateType.Aphids:
                GV.ws.aphidManager.SetAphidsActive(setActive);
                break;
        }
        //Grab the thing on the scene and set it active or not
        //enum DashboardElementSet represents growing set of elements; see DashboardManager.cs
    }
}
