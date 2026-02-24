
using UnityEngine;  

public class MouseMovement : MonoBehaviour
{

    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;
    void Awake()
    {
        
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    void Start()
    {
        
        //lock cursor to make it invis
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Get mouse Inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity *Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity *Time.deltaTime;

        //Rotation around x axis 
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-75f,75f);

        //Rotation around Y axis
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation,yRotation,0f);
    }
}
