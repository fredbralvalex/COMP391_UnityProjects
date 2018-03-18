using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonLaserController : MonoBehaviour {
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
            Vector2 nextPosition = Vector2.down * 2 * CannonController.speed * Time.deltaTime;
            transform.localPosition += (Vector3)nextPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log(" cannon Fired " + collider.name);
        if (collider.name == "cannon")
        {
            //Destroy(collider.gameObject);
            //Debug.Log(" cannon Fired " + collider.name);
            collider.gameObject.GetComponent<CannonController>().Restart();
            gameController.removeLife();
            Destroy(gameObject);
        }
        else if (collider.name == "ground")
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("on collision laser");
    }

}