using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{

    private float fadeDuration = 1;
    private float fadeProgress = 1;
    private float sourceOpacity = 1;
    private float targetOpacity = 1;
    private Image image;
    private bool stateChange = false;

    public void Initialize()
    {
        if (fadeDuration <= 0)
            fadeDuration = 1;
        fadeProgress = fadeDuration;
        if (!image)
            image = gameObject.GetComponent<Image>();
    }

    public void Initialize(Image _image)
    {
        if (fadeDuration <= 0)
            fadeDuration = 1;
        fadeProgress = fadeDuration;
        image = _image;
    }

    public void Refresh(float dt)
    {
        stateChange = false;
        float presentOpacity = GetPresentOpacity();
        if (presentOpacity != targetOpacity)
        {
            if (fadeDuration <= 0)
                fadeDuration = 1;

            float integral = GetIntegral(fadeProgress / fadeDuration);
            float newOpacity = targetOpacity * integral + sourceOpacity * (1 - integral);
            fadeProgress += dt;
            if (fadeProgress > fadeDuration)
                fadeProgress = fadeDuration;
            ApplyOpacity(newOpacity);
            stateChange = true;
        }
    }

    public float GetVelocity(float progress)
    {
        if (progress >= 0 && progress <= 1)
        {
            return -4 * Mathf.Abs(progress - 0.5f) + 2;
        }
        else
        {
            return 0;
        }
    }

    public float GetIntegral(float progress)
    {
        if (progress < 0)
        {
            return 0;
        }
        else if (progress > 1)
        {
            return 1;
        }
        else if (progress <= 0.5)
        {
            return progress * GetVelocity(progress) / 2;
        }
        else
        {
            return 0.5f + ((progress - 0.5f) * (GetVelocity(progress) + 2) / 2);
        }
    }

    public void SetTargetOpacity(float target, float duration)
    {
        sourceOpacity = GetPresentOpacity();
        targetOpacity = target;
        fadeProgress = 0;
        fadeDuration = duration;
    }

    public void SetPresentOpacity(float opacity)
    {
        sourceOpacity = opacity;
        targetOpacity = opacity;
        fadeProgress = fadeDuration;
        ApplyOpacity(targetOpacity);
    }

    public void ApplyOpacity(float newOpacity)
    {
        Color c = image.color;
        c.a = newOpacity;
        image.color = c;
    }

    public float GetPresentOpacity()
    {
        return image.color.a;
    }

    public float GetTargetOpacity()
    {
        return targetOpacity;
    }

    public bool IsStateChanged()
    {
        return stateChange;
    }

    public Image GetImage()
    {
        return image;
    }
}
