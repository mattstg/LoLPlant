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

    public List<SpriteRenderer> srlist;
    float lastLightLevel = 1;

    private SpriteTinter()
    {
        srlist = new List<SpriteRenderer>();

    }


    public void AddSprite(SpriteRenderer sr)
    {
        if (!srlist.Contains(sr))
            srlist.Add(sr);
    }

    public void RemoveSprite(SpriteRenderer sr)
    {
        srlist.Remove(sr);
    }

    public void UpdateSpriteTints(float lightLevel)
    {
        if (lastLightLevel == lightLevel)
            return;

        for(int i = srlist.Count - 1; i >= 0; i--)
        {
            if(srlist[i] == null) //Object was deleted and not removed properly
            {
                Debug.Log("Sprite was not removed from sprite tint properly before removed (is null now)");
                srlist.RemoveAt(i);
            }
            else
            {
                Color c = srlist[i].color;
                c.a = lightLevel;
                srlist[i].color = c; 
            }
        }
        lastLightLevel = lightLevel;
    }

}
