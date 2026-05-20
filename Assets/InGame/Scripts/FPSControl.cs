using UnityEngine;
using static UnityEditor.PlayerSettings;

public class FPSControl : MonoBehaviour
{
    private CharacterController chara;


    [Range(0, 30)] public float walkSpd = 5;
    [Range(0, 10)] public float sensitivity = 2;
    [Range(0, 10)]  public float jumpHeight;

    float vertR;
    public float lookLimit = 80f;
    private Camera cam;
    public GameObject cam2;
    private float defWalk;
    private float runSpd;
    private Vector3 currMovement;
    private float g = 9.81f;

    private Vector3 hitPoint;
    public ParticleSystem impact;
    [Range(10,30)] public int particleC = 20;
    private Transform location;

    public float pickUpRange = 2;
    public Transform holdPoint;
    private Item heldItem;
    public float throwForce = 5;

    public static int bCount;

    public GameObject holster; // Used to give it a location 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        location = GetComponent<Transform>();
        runSpd = walkSpd * 2;
        defWalk = walkSpd;
        chara = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        MouseLook();
        Jumping();

        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            walkSpd = runSpd; 
        }

        else 
        {
            walkSpd = defWalk;
        }

        if (heldItem != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                heldItem.Throw(throwForce, cam.transform.forward);
                heldItem = null;
            }
        }

        if (ObjectInFocus() != null)
        {

            float distanceToObject = Vector3.Distance(holster.transform.position, ObjectInFocus().transform.position);

            if (Input.GetMouseButtonDown(0))
            {
                if (ObjectInFocus().gameObject.tag == "Enemy")
                {
                    Debug.Log("Enemy Found");
                    impact.transform.position = hitPoint;
                    impact.Emit(particleC);
                    DestroyObject(ObjectInFocus().gameObject);
                    bCount++;
                }

            }

            if (distanceToObject <= pickUpRange && ObjectInFocus().GetComponent<Item>() != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    heldItem = ObjectInFocus().GetComponent<Item>();
                    heldItem.PickUp(holster.transform, holdPoint.position);
                }
            }

            else if(distanceToObject <= pickUpRange)
            {

                if (Input.GetMouseButtonDown(1))
                {
                    Instantiate(cam2);
                    cam2.GetComponent<Cam>();
                    cam2.GetComponentInChildren<Camera>();
                    //cam2.Throw(throwForce, cam.transform.forward);
                }



            }

        }


    }


    void Movement()
    {
        //Initialize/ Declare variaables to get UNITY's axis
        float verInput = Input.GetAxis("Vertical");
        float horInput = Input.GetAxis("Horizontal");

        //Makes a variable that allows us to walk;
        float vSpeed = verInput * walkSpd;
        float horSpeed = horInput * walkSpd;

        Vector3 horizontalMov = new Vector3(horSpeed, 0, vSpeed);
        horizontalMov = transform.rotation * horizontalMov;
        currMovement.x = horizontalMov.x;
        currMovement.z = horizontalMov.z;

        chara.Move(currMovement * Time.deltaTime);

        
    }

     void MouseLook()
    {
        float mouseXR = Input.GetAxis("Mouse X") * sensitivity;

        transform.Rotate(0,mouseXR,0);
        vertR -= Input.GetAxis("Mouse Y") * sensitivity;
        vertR = Mathf.Clamp(vertR, -lookLimit , lookLimit);
        cam.transform.localRotation = Quaternion.Euler(vertR, 0, 0);
    }

    void Jumping()
    {
        if (chara.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currMovement.y = jumpHeight;
                Debug.Log("jump");
            }
            
        }

        else
        {
            currMovement.y -= g * Time.deltaTime;
            Debug.Log("falling");
        }
    }

    public GameObject ObjectInFocus()
    {
        GameObject result = null;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            result = hit.transform.gameObject;
            hitPoint = hit.point;
            
        }

        
        return result;

    }

    public int GetBodyCount()
    {
        return bCount;
    }


}
