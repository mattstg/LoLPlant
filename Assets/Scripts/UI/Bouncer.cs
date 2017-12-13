using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bouncer : MonoBehaviour
{
    public RectTransform rectTransform;
    private Vector2 origin = new Vector2(0f, 0f);

    public Vector2 offset = new Vector2(0f, 0f);
    public float bounceHeight = 10f;
    public float bounceTime = 1f;
    public float pauseTime = 0f;

    public float timer = 0f;
    public float timerOffset = 0f;
    public bool isPausing = false;
    public float progress = 0f;

    public bool useWaveMotion = false;
    public bool locked = false;
    public bool lockRequested = false;

    public void InitializeBouncer()
    {
        if (!rectTransform)
            rectTransform = GetComponent<RectTransform>();
        origin = rectTransform.anchoredPosition;
        timer = 0f;
    }

    public void UpdateBouncer(float dt)
    {
        if (!locked)
        {
            timer += dt;

            if (isPausing && pauseTime <= 0f)
            {
                pauseTime = 0f;
                isPausing = false;
                timer = 0f;
            }
            if (bounceTime <= 0f)
                bounceTime = 1f;

            progress = (isPausing) ? (timer + timerOffset) / pauseTime : (timer + timerOffset) / bounceTime;

            if (isPausing && lockRequested)
            {
                locked = true;
                lockRequested = false;
            }

            if (progress >= 1f)
            {
                timer -= (isPausing) ? pauseTime : bounceTime;
                isPausing = (!isPausing && pauseTime > 0f);
                progress = (isPausing) ? (timer + timerOffset) / pauseTime : (timer + timerOffset) / bounceTime;
                if (lockRequested)
                {
                    locked = true;
                    lockRequested = false;
                }
            }

            rectTransform.anchoredPosition = new Vector2(origin.x + offset.x, origin.y + offset.y + ((isPausing) ? 0f : ((useWaveMotion) ? GV.WaveFactor(progress) : GV.BounceFactor(progress)) * bounceHeight));
        }
    }
}
