using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour {

    public CastsShadow parentObj;
    Vector2[] staticEdges;
    bool isStatic;

    public void InitializeAsStatic(CastsShadow _parent,Vector2[] _staticEdges)
    {
        staticEdges = _staticEdges;
        parentObj = _parent;
        isStatic = true;
    }

    public void Initialize(CastsShadow _parent)
    {
        parentObj = _parent;
        isStatic = false;
    }

	public void Refresh()
    {

    }
}
