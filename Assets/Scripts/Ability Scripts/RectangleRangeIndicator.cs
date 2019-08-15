﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleRangeIndicator : MonoBehaviour
{
    [HideInInspector] public Mesh mesh;
    Mesh castTimeMesh;
    public Material indicatorMaterial;

    Vector3[] normals;
    Vector2[] uv;

    Vector3[] vertices;
    int[] triangles;

    [HideInInspector] public Vector3 corner1;
    [HideInInspector] public Vector3 corner2;
    [HideInInspector] public Vector3 corner3;
    [HideInInspector] public Vector3 corner4;

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

        vertices = new Vector3[4];
        triangles = new int[3 * 2];

        mesh.uv = uv;
        mesh.normals = normals;

        castTimeMesh.uv = uv;
        castTimeMesh.normals = normals;
    }

    public void DrawIndicator(float attackWidth, float minRange, float maxRange)
    {
        float angleLookAt = GetForwardAngle();

        //float angleStart = angleLookAt - (attackWidth * 0.5f);
        //float angleEnd = angleLookAt + (attackWidth * 0.5f);
        //float angleDelta = (angleEnd - angleStart);

        //float angleCurrent = angleStart;
        //float angleNext = angleStart + angleDelta;

        Vector3 posCurrentMin = Vector3.zero;
        Vector3 posCurrentMax = Vector3.zero;

        Vector3 posNextMin = Vector3.zero;
        Vector3 posNextMax = Vector3.zero;

        //vertices = new Vector3[4];
        //triangles = new int[3 * 2];

        float halfAttackWidth = attackWidth * 0.5f;

        for (int i = 0; i < 1; i++)
        {
            //Vector3 sphereCurrent = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleStart)), 0, Mathf.Cos(Mathf.Deg2Rad * (angleStart)));

            //Vector3 sphereNext = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleEnd)), 0, Mathf.Cos(Mathf.Deg2Rad * (angleEnd)));



            //posCurrentMin = transform.position + sphereCurrent;
            //posCurrentMax = transform.position + sphereCurrent * maxRange;

            //posNextMin = transform.position + sphereNext;
            //posNextMax = transform.position + sphereNext * maxRange;

            posCurrentMin = transform.position;
            posCurrentMin.z -= halfAttackWidth;

            posCurrentMax = transform.position;
            posCurrentMax.z -= halfAttackWidth;
            //if (Mathf.Sign(transform.position.x) > 0)
            //{
                posCurrentMax.x += maxRange;
            //}
            //else
            //{
                //posCurrentMax.x += -maxRange;
            //}

            posNextMin = transform.position;
            posNextMin.z += halfAttackWidth;

            posNextMax = transform.position;
            posNextMax.z += halfAttackWidth;
            //if (Mathf.Sign(transform.position.x) > 0)
            //{
                posNextMax.x += maxRange;
            //}
            //else
            //{
                //posNextMax.x += -maxRange;
            //}

            int a = 4 * i;
            int b = 4 * i + 1;
            int c = 4 * i + 2;
            int d = 4 * i + 3;

            vertices[a] = posCurrentMin;
            vertices[b] = posCurrentMax;
            vertices[c] = posNextMax;
            vertices[d] = posNextMin;

            Quaternion qangle = Quaternion.AngleAxis(angleLookAt - 90.0f, Vector3.up);

            vertices[a] -= transform.position;
            vertices[b] -= transform.position;
            vertices[c] -= transform.position;
            vertices[d] -= transform.position;

            vertices[a] = qangle * vertices[a];
            vertices[b] = qangle * vertices[b];
            vertices[c] = qangle * vertices[c];
            vertices[d] = qangle * vertices[d];

            vertices[a] += transform.position;
            vertices[b] += transform.position;
            vertices[c] += transform.position;
            vertices[d] += transform.position;

            triangles[6 * i] = a;
            triangles[6 * i + 1] = d;
            triangles[6 * i + 2] = c;
            triangles[6 * i + 3] = c;
            triangles[6 * i + 4] = b;
            triangles[6 * i + 5] = a;

            corner1 = vertices[a];
            corner2 = vertices[b];
            corner3 = vertices[c];
            corner4 = vertices[d];

            //angleCurrent += angleDelta;
            //angleNext += angleDelta;
        }
        //for (int i = 0; i < uv.Length; i++)
        //{
        //uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        //}

        //mesh.uv = uv;
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Bounds should be automatically recalculated when setting triangles
        mesh.RecalculateBounds();

        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
    }

    public void DrawCastTimeIndicator(float attackWidth, float minRange, float drawPercent, float maxRange)
    {
        float angleLookAt = GetForwardAngle();

        float halfAttackWidth = attackWidth * 0.5f;

        //float angleStart = angleLookAt - (attackWidth * 0.5f);
        //float angleEnd = angleLookAt + (attackWidth * 0.5f);
        //float angleDelta = (angleEnd - angleStart);

        //float angleCurrent = angleStart;
        //float angleNext = angleStart + angleDelta;

        Vector3 posCurrentMin = Vector3.zero;
        Vector3 posCurrentMax = Vector3.zero;

        Vector3 posNextMin = Vector3.zero;
        Vector3 posNextMax = Vector3.zero;

        //vertices = new Vector3[4];
        //triangles = new int[3 * 2];

        for (int i = 0; i < 1; i++)
        {
            //Vector3 sphereCurrent = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleStart)), 0, Mathf.Cos(Mathf.Deg2Rad * (angleStart)));

            //Vector3 sphereNext = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angleEnd)), 0, Mathf.Cos(Mathf.Deg2Rad * (angleEnd)));



            //posCurrentMin = transform.position + sphereCurrent;
            //posCurrentMax = transform.position + sphereCurrent * maxRange;

            //posNextMin = transform.position + sphereNext;
            //posNextMax = transform.position + sphereNext * maxRange;

            posCurrentMin = transform.position;
            posCurrentMin.z -= (halfAttackWidth * drawPercent);

            posCurrentMax = transform.position;
            posCurrentMax.z -= (halfAttackWidth * drawPercent);
            //if (Mathf.Sign(transform.position.x) > 0)
            //{
            posCurrentMax.x += maxRange;
            //}
            //else
            //{
            //posCurrentMax.x += -maxRange;
            //}

            posNextMin = transform.position;
            posNextMin.z += (halfAttackWidth * drawPercent);

            posNextMax = transform.position;
            posNextMax.z += (halfAttackWidth * drawPercent);
            //if (Mathf.Sign(transform.position.x) > 0)
            //{
            posNextMax.x += maxRange;
            //}
            //else
            //{
            //posNextMax.x += -maxRange;
            //}

            int a = 4 * i;
            int b = 4 * i + 1;
            int c = 4 * i + 2;
            int d = 4 * i + 3;

            vertices[a] = posCurrentMin;
            vertices[b] = posCurrentMax;
            vertices[c] = posNextMax;
            vertices[d] = posNextMin;

            Quaternion qangle = Quaternion.AngleAxis(angleLookAt - 90.0f, Vector3.up);

            vertices[a] -= transform.position;
            vertices[b] -= transform.position;
            vertices[c] -= transform.position;
            vertices[d] -= transform.position;

            vertices[a] = qangle * vertices[a];
            vertices[b] = qangle * vertices[b];
            vertices[c] = qangle * vertices[c];
            vertices[d] = qangle * vertices[d];

            vertices[a] += transform.position;
            vertices[b] += transform.position;
            vertices[c] += transform.position;
            vertices[d] += transform.position;

            triangles[6 * i] = a;
            triangles[6 * i + 1] = d;
            triangles[6 * i + 2] = c;
            triangles[6 * i + 3] = c;
            triangles[6 * i + 4] = b;
            triangles[6 * i + 5] = a;

            //angleCurrent += angleDelta;
            //angleNext += angleDelta;
        }
        //for (int i = 0; i < uv.Length; i++)
        //{
        //uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        //}

        //mesh.uv = uv;
        castTimeMesh.vertices = vertices;
        castTimeMesh.triangles = triangles;

		castTimeMesh.RecalculateBounds();

		Graphics.DrawMesh(castTimeMesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
    }

    float GetForwardAngle()
    {
        return 90 - Mathf.Rad2Deg * Mathf.Atan2(transform.forward.z, transform.forward.x);
    }
}
