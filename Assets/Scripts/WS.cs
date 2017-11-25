using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WS : MonoBehaviour
{
    public Plant plant;
    public DayNightCycle dnc;
    public PlayerController pc;
    public DashboardManager dm;
    public PlatformManager platformManager;
    public Transform cloudParent;
    public Transform sun;
    public ShadowManager shadowManager;
    public List<Parallax> parallaxes;
    public Transform cameraTransform;
    public CameraManager cameraManager;
    public CloudManager cloudManager;
    public Flow currentFlow;
    public DayEndManager dayEndManager;
    public BasicPopup basicPopup;
    public AphidManager aphidManager;


    public void LinkToGV(Flow _flow)
    {
        currentFlow = _flow;
        GV.ws = this;
    }
}
