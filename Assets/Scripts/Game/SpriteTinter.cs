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
    float lastLightLevel = -10f;
    float lastPlayerLightLevel = -10f;

    private SpriteTinter()
    {
        tintObjectList = new List<TintObject>();
    }


    public void AddSprite(TintObject to)
    {
        if (!tintObjectList.Contains(to))
            tintObjectList.Add(to);
        InitialSpriteUpdate(to);
    }

    public void RemoveSprite(TintObject to)
    {
        tintObjectList.Remove(to);
    }

    public void UpdateSpriteTints(float lightLevel, float playerOffset)
    {
        float playerLightLevel = Mathf.Clamp(lightLevel + playerOffset, 0f, 1f);
        for (int i = tintObjectList.Count - 1; i >= 0; i--)
        {
            if(tintObjectList[i] == null) //Object was deleted and not removed properly
            {
                //Debug.Log("Sprite was not removed from sprite tint properly before removed (is null now)");
                tintObjectList.RemoveAt(i);
            }
            else
            {
                if (tintObjectList[i].isPlayer)
                {
                    if (playerLightLevel != lastPlayerLightLevel)
                    {
                        Color c = tintObjectList[i].spriteRenderer.color;
                        c.r = c.g = c.b = playerLightLevel;
                        tintObjectList[i].spriteRenderer.color = c;
                    }
                }
                else if (lightLevel != lastLightLevel)
                {
                    Color c = tintObjectList[i].spriteRenderer.color;
                    c.r = c.g = c.b = lightLevel;
                    tintObjectList[i].spriteRenderer.color = c;
                } 
            }
        }
        lastLightLevel = lightLevel;
        lastPlayerLightLevel = playerLightLevel;
    }

    private void InitialSpriteUpdate(TintObject to)
    {
        if (to.isPlayer)
        {
            if (lastPlayerLightLevel >= 0 && lastPlayerLightLevel <= 1f)
            {
                Color c = to.spriteRenderer.color;
                c.r = c.g = c.b = lastPlayerLightLevel;
                to.spriteRenderer.color = c;
            }
        }
        else
        {
            if (lastLightLevel >= 0 && lastLightLevel <= 1f)
            {
                Color c = to.spriteRenderer.color;
                c.r = c.g = c.b = lastLightLevel;
                to.spriteRenderer.color = c;
            }
        }
    }
}
