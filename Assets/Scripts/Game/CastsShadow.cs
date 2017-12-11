using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CastsShadow
{
    void RegisterShadow(bool _isStatic);
    Vector2[] RetrieveShadowEdges();
}
