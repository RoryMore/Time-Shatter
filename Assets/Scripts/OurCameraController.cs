using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurCameraController : MonoBehaviour
{
    // Where we the camera is focused. Where we are looking
    Transform focus = null;

    public float rotateSpeed = 5.0f;

    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public const float yMinAngle = 30.0f;
    public const float yMaxAngle = 85.0f;

    // How far away we want our camera to be from focused point
    float targetDistance = 5.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 15.0f;

    public float positionLerpSpeed = 0.2f;

    // If we want our camera to do an initial look around/movement before we start
    bool battleReady = false;

    public bool invertY = true;

    PlayerScript playerScript = null;

    Vector3 cameraOffset;

    float x = 0.0f;
    float y = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;
    }

    private void Awake()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        if (playerScript != null)
        {
            focus = playerScript.transform;
            cameraOffset = transform.position - focus.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = Mathf.Clamp(targetDistance - Input.mouseScrollDelta.y*5, minDistance, maxDistance);

        if (focus != null)
        {
            // Hide and lock cursor when right mouse button pressed
            if (Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Unlock and show cursor when right mouse button released
            if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * Time.unscaledDeltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.unscaledDeltaTime * (invertY ? 1.0f : -1.0f);

                

                

                if (Physics.Linecast(focus.position, transform.position, out RaycastHit hit))
                {
                    targetDistance -= hit.distance;
                }

            }
            y = ClampAngle(y, yMinAngle, yMaxAngle);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -targetDistance);
            Vector3 position = rotation * negDistance + focus.position;

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.2f);
            transform.position = Vector3.Slerp(transform.position, position, 0.2f);
        }
    }

    public void SetFocus(Transform t)
    {
        focus = t;
    }

    public Transform GetFocus()
    {
        return focus;
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
