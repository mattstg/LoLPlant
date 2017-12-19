using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, CastsShadow {

    public bool hasAphid;
    public bool castsShadow = true;

    public float speed = 1;
    int curWP = 0;
    int numOfWP = 0;
    Vector2[] wp;
    public bool isStaticPlatform { get { return !moves; } }
    bool moves;
    Vector2[] edgeOffsets;

	private GameObject player;

    public Vector2[] GetEdges()
    {
		return new Vector2[] { edgeOffsets[0] + (Vector2)transform.position + new Vector2(0, -GV.platformHeight), edgeOffsets[1] + (Vector2)transform.position + new Vector2(0, -GV.platformHeight) };
    }

    public Vector2 GetSidePoint(bool left, bool offset = false)
    {
        int side = (left) ?0:1;
        if (!offset)
            return edgeOffsets[side] + new Vector2(transform.position.x, transform.position.y);
        else
            return edgeOffsets[side];
    }

    public virtual void Initialize()
    {
        //Calculate edges
		/*
		 * 	edgeOffsets = new Vector2[2];
        EdgeCollider2D ec = GetComponent<EdgeCollider2D>();
        edgeOffsets[0] = ec.points[0] * transform.localScale.x;  //Edge points dont take scale into account in unity editor
        edgeOffsets[1] = ec.points[1] * transform.localScale.x;
        edgeOffsets[0].y += ec.offset.y * transform.localScale.y;
        edgeOffsets[1].y += ec.offset.y * transform.localScale.y;
		*/

        //Calculate Waypoints
        numOfWP = transform.childCount + 1; //Includes its starting position as a waypoint
        //current point is always waypoint zero
        wp = new Vector2[numOfWP];
        wp[0] = transform.position;
        for (int i = 1; i < numOfWP; i++)
            wp[i] = transform.GetChild(i - 1).position;
        foreach (Transform t in transform)
            Destroy(t.gameObject); //delete children wp
        moves = (numOfWP > 1);

		float[] spriteAndEdge = GV.GetSpriteAndEdge (transform.localScale.x);
		transform.localScale = new Vector3(1,1,1);
		GetComponent<SpriteRenderer> ().sprite = GV.GetPlatformSprite (6 - (int)spriteAndEdge [0]);
		edgeOffsets = new Vector2[2];
		EdgeCollider2D ec = GetComponent<EdgeCollider2D>();
		ec.offset = new Vector2(0,GV.platformHeight);
		ec.points = new Vector2[] { new Vector2(-spriteAndEdge [1],0),new Vector2(spriteAndEdge [1],0)};
		edgeOffsets [0] = ec.points [0];
		edgeOffsets [1] = ec.points [1];
        if (castsShadow)
            GV.ws.shadowManager.RegisterShadow(this, transform);
    }

	public void Refresh(float dt)
    {
		
        if (!moves)
            return;
		
		if (player == null) {
			transform.position = Vector2.MoveTowards (transform.position, wp [curWP], speed * dt);
			if (ReachedDest (transform.position, wp [curWP])) {
				curWP++;
				curWP %= numOfWP;
			}
		} else {
			Vector3 prevPos = transform.position;

			transform.position = Vector2.MoveTowards (transform.position, wp [curWP], speed * dt);
			Vector3 playersNewPos = player.transform.position + transform.position - prevPos;
			player.transform.position = playersNewPos;
			if (ReachedDest (transform.position, wp [curWP])) {
				curWP++;
				curWP %= numOfWP;
			}
		}
    }

    
    private bool ReachedDest(Vector2 pos1, Vector2 pos2)
    {
        return (pos1 - pos2).sqrMagnitude < .1f;
    }

    public virtual Vector2[] RetrieveShadowEdges()
    {
        return GetEdges();
    }

	public void AddPlayer(GameObject _player){
		player = _player;
	}

	public void RemovePlayer(){
		player = null;
	}
}
