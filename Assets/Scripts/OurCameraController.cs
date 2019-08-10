using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurCameraController : MonoBehaviour
{
    // Where we the camera is focused. Where we are looking
    Transform focus;

    // How far away we want our camera to be from focused point
    float targetDistance;
    float maxDistance = 15.0f;

    // If we want our camera to do an initial look around/movement before we start
    bool battleReady = false;

    PlayerScript playerScript = null;

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance += Mathf.Clamp(Input.mouseScrollDelta.y * 0.2f, 2.0f, maxDistance);

        if (battleReady)
        {
            transform.LookAt(focus.position);
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
