using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera cam;
    public Transform targetToFollow;
    public float defaultDampTime = 0.5f;
    public float dampTime = 0.5f;
    private Vector3 velocity = Vector3.zero;
    public Vector3 offset;
    public bool useLimits = true;
    public Transform cameraLimitBottomLeft;
    public Transform cameraLimitTopRight;
    public Vector2 targetLimitsDimensions;
    private float viewportMargin = 0.5f;


    public void Initialize()
    {
        cam = GetComponent<Camera>();
        dampTime = defaultDampTime;
    }

    public void Refresh(float dt)
    {
        if (targetToFollow)
        {
            Vector3 target = GetTarget();
            Vector3 point = cam.WorldToViewportPoint(target);
            Vector3 delta = target - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }

    public Vector3 GetTarget()
    {
        Vector3 target = new Vector3(targetToFollow.position.x + offset.x, targetToFollow.position.y + offset.y, -10);
        dampTime = defaultDampTime;

        if (useLimits)
        {
            Vector3 vpTarget = cam.WorldToViewportPoint(target);
            Vector3 vpTopRight = cam.WorldToViewportPoint(cameraLimitTopRight.position);
            Vector3 vpBottomLeft = cam.WorldToViewportPoint(cameraLimitBottomLeft.position);
            Vector2 vpTargetTopRight =   new Vector2(Mathf.Min(0.5f + targetLimitsDimensions.x / 2f, 1f), Mathf.Min(0.5f + targetLimitsDimensions.y / 2f, 1f));
            Vector2 vpTargetBottomLeft = new Vector2(Mathf.Max(0.5f - targetLimitsDimensions.x / 2f, 0f), Mathf.Max(0.5f - targetLimitsDimensions.y / 2f, 0f));

            bool overrule = false;

            if (vpTarget.x < vpBottomLeft.x + viewportMargin)
            {
                vpTarget.x = vpBottomLeft.x + viewportMargin;
                overrule = true;
            }
            if (vpTarget.x > vpTopRight.x - viewportMargin)
            {
                vpTarget.x = vpTopRight.x - viewportMargin;
                overrule = true;
            }
            if (vpTarget.y < vpBottomLeft.y + viewportMargin)
            {
                vpTarget.y = vpBottomLeft.y + viewportMargin;
                overrule = true;
            }
            if (vpTarget.y > vpTopRight.y - viewportMargin)
            {
                vpTarget.y = vpTopRight.y - viewportMargin;
                overrule = true;
            }

            if (vpTarget.x < vpTargetBottomLeft.x)
                dampTime /= (0.5f - vpTarget.x) / (0.5f - vpTargetBottomLeft.x);
            else if (vpTarget.x > vpTargetTopRight.x)
                dampTime /= (vpTarget.x - 0.5f) / (vpTargetTopRight.x - 0.5f);
            if (vpTarget.y < vpTargetBottomLeft.y)
                dampTime /= (0.5f - vpTarget.y) / (0.5f - vpTargetBottomLeft.y);
            if (vpTarget.y > vpTargetTopRight.y)
                dampTime /= (vpTarget.y - 0.5f) / (vpTargetTopRight.y - 0.5f);

            if (overrule)
            {
                target = cam.ViewportToWorldPoint(vpTarget);
                target.z = -10;
            }
        }
        return target;
    }

    public void SetZoom(float z)
    {
        cam.orthographicSize = z;
    }
}
