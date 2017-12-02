using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour {

    //Tracks all CastsShadows and creates shadows
    bool firstUpdate = true;
    List<Vector2[]> staticShadowsTemp;
    Vector2[][] staticShadows;
    List<CastsShadow> dynamicShadows;

    public void Initialize()
    {
        staticShadowsTemp = new List<Vector2[]>();
        dynamicShadows = new List<CastsShadow>();
        //At this point, shadows have not called Awake yet, so no shadows
    }

    public void Refresh()
    {
        //The first time UpdateShadowManager is called, there should be no more registration of static shadows!
        if(firstUpdate)
        {
            staticShadows = staticShadowsTemp.ToArray();
            firstUpdate = false;
        }

        //Create full 2D array for shadow update
        int index = 0;
        Vector2[][] fullShadowsArr = new Vector2[dynamicShadows.Count + staticShadows.Length][];
        foreach(CastsShadow cs in dynamicShadows)
            fullShadowsArr[index++] = cs.RetrieveShadowEdges();
        foreach (Vector2[] v2 in staticShadows)
            fullShadowsArr[index++] = v2;
        CastShadows(fullShadowsArr);
    }


    public void CastShadows(Vector2[][] shadowArr)
    {
        //Given an array of Vector2 arrays. Where the Vector2 arrays is a 2D array of the two points, 0 being left, 1 being right
        foreach(Vector2[] pt in shadowArr)
        {
            //pt[0] is left point
            //pt[1] is right point

        }
    }

    public void RegisterStaticShadow(Vector2[] edges)
    {
        staticShadowsTemp.Add(edges);
    }

    public void RegisterDynamicShadow(CastsShadow cs)
    {
        dynamicShadows.Add(cs);
    }

    //Dynamic shadows need to be removed 
    public void RemoveDynamicShado(CastsShadow cs)
    {
        dynamicShadows.Remove(cs);
    }
}
