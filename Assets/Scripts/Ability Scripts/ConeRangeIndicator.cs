﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeRangeIndicator : MonoBehaviour
{
    public int quality = 15;
    Mesh mesh;
    public Material indicatorMaterial;

    //public float angle;
    //public float minRange;
    //public float maxRange;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.vertices = new Vector3[4 * quality];
        mesh.triangles = new int[3 * 2 * quality];

        Vector3[] normals = new Vector3[4 * quality];
        Vector2[] uv = new Vector2[4 * quality];

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

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawIndicator(float angle, float minRange, float maxRange)
    {
        float angleLookAt = GetForwardAngle();

        float angleStart = angleLookAt - angle;
        float angleEnd = angleLookAt + angle;
        float angleDelta = (angleEnd - angleStart) / quality;

        float angleCurrent = angleStart;
        float angleNext = angleStart + angleDelta;

        Vector3 posCurrentMin = Vector3.zero;
        Vector3 posCurrentMax = Vector3.zero;

        Vector3 posNextMin = Vector3.zero;
        Vector3 posNextMax = Vector3.zero;

        Vector3[] vertices = new Vector3[4 * quality];
        int[] triangles = new int[3 * 2 * quality];

        for (int i = 0; i < quality; i++)
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

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
    }

    float GetForwardAngle()
    {
        return 90 - Mathf.Rad2Deg * Mathf.Atan2(transform.forward.z, transform.forward.x);
    }
}
