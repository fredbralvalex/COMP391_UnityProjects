using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    private PaddleController paddleController;
    private GameObject paddle;

    /* Controls the ball movement   */
    private float speed = 4f;
    public bool stateStoped = true;
    public bool stateLeft = true;
    public bool stateUp = true;

    private Vector2 direction;
    // Use this for initialization
    void Start () {
        paddle = GameObject.Find("paddle");
        paddleController = paddle.GetComponent<PaddleController>();
    }    

    void FixedUpdate() {
        if (stateStoped)
        {
            transform.position = new Vector3 (paddle.transform.position.x, paddle.transform.position.y);
        } else
        {
            moveUsingTransform();
            move();
        }
    }

    private void moveUsingTransform()
    {
        if (stateLeft)
        {
            direction.x = Vector2.left.x;
        } else
        {
            direction.x = Vector2.right.x;
        }

        if (stateUp)
        {
            direction.y = Vector2.up.y;
        } else
        {
            direction.y = Vector2.down.y;
        }
    }

    private void move()
    {
        Vector2 newDirection = direction * speed * Time.deltaTime;                
        transform.position += (Vector3)newDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "paddle")
        {         
            effectByHitCollision(collision);
            System.Console.WriteLine("HIT PADDLE");
            stateUp = true;
        }
         
        if (collision.gameObject.tag == "Brick")
        {


            System.Console.WriteLine("HIT BRICK");
            Destroy(collision.gameObject.gameObject);
        }        
    }
    void effectByHitCollision(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = collider.bounds.center;

        bool right = contactPoint.x >= center.x;
		bool left = contactPoint.x <= center.x;

        if (stateLeft && right)
        {
            System.Console.Write("Effect Right");
            stateLeft = false;
            return;
        }
        if (!stateLeft && left)
        {
            System.Console.Write("Effect Left");
            stateLeft = true;
            return;
        }       
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "right wall")
        {
            stateLeft = true;
            System.Console.WriteLine("HIT R WALL");
        }
        else if (collider.name == "left wall")
        {
            stateLeft = false;
            System.Console.WriteLine("HIT L WALL");
        }
        else if (collider.name == "top wall")
        {
            stateUp = false;
            System.Console.WriteLine("HIT T WALL");
        }
        else if (collider.name == "botton wall")
        {
            stateUp = true;
            System.Console.WriteLine("HIT B WALL");
            //TODO Must lose one chance
            paddleController.InitialPosition();
        }
    }
}
