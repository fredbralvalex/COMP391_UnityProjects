using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonLaserController : MonoBehaviour
{
    GameController gameController;
    public bool fired = false;
    void Start()
    {
        GameObject ground = GameObject.Find("ground");
        gameController = ground.GetComponent<GameController>();
    }

    void FixedUpdate()
    {
        if (fired)
        {
            Vector2 nextPosition = Vector2.up * 2 * GameController.laserSpeed * Time.deltaTime;
            transform.localPosition += (Vector3)nextPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log(" on trigger enter cannon Fired " + collider.name);
        if (collider.tag == "eShip01" || collider.tag == "eShip02" || collider.tag == "eShip03")
        {
            if (collider.tag == "eShip01") {
                gameController.points += 10;

            } else if(collider.tag == "eShip02")
            {
                gameController.points += 20;

            }
            else if (collider.tag == "eShip03")
            {
                gameController.points += 30;
            }
            else if (collider.tag == "eShip04")
            {
                System.Random r = new System.Random();
                int rInt = r.Next(0, 3);
                if (rInt == 0)
                {
                    gameController.points += 300;
                } else
                {
                    gameController.points += 50 * rInt;
                }
            }
            EShipController controller = collider.gameObject.GetComponent<EShipController>();
            //controller.Clone();
            Destroy(collider.gameObject);
            controller.destroyed = true;
            //Debug.Log(" laser Fired " + collider.name);
            //gameController.removeLife();
            Destroy(gameObject);
            gameController.SetScoreText();
        }
        else if (collider.name == "ground")
        {
            Destroy(gameObject);
        }
        else if (collider.tag == "bunker")
        {
            Destroy(gameObject);
        }
        else if (collider.name == "efire(Clone)")
        {
            //Debug.Log("trigger enter fire");
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        Collider2D collider = collision.collider;
        Debug.Log(" on collide cannon Fired " + collider.name);
        if (collider.name == "efire")
        {
            Debug.Log("on collision efire");
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
        */
    }
}
