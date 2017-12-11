using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour {

    //Tracks all CastsShadows and creates shadows
    List<Shadow> shadows;

    public void Initialize()
    {
        shadows = new List<Shadow>();
        //At this point, shadows have not called Awake yet, so no shadows
    }

    public void Refresh()
    {
        foreach(Shadow s in shadows)
        {
            s.Refresh();
        }
        
    }

    public void RegisterShadow(CastsShadow cs, bool isStatic)
    {
        //step one, create shadow object for it
        GameObject go = Instantiate(Resources.Load("Prefabs/Shadow")) as GameObject;
        Shadow shadow = go.GetComponent<Shadow>();
        if (isStatic)
            shadow.Initialize(cs);
        else
            shadow.InitializeAsStatic(cs, cs.RetrieveShadowEdges());
        shadows.Add(shadow);
    }

    //Dynamic shadows need to be removed 
    public void RemoveShadow(CastsShadow cs)
    {
        for(int i = shadows.Count; i > 0; i--)
        {
            if (shadows[i].parentObj == cs)
            {
                shadows.RemoveAt(i);
                return;
            }
        }
    }
}
