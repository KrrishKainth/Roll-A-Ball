using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private Vector3 startPos;
    public int count;
    private float movementX;
    private float movementY;
    public float forceScale;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI livesText;

    private bool inJump;
    [SerializeField] GameObject bullet;
    public float speedLimit;
    public CameraController camera;
    private bool alive;
    private float deathTime;
    public int lives;
    public GroundController ground;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        inJump = true;
        count = 0;
        alive = true;
        deathTime = 0;
        lives = 3;
        SetCountText();
        SetLivesText();
    }

    private void FixedUpdate()
    {
        if (alive) 
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

            // If player's y-position is at or below -1, player dies
            if (transform.position.y <= -1)
            {
                alive = false;
                deathTime = Time.time;
                lives--;
                SetLivesText();

                if (lives == 0)
                {
                    // Go to lose screen
                    updateStats();
                    SceneManager.LoadScene("LoseScreen");
                }
            }
        }
        else
        {
            // Freeze for 2 seconds, then respawn player
            if (Time.time - deathTime < 2)
            {
               rb.velocity = new Vector3(0, 0, 0);
               rb.angularVelocity = new Vector3(0, 0, 0);
            }
            else
            {
                alive = true;
                transform.position = startPos;
                ground.respawn();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }


        if (other.gameObject.CompareTag("FinishLine"))
        {
            // Go to win screen
            updateStats();
            SceneManager.LoadScene("WinScreen");
        }
    }

    void OnTriggerStay(Collider other)
    {
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

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
    }

    void updateStats()
    {
        PlayerStatistics.Instance.lives = lives;
        PlayerStatistics.Instance.count = count;
        PlayerStatistics.Instance.score = lives * 1000 + count * 50;
    }    
}
