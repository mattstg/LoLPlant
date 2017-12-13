using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public class MainFlow : Flow {

    public override void Initialize(int progressNumber)
    {
        UnityEngine.GameObject.FindObjectOfType<MainMenu>().SDKLoaded(progressNumber);
        initialized = true;
    }
}
