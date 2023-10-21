using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{

    float cooldown = 3;
    int dropNum = 0;
    float lastDropTime;
    int numDropTiles = 18;

    // Start is called before the first frame update
    void Start()
    {
        lastDropTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastDropTime > cooldown && dropNum < numDropTiles)
        {
            Transform groundPiece = transform.GetChild(dropNum);
            groundPiece.GetComponent<GroundTileController>().startDrop();
            dropNum++;
            lastDropTime = Time.time;
        }        
    }

    public void respawn()
    {
        for(int i = 0; i < numDropTiles; i++)
        {
            if (transform.GetChild(i) == null)
            {
                Debug.Log("null" + i.ToString());
                continue;
            }
            transform.GetChild(i).GetComponent<GroundTileController>().respawn();
        }

        dropNum = 0;
        lastDropTime = Time.time;
    }
}
