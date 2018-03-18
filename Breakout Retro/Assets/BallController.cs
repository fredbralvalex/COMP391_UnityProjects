using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    private PaddleController paddleController;
    private GameController gameController;
    private GameObject paddle;
    private GameObject game;

    /* Controls the ball movement   */
    private float speed = 4f;
    private float var = 1;
    public bool stateStoped = true;
    public bool stateLeft = true;
    public bool stateUp = true;

    int redHits = 0;
    int brownHits = 0;
    int greenHits = 0;
    int yellowHits = 0;


    private Vector2 direction;
    // Use this for initialization
    void Start () {
        paddle = GameObject.Find("paddle");
        game = GameObject.Find("Canvas");
        paddleController = paddle.GetComponent<PaddleController>();
        gameController = game.GetComponent<GameController>();

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
        Vector2 newDirection = direction * speed * Time.deltaTime * var;                
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
            string nameSprit = collision.gameObject.GetComponent<SpriteRenderer>().name;
            //Debug.Log(collision.gameObject.GetComponent<SpriteRenderer>());

            //Debug.Log(nameSprit);
            if (nameSprit.Contains("yellow"))
            {
                gameController.points += 1;
                yellowHits += 1;
                //Debug.Log("yellow");

            }
            else if (nameSprit.Contains("green"))
            {
                gameController.points += 3;
                greenHits += 1;
                //Debug.Log("green");
            } else if (nameSprit.Contains("brown"))
            {
                gameController.points += 5;
                brownHits += 1;
                //Debug.Log("brown");
            } else if (nameSprit.Contains("red"))
            {
                gameController.points += 7;
                yellowHits += 1;
                //Debug.Log("red");
            }
            gameController.SetScoreText();
            
            Destroy(collision.gameObject.gameObject);

            int sumHits = yellowHits + greenHits + brownHits + redHits;
            if (sumHits== 4 || sumHits == 12 || brownHits==1 || redHits==1)
            {
                increaseSpeed();
            }
           
        }        
    }

    //after four hits, after twelve hits, and after making contact with the orange and red rows
    void increaseSpeed()
    {
        var += 0.2f;
    }
    void shrinkSize()
    {

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
            shrinkSize();
        }
        else if (collider.name == "botton wall")
        {
            stateUp = true;
            System.Console.WriteLine("HIT B WALL");
            //TODO Must lose one chance
            //shrink to half
            paddleController.InitialPosition();
            gameController.removeLife();
        }
    }
}
