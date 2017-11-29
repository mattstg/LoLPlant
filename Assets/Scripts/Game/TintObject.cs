using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isPlayer = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            SpriteTinter.Instance.AddSprite(this);
    }

    void OnDestroy()
    {
        if (spriteRenderer)
            SpriteTinter.Instance.RemoveSprite(this);
    }
}
