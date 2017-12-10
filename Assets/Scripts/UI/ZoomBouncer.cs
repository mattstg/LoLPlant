using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomBouncer : MonoBehaviour
{
    public enum Type { In, Out }

    public RectTransform rectTransform;

    private float delayProgress = 0f;
    private float delayDuration = 0f;
    private bool delaying = false;
    private float zbProgress = 0f;
    private float zbDuration = 0f;
    private float zbPeakScale = 1.25f;
    private Type type;
    private Bouncer bouncer;

    public void InitializeZoomBouncer()
    {
        if (!rectTransform)
            rectTransform = GetComponent<RectTransform>();
        if (GetComponent<Bouncer>())
            bouncer = GetComponent<Bouncer>();
    }

    public void UpdateZoomBouncer(float dt)
    {
        if (delaying)
        {
            if (delayProgress < delayDuration)
            {
                delayProgress += dt;
            }
            else
            {
                delayProgress = delayDuration = 0f;
                delaying = false;
            }
        }
        else
        {
            if (zbProgress < zbDuration)
            {
                zbProgress += dt;
                float bounceScale;
                if (type == Type.Out)
                {
                    bounceScale = GV.ZoomBounceOut(zbProgress / zbDuration, zbPeakScale);
                    rectTransform.localScale = new Vector3(bounceScale, bounceScale, rectTransform.localScale.z);
                }
                else
                {
                    bounceScale = GV.ZoomBounceIn(zbProgress / zbDuration, zbPeakScale);
                    rectTransform.localScale = new Vector3(bounceScale, bounceScale, rectTransform.localScale.z);
                }
                if (bouncer)
                    bouncer.locked = true;
            }
            else
            {
                rectTransform.localScale = new Vector3(1f, 1f, rectTransform.localScale.z);
                zbProgress = zbDuration = 0f;
                if (bouncer)
                    bouncer.locked = false;
            }
        }
    }

    public void ZoomBounceIn(float duration = 1f, float delay = 0f, float peakScale = 1.4f)
    {
        type = Type.In;
        delayDuration = delay;
        zbDuration = duration;
        delayProgress = zbProgress = 0f;
        zbPeakScale = peakScale;
        if (delayDuration > 0f)
            delaying = true;
        else
        {
            delaying = false;
            delayDuration = 0f;
        }
    }

    public void ZoomBounceOut(float duration = 1f, float delay = 0f, float peakScale = 1.4f)
    {
        type = Type.Out;
        delayDuration = delay;
        zbDuration = duration;
        delayProgress = zbProgress = 0f;
        zbPeakScale = peakScale;
        if (delayDuration > 0f)
            delaying = true;
        else
        {
            delaying = false;
            delayDuration = 0f;
        }
    }
}
