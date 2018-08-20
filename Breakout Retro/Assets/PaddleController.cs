using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {

    public const float SPEED_CONSTANT = 1f;    

    public float speed = 8f;
    public float distance = 0.11f;

    /*The ball*/
    private BallController ballController;
    private GameObject ball;

    public Rigidbody2D paddle;
    bool stateMovement = true;

    void Start()
    {
        ball = GameObject.Find("ball");
        ballController = ball.GetComponent<BallController>();

        paddle = GetComponent<Rigidbody2D>();
    }

    public void InitialPosition ()
    {
        transform.localPosition = new Vector3(0, -6.92f, 0);
        ballController.stateStoped = true;
    }


    void FixedUpdate()
    {
        moveUsingTransformStoping();
    }

    private void moveUsingTransformStoping()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.A) || moveHorizontal < 0)
        {
            moveStoping(Vector2.left);            
            if (ballController.stateStoped)
            {
                ballController.stateLeft = true;
                ballController.stateUp = true;
                ballController.stateStoped = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || moveHorizontal > 0)
        {
            moveStoping(Vector2.right);
            if (ballController.stateStoped)
            {
                ballController.stateLeft = false;
                ballController.stateUp= true;
                ballController.stateStoped = false;
            }

        }        
    }

    private void moveStoping(Vector2 direction)
    {
        Vector2 nextPosition = direction * speed * Time.deltaTime;
        if (stateMovement)
        {
            if (keyValid())
            {
                transform.localPosition += (Vector3)nextPosition;
            }
        } else
        {
            transform.localPosition -= (Vector3)nextPosition;
            stateMovement = true;
        }
    }

    private bool keyValid()
    {
        return !(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D));
    }

    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.name == "right wall")
        {
            System.Console.WriteLine("HIT R WALL");
        }
        else if (other.name == "left wall")
        {
            System.Console.WriteLine("HIT L WALL");
        }
            stateMovement = false;

    }

    private void OnTriggerExit(Collider other)
    {
        stateMovement = true;
    }
}
