using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CastsShadow
{
    void RegisterStaticShadow();
    void RegisterDynamicShadow();
    Vector2[] RetrieveShadowEdges();

}
