using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WS : MonoBehaviour
{
    public PlantGame plant;
    public DayNightCycle dnc;
    public PlayerController pc;
    public MeterUIManager mui;
    public ScoreUI scoreui;
    public PlatformManager platformManager;
    public Transform cloudParent;
    public Transform sun;

    public void Awake()
    {
        GV.ws = this;
    }

}
