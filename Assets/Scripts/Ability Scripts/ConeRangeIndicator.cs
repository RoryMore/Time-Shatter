using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeRangeIndicator : MonoBehaviour
{
    //[Tooltip("The 'Draw Quality' of the Indicator. Higher values draw more vertices giving higher quality shapes if necessary at the cost of performance if the number of vertices is a lot")]
    // Smoother circle with lower?higher? quality value
    public int quality = 15;

    Mesh mesh;
    Mesh castTimeMesh;
    public Material indicatorMaterial;

    //public float angle;
    //public float minRange;
    //public float maxRange;

    Vector3[] normals;
    Vector2[] uv;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.vertices = new Vector3[4 * quality];
        mesh.triangles = new int[3 * 2 * quality];

        castTimeMesh = new Mesh();
        castTimeMesh.vertices = new Vector3[4 * quality];
        castTimeMesh.triangles = new int[3 * 2 * quality];

        normals = new Vector3[4 * quality];
        uv = new Vector2[4 * quality];

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
        //for (int i = 0; i < uv.Length; i++)
        //{
            //uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        //}

        //mesh.uv = uv;
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
    }

    public void DrawCastTimeIndicator(float angle, float minRange, float drawDistance)
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
            posCurrentMax = transform.position + sphereCurrent * drawDistance;

            posNextMin = transform.position + sphereNext * minRange;
            posNextMax = transform.position + sphereNext * drawDistance;

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

        castTimeMesh.vertices = vertices;
        castTimeMesh.triangles = triangles;
        //castTimeMesh.uv = uv;
        //castTimeMesh.normals = normals;

        Graphics.DrawMesh(castTimeMesh, Vector3.zero, Quaternion.identity, indicatorMaterial, 0);
    }

    float GetForwardAngle()
    {
        return 90 - Mathf.Rad2Deg * Mathf.Atan2(transform.forward.z, transform.forward.x);
    }

    public void Init(float angle, float minRange, float maxRange)
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

            //uv[a] = posCurrentMin;
            //uv[b] = posCurrentMax;
            //uv[c] = posNextMax;
            //uv[d] = posNextMin;

            triangles[6 * i] = a;
            triangles[6 * i + 1] = b;
            triangles[6 * i + 2] = c;
            triangles[6 * i + 3] = c;
            triangles[6 * i + 4] = d;
            triangles[6 * i + 5] = a;

            angleCurrent += angleDelta;
            angleNext += angleDelta;
        }

        /*
        float x1 = maxRange * Mathf.Cos(0);
        float y1 = maxRange * Mathf.Sin(0);
        
        for (int i = 0; i < uv.Length; i++)
        {
            float x2 = maxRange * Mathf.Cos((i + angleDelta) * Mathf.Deg2Rad);
            float y2 = maxRange * Mathf.Sin((i + angleDelta) * Mathf.Deg2Rad);

            //uv[i] = new Vector2(vertices[i].x / (angle * 0.1f), vertices[i].z / (angle * 0.1f));
            uv[i] = new Vector2((x2 + maxRange) / 2 * maxRange, (y2 + maxRange) / 2 * maxRange);

            uv[0] = new Vector2(0.5f, 0.5f);
            uv[1] = new Vector2((x1 + maxRange) / 2 * maxRange, (y1 + maxRange) / 2 * maxRange);
        }*/

        //mesh.uv = uv;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
