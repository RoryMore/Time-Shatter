using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurCameraController : MonoBehaviour
{
    // Where we the camera is focused. Where we are looking
    public Transform focus = null;

    public float xSpeed = 60.0f;
    public float ySpeed = 60.0f;

    public float yMinAngle = 30.0f;
    public float yMaxAngle = 85.0f;

    // How far away we want our camera to be from focused point
    float targetDistance = 25.0f;
    public float minDistance = 10.0f;
    public float maxDistance = 100.0f;

    public float positionLerpSpeed = 0.2f;

    // If we want our camera to do an initial look around/movement before we start
    [Tooltip("A series of Transforms that the Camera will move between, before the start of battle. \n Not setup yet. Might be removed - don't know yet")]
    public Transform[] entryPositions;

    public bool invertY = true;

    [Header("Death Camera Zoom Settings")]
    public float deathZoomMinDistance = 10.0f;

    public float deathZoomStartLerpSpeed = 0.4f;
    public float deathZoomEndLerpSpeed = 0.02f;
    float deathZoomLerpSpeed;
    [Tooltip("The number of seconds it will take for the camera position lerp speeds to go \n from 'deathZoomStartLerpSpeed' to 'deathZoomEndLerpSpeed'")]
    public float deathZoomStartToEndTime = 2.0f;
    

    bool battleReady = true;

    

    PlayerScript playerScript = null;

    float x = 0.0f;
    float y = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = 0.0f;
        y = angles.y;

        if (entryPositions.Length > 0)
        {
            battleReady = false;
        }
        else
        {
            battleReady = true;
        }

        deathZoomLerpSpeed = deathZoomStartLerpSpeed;
    }

    private void Awake()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        if (playerScript != null)
        {
            if (focus == null)
            {
                focus = playerScript.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (battleReady)
        {
            if (playerScript.isDead)
            {
                focus = playerScript.transform;

                deathZoomLerpSpeed = Mathf.Clamp( Mathf.Lerp(deathZoomLerpSpeed, deathZoomEndLerpSpeed, Time.unscaledDeltaTime / deathZoomStartToEndTime), deathZoomEndLerpSpeed, deathZoomStartLerpSpeed);

                targetDistance = Mathf.Lerp(targetDistance, deathZoomMinDistance, deathZoomLerpSpeed);

                x += (xSpeed * (deathZoomLerpSpeed * 5.0f)) * Time.unscaledDeltaTime;
                y = Mathf.Lerp(y, yMinAngle - 5.0f, deathZoomLerpSpeed);
            }
            else
            {
                targetDistance = Mathf.Clamp(targetDistance - Input.mouseScrollDelta.y * 1.2f, minDistance, maxDistance);
            }
            //Debug.Log("TargetDistance: " + targetDistance);
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
                if (!playerScript.isDead)
                {
                    if (Input.GetMouseButton(1))
                    {
                        x += Input.GetAxis("Mouse X") * xSpeed * Time.unscaledDeltaTime;
                        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.unscaledDeltaTime * (invertY ? 1.0f : -1.0f);

                        if (Physics.Linecast(focus.position, transform.position, out RaycastHit hit))
                        {
                            targetDistance -= hit.distance;
                        }

                    }
                }
                y = ClampAngle(y, yMinAngle, yMaxAngle);
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -targetDistance);
                Vector3 position = rotation * negDistance + focus.position;

                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, positionLerpSpeed);
                transform.position = Vector3.Slerp(transform.position, position, positionLerpSpeed);
            }
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
