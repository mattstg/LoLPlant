using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour {

    //Tracks all CastsShadows and creates shadows
    List<Shadow> shadows = new List<Shadow>();

    public void Initialize()
    {
        //At this point, shadows have not called Awake yet, so no shadows
    }

    public void Refresh()
    {
        foreach(Shadow s in shadows)
            s.Refresh();
    }

    public void RegisterShadow(CastsShadow cs, Transform parentTransform)
    {
        //step one, create shadow object for it
        GameObject go = Instantiate(Resources.Load("Prefabs/Shadow")) as GameObject;
        Shadow shadow = go.GetComponent<Shadow>();
        shadow.Initialize(cs, cs.RetrieveShadowEdges());
        if(parentTransform.gameObject.CompareTag("Rain Cloud")) //rainclouds cast darker shadows
            go.GetComponent<MeshRenderer>().material = Resources.Load("Materials/Cloud Shadow") as Material;
        shadows.Add(shadow);
        go.transform.SetParent(parentTransform);
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
