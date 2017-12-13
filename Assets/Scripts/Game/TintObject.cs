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

    public void Initialize()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        SpriteTinter.Instance.AddSprite(this);
    }

    void OnDestroy()
    {
        SpriteTinter.Instance.RemoveSprite(this);
    }
}
