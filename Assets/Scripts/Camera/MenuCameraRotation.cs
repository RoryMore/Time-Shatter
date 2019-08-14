using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraRotation : MonoBehaviour
{
    public Transform focus;

    public float xSpeed;

    [Tooltip("How far away the Camera will be from the focused point")]
    public float distance;

    public float positionLerpSpeed;

    float x;
    float y;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = focus.position.y * 1.725f;
    }

    // Update is called once per frame
    void Update()
    {
        x += xSpeed * Time.unscaledDeltaTime;
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        //y = ClampAngle(y, yMinAngle, yMaxAngle);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + focus.position;

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, positionLerpSpeed);
        transform.position = Vector3.Slerp(transform.position, position, positionLerpSpeed);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
