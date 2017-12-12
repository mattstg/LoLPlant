using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour {

    float leftWall = -1 * ((GV.worldWidth / 2) + GV.shadowBuffer); //left shadow cutoff
    float rightWall = (GV.worldWidth / 2) + GV.shadowBuffer; //right shadow cutoff
    float floor = -1 * ((GV.worldHeight / 2) + GV.shadowBuffer); //bottom shadow cutoff
    
    float angleBuffer = GV.sunAngleBuffer;
    Vector3[] vertices = new Vector3[4];
    Mesh mesh;
    public CastsShadow parentObj;
    Vector2[] staticEdges;
    bool isStatic;

    public void InitializeAsStatic(CastsShadow _parent,Vector2[] _staticEdges)
    {
        staticEdges = _staticEdges;
        parentObj = _parent;
        isStatic = true;
        vertices[0] = staticEdges[0];
        vertices[1] = staticEdges[1];
        SetupMesh();
    }

    public void Initialize(CastsShadow _parent)
    {
        parentObj = _parent;
        isStatic = false;
        SetupMesh();
    }

    private void SetupMesh()
    {
        Renderer rend = this.GetComponent<MeshRenderer>();
        rend.sortingLayerName = "Shadows";

        mesh = this.GetComponent<MeshFilter>().mesh;
        int[] triangles = new int[6] { 2, 1, 3, 1, 2, 0 };
        mesh.triangles = triangles;
    }

	public void Refresh()
    {
        float theta = GV.ws.dnc.groundToSunAngle;
        if ((theta > angleBuffer) && (theta < 180 - angleBuffer)) //shadows stop updating when the sun gets close to horizon
        {
            if(!isStatic)
            {
                vertices[0] = parentObj.RetrieveShadowEdges()[0];
                vertices[1] = parentObj.RetrieveShadowEdges()[1];
            }

            if(theta == 90) // catch for sun directly overhead: tan(90) is division by 0
            {
                vertices[2].x = vertices[0].x;
                vertices[2].y = floor;
                vertices[3].x = vertices[1].x;
                vertices[3].y = floor;
            }
            else if(!(theta == 90))
            {
                float m = Mathf.Tan(theta * Mathf.Deg2Rad); //slope of sunlight
                vertices[2] = GetVertex(vertices[0], m);
                vertices[3] = GetVertex(vertices[1], m);
            }
        }
        mesh.vertices = vertices;
    }

    private Vector2 GetVertex(Vector2 vertexAbove, float m)
    {
        Vector2 vertex = Vector2.zero;
        float b = vertexAbove.y - (m * vertexAbove.x); //y intercept

        float tempX = (floor - b) / m; // x value at floor, used to determine whether edge ends at floor, left wall, or right wall

        if(tempX < rightWall && tempX > leftWall) // edge ends at floor
        {
            vertex.x = tempX;
            vertex.y = floor;
        }
        else if(tempX >= rightWall) //edge ends at right wall
        {
            vertex.x = rightWall;
            vertex.y = (rightWall * m) + b;
        }
        else if (tempX <= leftWall) //edge ends at left wall
        {
            vertex.x = leftWall;
            vertex.y = (leftWall * m) + b;
        }
 
        return vertex; 
    }
}
