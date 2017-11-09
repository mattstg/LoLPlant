using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchControls : InputManager {

    public PlayerController pc;

    protected override void MouseDown(Vector2 mouseWorldPos)
    {
        pc.MouseDown(mouseWorldPos);
    }
    
}
