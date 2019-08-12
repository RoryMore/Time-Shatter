using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleRangeIndicator : MonoBehaviour
{
    Mesh mesh;
    Mesh castTimeMesh;
    public Material indicatorMaterial;

    Vector3[] normals;
    Vector2[] uv;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        mesh = new Mesh();
        mesh.vertices = new Vector3[4];
        mesh.triangles = new int[3 * 2];

        castTimeMesh = new Mesh();
        castTimeMesh.vertices = new Vector3[4];
        castTimeMesh.triangles = new int[3 * 2];

        normals = new Vector3[4];
        uv = new Vector2[4];

        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(0, 0);
        }
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = new Vector3(0, 1, 0);
        }

        mesh.uv = uv;
        mesh.normals = normals;

        castTimeMesh.uv = uv;
        castTimeMesh.normals = normals;
    }

    public void DrawIndicator(float attackWidth, float minRange, float maxRange)
    {
        float angleLookAt = GetForwardAngle();

        float angleStart = angleLookAt - attackWidth;
        float angleEnd = angleLookAt + attackWidth;
        float angleDelta = (angleEnd - angleStart);

        float angleCurrent = angleStart;
        float angleNext = angleStart + angleDelta;

        Vector3 posCurrentMin = Vector3.zero;
        Vector3 posCurrentMax = Vector3.zero;

        Vector3 posNextMin = Vector3.zero;
        Vector3 posNextMax = Vector3.zero;

        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[3 * 2];

        for (int i = 0; i < 1; i++)
        {
            Vector3 sphereCurrent = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleCurrent)), 0,
                                                Mathf.Cos(Mathf.Deg2Rad * (angleCurrent)));

            Vector3 sphereNext = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleNext)), 0,
                                             Mathf.Cos(Mathf.Deg2Rad * (angleNext)));

            posCurrentMin = transform.position + sphereCurrent * minRange;
            posCurrentMax = transform.position + sphereCurrent * maxRange;

            posNextMin = transform.position + sphereNext * minRange;
            posNextMax = transform.position + sphereNext * maxRange;

            int a = 4 * i;
            int b = 4 * i + 1;
            int c = 4 * i + 2;
            int d = 4 * i + 3;

            vertices[a] = posCurrentMin;
            vertices[b] = posCurrentMax;
            vertices[c] = posNextMax;
            vertices[d] = posNextMin;

            triangles[6 * i] = a;
            triangles[6 * i + 1] = b;
            triangles[6 * i + 2] = c;
            triangles[6 * i + 3] = c;
            triangles[6 * i + 4] = d;
            triangles[6 * i + 5] = a;

            angleCurrent += angleDelta;
            angleNext += angleDelta;
        }
        //for (int i = 0; i < uv.Length; i++)
        //{
        //uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        //}

        //mesh.uv = uv;
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
    }

    float GetForwardAngle()
    {
        return 90 - Mathf.Rad2Deg * Mathf.Atan2(transform.forward.z, transform.forward.x);
    }
}
