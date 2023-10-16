using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private int count;
    public int totalPickUps;
    private float movementX;
    private float movementY;
    public float forceScale;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private bool inJump = true;
    [SerializeField] GameObject bullet;
    public float speedLimit;
    public CameraController camera;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Rotate movement vector to match camera angle
        float rotMovementX = movementX * Mathf.Cos(camera.horzRotAngle) - movementY * Mathf.Sin(camera.horzRotAngle);
        float rotMovementY = movementX * Mathf.Sin(camera.horzRotAngle) + movementY * Mathf.Cos(camera.horzRotAngle);
        
        Vector3 movement = new Vector3(rotMovementX, 0.0f, rotMovementY);
        rb.AddForce(forceScale * movement);

        // If total speed in xz directions exceeds speed limit, 
        // scale xz velocity down to speed limit
        float xzSpeed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
        if (xzSpeed > speedLimit)
        {
            rb.velocity = new Vector3(rb.velocity.x * speedLimit / xzSpeed,
                                      rb.velocity.y,
                                      rb.velocity.z * speedLimit / xzSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            if (count == totalPickUps)
            {
                winTextObject.SetActive(true);
            }
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            inJump = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            inJump = true;
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    void OnJump(InputValue jumpVal)
    {
        
        if (jumpVal.Get() != null && !inJump)
        {
            if (jumpVal.Get<float>() == 0)  // Tap
            {
                rb.velocity = new Vector3(rb.velocity.x, 6, rb.velocity.z);
            }
            else if (jumpVal.Get<float>() == 1)  // Hold
            {
                rb.velocity = new Vector3(rb.velocity.x, 8, rb.velocity.z);
            }
        }
    }

    // void OnFire()
    // {
    //     Quaternion q = transform.rotation;
    //     q.eulerAngles = new Vector3(90, 0, 0);
    //     Instantiate(bullet, transform.position, q);
    // }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    
}
