using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesBlockController : MonoBehaviour {


    private float speed = 0.8f;

    public bool stateRight = true;
    public bool stateDown = false;
    public bool stateStop = false;

    private Vector2 direction;
    private Vector3 initialPosition;

    void Start () {
        
    }

    void FixedUpdate() {
		
	}

    public EShipController getOneEnemy()
    {
        EShipController [] enemies05 = GameObject.FindObjectsOfType<EShipController>();       
        
        return chooseEnemy(enemies05);
    }
    public EShipController chooseEnemy(EShipController[] enemies)
    {
        System.Random r = new System.Random();
        int rInt = r.Next(0, enemies.Length);        
        if (enemies[rInt].destroyed)
        {
            return chooseEnemy(enemies);
        }
        return enemies[rInt];
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("on collision block");
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("on collider block");
    }

    public void moveUsingTransform()
    {
        if (stateStop)
        {
            if (stateDown)
            {
                //Debug.Log("moving .... down");
                direction.y = Vector2.down.y;
                direction.x = 0;
                Vector2 newDirection = direction * speed;
                transform.position += (Vector3)newDirection;
                stateDown = false;
                stateStop = false;
            }
        }
        else
        {
            direction.y = 0;
            if (stateRight)
            {                
                direction.x = Vector2.right.x;
            }
            else
            {
                direction.x = Vector2.left.x;
            }
            moveBlock();

        }

        
    }  

    private void moveBlock()
    {
        Vector2 newDirection = direction * speed;
        transform.position += (Vector3)newDirection;

    }
}
