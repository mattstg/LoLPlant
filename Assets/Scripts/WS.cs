using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WS : MonoBehaviour
{
    public Flow currentFlow;
    public DayEndManager dayEndManager;

    public Plant plant;
    public DayNightCycle dnc;
    public PlayerController pc;
    public EventSystem es;

    public DashboardManager dm;
    public PopupManager popupManager;

    public List<Parallax> parallaxes;
    public Transform cameraTransform;
    public CameraManager cameraManager;

    public PlatformManager platformManager;
    public AphidManager aphidManager;
    
    public CloudManager cloudManager;
    public RaincloudManager raincloudManager;
    public ShadowManager shadowManager;
    public Transform sun;

    public void LinkToGV(Flow _flow)
    {
        currentFlow = _flow;
        GV.ws = this;
    }
}
