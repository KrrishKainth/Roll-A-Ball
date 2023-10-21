using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTileController : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 startingPos;
    private bool active;
    private float dropSpeed;
    private float dropTime;
    void Start()
    {
        startingPos = transform.position;
        active = true;
        dropSpeed = 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!active)
        {
            drop();
            // After tile has been dropping for a certain time, hide and disable it
            if (Time.time - dropTime > 1)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // Resets tile (ex. after player dies)
    public void respawn()
    {
        gameObject.SetActive(true);
        transform.position = startingPos;
        active =  true;
    }

    // Called by GroundController when tile should be dropped
    public void startDrop()
    {
        dropTime = Time.time;
        active = false;
    }

    // Updates position of tile while dropping
    private void drop()
    {
        transform.position -= new Vector3(0, dropSpeed * Time.deltaTime, 0);
    }

}
