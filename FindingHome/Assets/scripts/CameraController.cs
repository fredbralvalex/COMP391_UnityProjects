using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public PlyrMoveController playerController;

    private Vector3 offset;
    float varDown = 0.02f;
    float verticalVariation = 0;

    void Start()
    {
        //player = GameObject.Find("char");

        offset = transform.position - player.transform.position;
        //Debug.Log(offset);

        playerController = player.GetComponent<PlyrMoveController>();
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {        
        if (playerController.GetMovement() == PlyrMoveController.Movement.Down && playerController.MoveCameraDown())
        {
            verticalVariation = verticalVariation + varDown;
            if (verticalVariation >= 2)
            {
                verticalVariation = 2;
            }
        }
        else
        {
            verticalVariation = verticalVariation - varDown;
            if (verticalVariation <= 0)
            {
                verticalVariation = 0;
            }
            //transform.position = player.transform.position + offset;
        }
        transform.localPosition = new Vector3(player.transform.position.x, player.transform.localPosition.y - verticalVariation, player.transform.localPosition.z) + offset;
    }
}
