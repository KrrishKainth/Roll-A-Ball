using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private Vector3 offset;
    private Vector3 cameraPosVector = new Vector3(0, 10, -10);
    private Vector3 cameraRotVector = new Vector3(45, 0, 0);

    private Vector3 screenCenter = new Vector3(1080 / 2, 607 / 2, 0);
    public float horzRotAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = cameraPosVector;
        transform.eulerAngles = cameraRotVector;
    }

    void Update()
    {
        // Mouse position: bottom left corner is (0, 0), top right is (1080, 607)
        Vector3 mousePos = Input.mousePosition - screenCenter;

        if (mousePos.x < -1080 / 2)
        {
            mousePos.x = -1080 / 2;
        }
        else if (mousePos.x > 1080 / 2) 
        {
            mousePos.x = 1080 / 2;
        }

        // Determine rotation angle based on mouse position
        horzRotAngle = Mathf.PI / 3 * -mousePos.x / (1080 / 2);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Rotate camera from default position based on mouse position
        float newX = cameraPosVector.x * Mathf.Cos(horzRotAngle) - cameraPosVector.z * Mathf.Sin(horzRotAngle);
        float newZ = cameraPosVector.x * Mathf.Sin(horzRotAngle) + cameraPosVector.z * Mathf.Cos(horzRotAngle);

        // Update camera position and rotation, making sure to follow player
        transform.position = new Vector3(newX + player.transform.position.x, transform.position.y, newZ + player.transform.position.z);
        transform.eulerAngles = new Vector3(cameraRotVector.x, -horzRotAngle * 180 / Mathf.PI, cameraRotVector.z);
    }
}
