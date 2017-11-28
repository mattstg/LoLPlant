using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastsShadow : MonoBehaviour {

    public bool autoWidthCalculate = true;
    public float width;
    public bool isStatic;

	public void Start()
    {
        if (autoWidthCalculate)
            width = transform.localScale.x; //The shadow sprite is 100x10, so the scale is directly porptional to the width

        if (isStatic)
            GV.ws.shadowManager.RegisterStaticShadow(this);
        else
            GV.ws.shadowManager.RegisterDynamicShadow(this);
    }
}
