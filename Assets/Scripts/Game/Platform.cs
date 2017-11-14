using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public float speed = 1;
    int curWP = 0;
    int numOfWP = 0;
    Vector2[] wp;
    bool moves;

    public void Initialize()
    {
        numOfWP = transform.childCount;
        if (numOfWP > 0)
        {
            //current point is always waypoint zero
            wp = new Vector2[numOfWP];
            wp[0] = transform.position;
            for (int i = 1; i < numOfWP; i++)
                wp[i] = transform.GetChild(i).position;
            foreach (Transform child in transform)
                Destroy(child);
            moves = true;
        }
        else
        {
            moves = false;
        }
    }

	public void Refresh(float dt)
    {
        if (!moves)
            return;
        transform.position = Vector2.MoveTowards(transform.position, wp[curWP], speed * Time.deltaTime);
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
