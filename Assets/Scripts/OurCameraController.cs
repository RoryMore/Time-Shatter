using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurCameraController : MonoBehaviour
{
    // Where we the camera is focused. Where we are looking
    Transform focus = null;

    public float rotateSpeed = 5.0f;

    public const float yMinAngle = 10.0f;
    public const float yMaxAngle = 85.0f;

    // How far away we want our camera to be from focused point
    float targetDistance = 5.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 15.0f;

    public float positionLerpSpeed = 0.2f;

    // If we want our camera to do an initial look around/movement before we start
    bool battleReady = false;

    public bool invertY = false;

    PlayerScript playerScript = null;

    Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {

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
        targetDistance += Mathf.Clamp(Input.mouseScrollDelta.y * 0.2f, minDistance, maxDistance);

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

        if (focus != null)
        {

            if (Input.GetMouseButton(1))
            {
                /*float mouseYMove = Input.GetAxis("Mouse Y");
                Quaternion horizCamTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateSpeed, Vector3.up);
                Quaternion vertCamTurnAngle = Quaternion.AngleAxis(mouseYMove * rotateSpeed, (invertY ? Vector3.left : Vector3.right));

                cameraOffset = horizCamTurnAngle * cameraOffset;

                Vector3 dir = transform.position - focus.position;
                float angle = Vector3.Angle(transform.position, Vector3.up);
                //Debug.Log("Angle between Camera & Player: " + angle);

                cameraOffset = vertCamTurnAngle * cameraOffset;*/
            }

            //Vector3 newPos = focus.position + cameraOffset;
            //transform.position = Vector3.Slerp(transform.position, newPos, positionLerpSpeed);

            transform.LookAt(focus);
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
}
