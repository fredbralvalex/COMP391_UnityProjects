using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour {

    private GameObject ball;
    private BallController ballController;

    void Start () {
        ball = GameObject.Find("ball");
        ballController = ball.GetComponent<BallController>();   
    }
	
	
	void FixedUpdate() {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        float RectWidth = GetComponent<Collider2D>().bounds.size.x;
        float RectHeight = GetComponent<Collider2D>().bounds.size.y;
        //float circleRad = collider.bounds.size.x;

        if (collision.contacts.Length < 1)
            return;

        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = collider.bounds.center;

        if (contactPoint.y > center.y && //checks that circle is on top of rectangle
            (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
        {
            
            System.Console.WriteLine("HIT TOP");
            ballController.stateUp = false;
        }
        else if (contactPoint.y < center.y &&
            (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
        {
           
            System.Console.WriteLine("HIT BOTTOM");
            ballController.stateUp = true;
        }
        else if (contactPoint.x > center.x &&
            (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2))
        {
            
            System.Console.WriteLine("HIT RIGHT");
            ballController.stateLeft = true;
        }
        else if (contactPoint.x < center.x &&
            (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2))
        {
            
            System.Console.WriteLine("HIT LEFT");
            ballController.stateLeft = false;
        }
        
    }

}
