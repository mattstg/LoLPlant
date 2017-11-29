using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTinter
{
    #region Singleton
    private static SpriteTinter instance;

    

    public static SpriteTinter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SpriteTinter();
            }
            return instance;
        }
    }
    #endregion

    public List<TintObject> tintObjectList;
    float lastLightLevel = 1f;
    float lastPlayerOverride = -1f;

    private SpriteTinter()
    {
        tintObjectList = new List<TintObject>();
    }


    public void AddSprite(TintObject to)
    {
        if (!tintObjectList.Contains(to))
            tintObjectList.Add(to);
    }

    public void RemoveSprite(TintObject to)
    {
        tintObjectList.Remove(to);
    }

    public void UpdateSpriteTints(float lightLevel, float playerOverride)
    {
        for(int i = tintObjectList.Count - 1; i >= 0; i--)
        {
            if(tintObjectList[i] == null) //Object was deleted and not removed properly
            {
                Debug.Log("Sprite was not removed from sprite tint properly before removed (is null now)");
                tintObjectList.RemoveAt(i);
            }
            else
            {
                if (tintObjectList[i].isPlayer && playerOverride > -0.001f)
                {
                    if (playerOverride != lastPlayerOverride)
                    {
                        Color c = tintObjectList[i].spriteRenderer.color;
                        c.r = c.g = c.b = Mathf.Clamp(playerOverride, 0f, 1f);
                        tintObjectList[i].spriteRenderer.color = c;
                    }
                }
                else if (lightLevel != lastLightLevel)
                {
                    Color c = tintObjectList[i].spriteRenderer.color;
                    c.r = c.g = c.b = Mathf.Clamp(lightLevel, 0f, 1f);
                    tintObjectList[i].spriteRenderer.color = c;
                } 
            }
        }
        lastLightLevel = lightLevel;
        lastPlayerOverride = playerOverride;
    }

}
