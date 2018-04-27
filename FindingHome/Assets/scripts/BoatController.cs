using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

    public bool sail;
    public float initialPosition;
    public float deltaPosition;

    WaterController w;
    GameObject water;

    void Start()
    {
        initialPosition = transform.position.x;
        water = GameObject.Find("water_02");
        w = water.GetComponent<WaterController>();
    }

    void FixedUpdate()
    {
        deltaPosition = initialPosition - transform.position.x;
        initialPosition = transform.position.x;
        sailing();

        transform.localPosition = new Vector3(transform.position.x, water.transform.position.y - 1, transform.localPosition.z);
    }

    public float getDeltaPosition()
    {
        return deltaPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);

        if (collision.collider.name == "player")
        {
            GameObject.Find("Game").GetComponent<GameController>().setSavePoint(2);
            sail = true;
        }
    }

    private void sailing()
    {
        if(sail)
        {
            Debug.Log("SAIL");
            transform.localPosition += (Vector3)Vector2.right * GameController.SPEED_SAILING_CONSTANT * Time.deltaTime;
            //transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.localPosition.z);
        }
    }

    public void goToLeve1()
    {
        transform.localPosition = new Vector3(transform.position.x, 2.3f, transform.localPosition.z);
    }

    public void goToLevel2()
    {
        transform.localPosition = new Vector3(transform.position.x, -3.5f, transform.localPosition.z);
    }
}
