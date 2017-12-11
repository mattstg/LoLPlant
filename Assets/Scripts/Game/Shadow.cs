using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour {

    public CastsShadow parentObj;

    public void Initialize(CastsShadow _parent)
    {
        parentObj = _parent;
    }

	public void Refresh()
    {

    }
}
