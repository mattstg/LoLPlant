﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

  
    public float parallaxMultiplier = 0.5f;
    
    public void Refresh(float dt)
    {
        this.transform.position = GV.ws.cameraTransform.position * parallaxMultiplier;
    }
}