using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

    float varDown = GameController.WATER_SPEED_CONSTANT;
    float verticalVariation = 0;
    private float level;

    // Use this for initialization
    void Start () {
        level = transform.position.y;
    }

	// Update is called once per frame
	void FixedUpdate () {
        
        if (GameController.usingGameAction)
        {
            if (level > transform.position.y)
            {
                verticalVariation = verticalVariation + varDown;
                if (verticalVariation >= level)
                {
                    verticalVariation = level;
                }            
            } else if (level < transform.position.y)
            {
                verticalVariation = verticalVariation - varDown;
                if (verticalVariation <= level)
                {
                    verticalVariation = level;
                }
            }

            //Debug.Log("SOFT");
        } else
        {
           //transform.localPosition = new Vector3(transform.position.x, level, transform.localPosition.z);
            verticalVariation = level;
            //Debug.Log("NOT SOFT");
        }

        if (!(level == verticalVariation && level == transform.position.x))
        {
           transform.localPosition = new Vector3(transform.position.x, verticalVariation, transform.localPosition.z);
        }
        

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            Debug.Log("enter Water");            
        }
    }

    public void goToLevel0()
    {
        level = -26;
        //transform.localPosition = new Vector3(transform.position.x, -6, transform.localPosition.z);
    }

    public void goToLevel1()
    {
        level = -6;
        //transform.localPosition = new Vector3(transform.position.x, -6, transform.localPosition.z);
    }

    public void goToLevel2()
    {
        level = 4.7f;
        //transform.localPosition = new Vector3(transform.position.x, 4.7f, transform.localPosition.z);
    }

    public void goToLevel3()
    {
        level = 13.6f;
        //transform.localPosition = new Vector3(transform.position.x, 13.6f, transform.localPosition.z);
    }

    public void goToLevel4()
    {
        level = 3.2f;
        //transform.localPosition = new Vector3(transform.position.x, 3.2f, transform.localPosition.z);
    }

    public void goToLevel4Level1()
    {
        level = -11f;
        //transform.localPosition = new Vector3(transform.position.x, -11f, transform.localPosition.z);
    }

    public void goToLevel5()
    {
        level = -2.5f;
        //transform.localPosition = new Vector3(transform.position.x, -2.5f, transform.localPosition.z);
    }
}
