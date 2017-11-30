using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAwake : MonoBehaviour
{
    void Awake()
    {
        foreach (TintObject to in GetComponentsInChildren<TintObject>(true))
            to.ForceAwake();
    }
}
