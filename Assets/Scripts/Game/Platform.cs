using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float speed = 1;
    int curWP = 0;
    int numOfWP = 0;
    Vector2[] wp;
    public bool isStaticPlatform { get { return !moves; } }
    bool moves;

    public void Initialize()
    {
        string mahName = transform.name;
        numOfWP = transform.childCount + 1; //Includes its starting position as a waypoint
        //current point is always waypoint zero
        wp = new Vector2[numOfWP];
        wp[0] = transform.position;
        for (int i = 1; i < numOfWP; i++)
            wp[i] = transform.GetChild(i - 1).position;
        foreach (Transform child in transform)
            Destroy(child.gameObject);
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
