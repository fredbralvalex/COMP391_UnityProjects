using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EShipController : MonoBehaviour {

    //private float speed = 0.8f;
    private Vector2 direction;
    private EnemiesBlockController eShipController;
    public bool destroyed= false;

    public GameObject cannonLaser;
    public Vector3 initialPosition;

    EShipController clone;

    void Start()
    {
        eShipController = GameObject.Find("Enemies").GetComponent<EnemiesBlockController>();
        cannonLaser = GameObject.Find("efire");
        initialPosition = transform.position;
    }

    public void Respawn()
    {
        if (destroyed == true && clone != null)
        {
            EShipController respaw = Instantiate(clone, transform.position, clone.transform.rotation) as EShipController;
            respaw.transform.position = initialPosition;
        }
    }
    public void Clone()
    {
        clone = Instantiate(this, transform.position, this.transform.rotation) as EShipController;
        clone.transform.position = new Vector3(-9.72f, -1.58f, 0);
    }

    public void fire()
    {
        Rigidbody2D clone;
        Rigidbody2D projectile = cannonLaser.GetComponent<Rigidbody2D>();
        clone = Instantiate(projectile, projectile.transform.position, projectile.transform.rotation) as Rigidbody2D;
        clone.transform.position = GetComponent<Rigidbody2D>().transform.position;
        EnemyCannonLaserController controller = clone.GetComponent<EnemyCannonLaserController>();
        controller.fired = true;
    }

    void FixedUpdate()
    {
        /*
        time += Time.deltaTime;
        //Debug.Log("time :" + time);

        if (time > 1)
        {
            //Debug.Log("moving");
            moveUsingTransform();
            time = 0;
        }*/
    }

    /*
    public void moveUsingTransform()
    {
        
        if (touched == false)
        {
            if (stateDown)
            {
                Debug.Log("moving .... down");
                direction.y = Vector2.down.y;            
                stateDown = false;
                moveBlock();
            } else
            {
                //Debug.Log("touched " + touched);
                if (touched == false)
                {
                    if (stateRight)
                    {
                        //Debug.Log("moving .... right");
                        direction.x = Vector2.right.x;
                    } else
                    {
                        //Debug.Log("moving .... left");
                        direction.x = Vector2.left.x;
                    }
                    moveBlock();
                }

            }
        }
    }

    void willHit()
    {
        Vector2 pos = transform.position;
        //direction += new Vector2(direction.x * 0.45f, direction.y * 0.45f);
        Vector2 newDirection = direction * speed;
        RaycastHit2D hit = Physics2D.Linecast(pos + newDirection, pos);
        Debug.Log("hit " + hit);
        if (hit.collider != null)
        {
            Debug.Log("HIT :: " + hit.collider.name);
        }
        //return hit.collider.name == "pacdot" || (hit.collider == GetComponent<Collider2D>());
    }

    private void moveBlock()
    {
        willHit();
        //Debug.Log("moving .... all");
        Vector2 newDirection = direction * speed;
        //transform.position += (Vector3)newDirection;

        GameObject block = GameObject.Find("Enemies");
        block.transform.position += (Vector3)newDirection;

        //Debug.Log("Block moved");

    }
    */
    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        Debug.Log("on collision ship");
        Collider2D collider = collision.collider;
        if (collider.name == "BoundaryRight")
        {
            stateDown = true;
            stateRight = false;
            Debug.Log("HIT RIGHT");
            //moveUsingTransform();
        }
        if (collider.name == "BoundaryLeft")
        {
            stateDown = true;
            stateRight = true;
            Debug.Log("HIT LEFT");
            //moveUsingTransform();
        }
        touched = true;
        Debug.Log("touched " + touched);
        */
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("on Enter collider ship");

        if (collider.name == "BoundaryRight" || collider.name == "BoundaryLeft")
        {
            if (!eShipController.stateDown)
            {
                if (!eShipController.stateStop && collider.name == "BoundaryRight" && eShipController.stateRight)
                {
                    eShipController.stateDown = true;
                    eShipController.stateRight = false;
                    //Debug.Log("HIT RIGHT");
                }
                if (!eShipController.stateStop && collider.name == "BoundaryLeft" && !eShipController.stateRight)
                {
                    eShipController.stateDown = true;
                    eShipController.stateRight = true;
                    //Debug.Log("HIT LEFT");
                }
                eShipController.stateStop = true;
            }
        }
        else if (collider.name == "fire")
        {
            Destroy(gameObject);
            Debug.Log(" ship Fired");
        }
        else if (collider.name == "ground")
        {            
            //game over
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
       // Debug.Log("on exit collider ship");

        if (collider.name == "BoundaryRight" && !eShipController.stateRight)
        {             
           // Debug.Log("HIT RIGHT");
        }
        if (collider.name == "BoundaryLeft" && eShipController.stateRight)
        {
            //Debug.Log("HIT LEFT");
        }
        //eShipController.stateStop = false;
    }
    private void OnDestroy()
    {
        destroyed = true;
    }

}
