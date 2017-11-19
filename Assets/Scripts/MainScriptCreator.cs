using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScriptCreator : MonoBehaviour {

    public CurrentState levelType;
    public static bool MS_CREATED = false;

    public void Awake()
    {
        if(!MS_CREATED)
        {
            //Create the DoNotDestroy object containing the mainscript
            GameObject msobj = new GameObject();
            msobj.name = "MainScript";
            msobj.AddComponent<MainScript>().Initialize(levelType);            
            DontDestroyOnLoad(msobj);
            MS_CREATED = true;
        }
        Destroy(this);
    }
}
