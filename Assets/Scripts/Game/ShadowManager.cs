using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour {

    //Tracks all CastsShadows and creates shadows
    List<Shadow> shadows = new List<Shadow>();
    List<Material> allPlatformMaterials = new List<Material>();
    List<Material> allCloudMaterials = new List<Material>();

    readonly float defaultShadowAlpha = 80/255f;
    readonly float defaultCloudShadowAlpha = 120/255f;
    readonly float sunAngleMaxAlpha = 25;
    readonly float sunAngleMinAlpha = 8;
    float alphaLastTurn = 1;


    public void Initialize()
    {
        StandaloneShadow[] sass = GameObject.FindObjectsOfType<StandaloneShadow>();
        foreach (StandaloneShadow sas in sass)
            sas.Initialize();
        //At this point, shadows have not called Awake yet, so no shadows
    }

    public void Refresh()
    {
        float sunAngle = GV.ws.dnc.GetSunAngle();
        float angleReduced = Mathf.Min(sunAngle, 180 - sunAngle);
        float setAlpha;
        if (angleReduced <= sunAngleMaxAlpha)
            setAlpha = Mathf.Clamp01((angleReduced - sunAngleMinAlpha) / (sunAngleMaxAlpha - sunAngleMinAlpha));
        else
            setAlpha = 1;

        //actual alpha since it's just a multiplier  0-1
        
        if (setAlpha != alphaLastTurn)
        {
            alphaLastTurn = setAlpha;

            foreach(Material m in allPlatformMaterials)
            {
                Color c = m.color;
                c.a = setAlpha * defaultShadowAlpha;
                m.color = c;
            }
            foreach (Material m in allCloudMaterials)
            {
                Color c = m.color;
                c.a = setAlpha * defaultCloudShadowAlpha;
                m.color = c;
            }
        }
            
        foreach (Shadow s in shadows)
            s.Refresh();
    }



    public void RegisterShadow(CastsShadow cs, Transform parentTransform)
    {
        //step one, create shadow object for it
        GameObject go = Instantiate(Resources.Load("Prefabs/Shadow")) as GameObject;
        Shadow shadow = go.GetComponent<Shadow>();
        shadow.Initialize(cs, cs.RetrieveShadowEdges());
        if (parentTransform.gameObject.CompareTag("Rain Cloud")) //rainclouds cast darker shadows
        {
            go.GetComponent<MeshRenderer>().material = Resources.Load("Materials/Cloud Shadow") as Material;
            allCloudMaterials.Add(go.GetComponent<MeshRenderer>().material);
        }
        else
            allPlatformMaterials.Add(go.GetComponent<MeshRenderer>().material);
        shadows.Add(shadow);
        go.transform.SetParent(parentTransform);
        
    }

    //Dynamic shadows need to be removed 
    //public void RemoveShadow(CastsShadow cs)
    //{
    //    for(int i = shadows.Count; i > 0; i--)
    //    {
    //        if (shadows[i].parentObj == cs)
    //        {
    //            shadows.RemoveAt(i);
    //            return;
    //        }
    //    }
    //}
}
