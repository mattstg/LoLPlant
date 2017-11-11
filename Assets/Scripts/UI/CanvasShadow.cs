using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasShadow : MaskableGraphic {

    public Transform k;
    public List<Transform> p = new List<Transform>();
    private int platformCount = 0;
    private float time = 0;

    protected override void Start()
    {
        base.Start();
        Initialize();
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        float x = 512 - 600 * Mathf.Cos(time * Mathf.PI / 30);
        float y = 568 + 180 * Mathf.Sin(time * Mathf.PI / 30);
        k.position = new Vector3(x, y, k.position.z);
        float normalTime = time % 60;
        float a;
        if (normalTime >= 5 && normalTime < 25)
            a = 1;
        else if (normalTime >= 30)
            a = 0;
        else if (normalTime < 5)
            a = normalTime / 5;
        else
            a = 1 - ((normalTime - 25) / 5);
        color = new Color(color.r, color.g, color.b, a * 0.3f);

        UpdateGeometry();
    }

    private void Initialize()
    {
        if (p.Count % 2 != 0)
        {
            platformCount = -1;
            Debug.Log("Error. Public List<Transform> p must have even Count, not odd. Two endpoints for each platform.");
        }
        else
        {
            platformCount = p.Count / 2;
        }
    }

    protected UIVertex[] SetupVH(Vector2[] vertices, Vector2[] uvs)
    {
        UIVertex[] vbo = new UIVertex[4];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = vertices[i];
            vert.uv0 = uvs[i];
            vbo[i] = vert;
        }
        return vbo;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        float kx = k.transform.position.x;
        float ky = k.transform.position.y;
        float ax = 0;
        float ay = 0;
        float bx = 0;
        float by = 0;
        for (int i = 0; i < platformCount; i++)
        {
            ax = p[2 * i].position.x;
            ay = p[2 * i].position.y;
            bx = p[2 * i + 1].position.x;
            by = p[2 * i + 1].position.y;

            vh.AddUIVertexQuad(SetupVH(new[] { new Vector2(ax, ay),
                                                new Vector2(bx, by),
                                                new Vector2((((bx - kx) * -ky) / (by - ky)) + kx, 0),
                                                new Vector2((((ax - kx) * -ky) / (ay - ky)) + kx, 0) },
                                       new[] { Vector2.up, new Vector2(1, 1), Vector2.right, Vector2.zero }));
        }
    }
}
