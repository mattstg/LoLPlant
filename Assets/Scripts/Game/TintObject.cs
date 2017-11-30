using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isPlayer = false;

    void Awake()
    {
        Initialize();
    }

    public void ForceAwake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (spriteRenderer)
            return;
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
