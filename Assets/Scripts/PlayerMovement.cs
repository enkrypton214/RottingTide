
using System.Xml.Serialization;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller; 
    public float speed = 12f;
    public float gravity = -9.81f *2;

    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistacne=0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;
    private Vector3 lastPosition = new Vector3 (0,0,0);
    
    void Awake()
    {
        transform.rotation.x.Equals(0);
        transform.rotation.y.Equals(0);
        transform.rotation.z.Equals(0);
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistacne, groundMask);
        //Reset Velocity
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Get Input
        float x= Input.GetAxis("Horizontal");
        float z= Input.GetAxis("Vertical");

        Vector3 move= transform.right * x +transform.forward*z;

        controller.Move(move*speed*Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight*-2f*gravity);
        }

        velocity.y+=gravity *Time.deltaTime;

        controller.Move(velocity*Time.deltaTime);

        if (lastPosition!= gameObject.transform.position && isGrounded==true)
        {
            isMoving = true;
            //laterUse
        }
        else
        {
            isMoving=false;
            //laterUse
        }
    }
}
