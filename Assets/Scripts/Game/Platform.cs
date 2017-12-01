using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public enum Side { Left = 0, Right = 1 }
    public bool hasAphid;
    public bool castsShadow;

    public float speed = 1;
    int curWP = 0;
    int numOfWP = 0;
    Vector2[] wp;
    public bool isStaticPlatform { get { return !moves; } }
    bool moves;
    Vector2[] edgeOffsets;
   
    public Vector2 GetSide(Side side)
    {
        return edgeOffsets[(int)side] + new Vector2(transform.position.x, transform.position.y);
    }

    public void Initialize()
    {
        //Calculate edges
        edgeOffsets = new Vector2[2];
        Transform edgesParent = transform.Find("Edges");
        if (!edgesParent || !edgesParent.GetChild(0) || !edgesParent.GetChild(1))
        {
            Debug.Log("Error! Platform: " + transform.name + " \"Edges\" transform missing, or missing two children objects for edges");
            edgeOffsets[0] = edgeOffsets[1] = new Vector2();
            return;
        }
        Transform child0, child1;
        child0 = edgesParent.GetChild(0).transform;
        child1 = edgesParent.GetChild(1).transform;
        edgeOffsets[0] = (child0.position.x > child1.position.x) ? child0.position : child0.position;  //Left edge is stored in first offset array slot
        edgeOffsets[1] = (child0.position.x <= child1.position.x) ? child0.position : child0.position;
        edgeOffsets[0] = edgeOffsets[0] - new Vector2(transform.position.x, transform.position.y);   //Edges are turned into offset instead of hard values
        edgeOffsets[1] = edgeOffsets[1] - new Vector2(transform.position.x, transform.position.y);
        Destroy(edgesParent.gameObject);

        //Calculate Waypoints
        Transform wpTransform = transform.Find("WP");
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
