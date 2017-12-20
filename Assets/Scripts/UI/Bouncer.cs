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
    public float pauseTime = -1f;
    private float revolutionTime = 1f;
    private float time = 0f;
    private float bounceProgress = 0f;
    private float pauseProgress = 0f;

    public float timeOffset = 0f;
    public bool isPausing = false;

    public bool useWaveMotion = false;
    public bool locked = false;
    public bool lockRequested = false;

    public void InitializeBouncer()
    {
        if (!rectTransform)
            rectTransform = GetComponent<RectTransform>();
        origin = rectTransform.anchoredPosition;
        if (bounceTime <= 0f)
            bounceTime = 1f;
        if (pauseTime <= 0f)
            pauseTime = -1f;
        revolutionTime = (pauseTime > 0f) ? bounceTime + pauseTime : bounceTime;
        time = bounceProgress = pauseProgress = 0f;
    }

    public void UpdateBouncer(float dt)
    {
        if (!locked)
        {
            int oldRevolution = (int)((time - timeOffset) / revolutionTime);
            time += dt;
            float newProgress = 0f;
            int newRevolution = 0;
            SetState(ref isPausing, ref newProgress, ref newRevolution);
            bool sameRevolution = (oldRevolution == newRevolution);
            if (isPausing)
            {
                pauseProgress = newProgress;
                if (lockRequested)
                {
                    locked = true;
                    lockRequested = false;
                }
            }
            else
            {
                if (!sameRevolution && lockRequested)
                {
                    time -= newProgress;
                    bounceProgress = 0f;
                    locked = true;
                    lockRequested = false;
                }
                else
                {
                    bounceProgress = newProgress;
                }
            }

            float bouncePosition = (isPausing) ? 0f : bounceHeight * ((useWaveMotion) ? GV.WaveFactor(bounceProgress / bounceTime) : GV.BounceFactor(bounceProgress / bounceTime));
            rectTransform.anchoredPosition = new Vector2(origin.x + offset.x, origin.y + offset.y + bouncePosition);
        }
    }

    public void SetState(ref bool _isPausing, ref float _progress, ref int _revolution)
    {
        float nTime = (time - timeOffset) % revolutionTime;
        _revolution = (int)((time - timeOffset) / revolutionTime);

        time = nTime + timeOffset;

        if (pauseTime > 0f && nTime >= bounceTime)
        {
            _isPausing = true;
            _progress = nTime - bounceTime;
        }
        else
        {
            _isPausing = false;
            _progress = nTime;
        }
    }
}
