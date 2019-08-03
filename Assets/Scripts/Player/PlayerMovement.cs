using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Public Variables
    public float speed = 6f;

    //Private Variables
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    void Awake()
    {
        //Initialise Variables. Use awake for ease of calling
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        //GetAxisRaw means no gradual speed increase, instant max speed
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //Move function
        Move(h, v);
        //Animating
        Animating(h, v);
        //Turning
        Turning();
        
    }

    void Move(float h, float v)
    {
        //X and Z movement means you cannot go up
        movement.Set(h, 0f, v);
        //Normalised for fair movement rules diagonally
        movement = movement.normalized * speed * Time.deltaTime;
        //Update movement
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        //Casts a ray from camera location to mouse position in the scene
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;
        //Cast a ray, if it hits something, return true
        if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
        {
            //Set the direction of the player towards the direction of the mouse
            Vector3 playerToMouse = floorHit.point - transform.position;
            //Shouldn't be needed with the floor raycast but a double check to make
            //Sure the player cannot turn upwards
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }

        //IF THIS BREAKS LATER, MAKE SURE THE CAMERA IS SET TO MAIN CAMERA

    }
    void Animating(float h, float v)
    {
        //Complicated logic leap in 3... 2... 1...
       
        bool walking = h != 0f || v != 0f;
        //Translation: Is H not equal to 0 or V not equal to 0? If either or both is true, return yes
        //if yes, we are walking

        
        
        anim.SetBool("IsWalking", walking);
        //Make Sure has Exit Time in the animation transition is ticked off
    }
}
