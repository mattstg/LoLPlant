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
    public CloudManager cloudManager;

    public void LinkToGV()
    {
        GV.ws = this;
    }
}
