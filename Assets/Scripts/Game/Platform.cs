using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public bool hasAphid;
    public bool castsShadow;

    public float speed = 1;
    int curWP = 0;
    int numOfWP = 0;
    Vector2[] wp;
    public bool isStaticPlatform { get { return !moves; } }
    bool moves;
    Vector2[] edges;

    public void Initialize()
    {
        //Calculate Waypoints
        Transform wpTransform = transform.Find("wp");
        int numOfWPinWPTransform = (wpTransform != null) ?wpTransform.childCount:0;
        numOfWP = numOfWPinWPTransform + 1; //Includes its starting position as a waypoint
        //current point is always waypoint zero
        wp = new Vector2[numOfWP];
        wp[0] = transform.position;
        for (int i = 1; i < numOfWP; i++)
            wp[i] = wpTransform.GetChild(i - 1).position;
        if (wpTransform != null)
            Destroy(wpTransform.gameObject);
        moves = (numOfWP > 1);

        //Calculate edges
        edges = new Vector2[2];
        Transform edgesParent = transform.Find("edges");

    }

	public void Refresh(float dt)
    {
        if (!moves)
            return;
        transform.position = Vector2.MoveTowards(transform.position, wp[curWP], speed * dt);
        if(ReachedDest(transform.position,wp[curWP]))
        {
            curWP++;
            curWP %= numOfWP;
        }
    }

    
    private bool ReachedDest(Vector2 pos1, Vector2 pos2)
    {
        return (pos1 - pos2).sqrMagnitude < .1f;
    }
}
