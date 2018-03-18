using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

    static public float speed = 8f;
    public const float SPEED_CONSTANT = 1f;
    
    public float distance = 0.11f;

    public Rigidbody2D cannon;
    public GameObject cannonLaser;
    //stop when reaches the boundary
    bool stateMovement = true;


    private Vector3 initialCannonPosition;
        

       // Use this for initialization
    void Start () {
		cannon = GetComponent<Rigidbody2D>();
        cannonLaser = GameObject.Find("fire");
        initialCannonPosition = transform.position;
    }

    public void Restart()
    {
        transform.position = initialCannonPosition;
    }

    void FixedUpdate()
    {
        fire();
        move();
    }

    private void move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.A) || moveHorizontal < 0)
        {
            moveStoping(Vector2.left);
            
        }
        else if (Input.GetKeyDown(KeyCode.D) || moveHorizontal > 0)
        {
            moveStoping(Vector2.right);
           

        }
    }

    private void fire()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Rigidbody2D clone;
            Rigidbody2D projectile = cannonLaser.GetComponent<Rigidbody2D>();
            clone = Instantiate(projectile, projectile.transform.position, projectile.transform.rotation) as Rigidbody2D;
            clone.transform.position = cannon.transform.position;
            CannonLaserController controller = clone.GetComponent<CannonLaserController>();
            controller.fired = true;            
        }
                
    }

    private bool keyValid()
    {
        return !(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "BoundaryRight")
        {
            //System.Console.WriteLine("HIT Right");
        }
        else if (other.name == "BoundaryLeft")
        {
            //System.Console.WriteLine("HIT Left");
        }
        stateMovement = false;

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
        }
        else
        {
            transform.localPosition -= (Vector3)nextPosition;
            stateMovement = true;
        }
    }    

}
