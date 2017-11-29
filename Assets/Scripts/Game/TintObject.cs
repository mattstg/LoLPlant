using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintObject : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            SpriteTinter.Instance.AddSprite(spriteRenderer);
    }

    void OnDestroy()
    {
        if (spriteRenderer)
            SpriteTinter.Instance.RemoveSprite(spriteRenderer);
    }
}
