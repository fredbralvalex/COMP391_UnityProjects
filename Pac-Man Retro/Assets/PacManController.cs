using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManController : MonoBehaviour {


    //Speed of PacMan and Phantoms
    public const float SPEED_CONSTANT = 1f;

    public enum Direction { Stoped, Up, Down, Left, Right };

    public float speed = 8f;

    private Rigidbody2D pacman;

    void Start()
    {
        pacman = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {
        //get move directions
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 direction = GetDirection(moveHorizontal, moveVertical);
        move(direction);

    }

    Vector2 GetDirection(float moveHorizontal, float moveVertical)
    {
        if (moveHorizontal > 0)
        {
            pacman.MoveRotation(-90);
            return Vector2.right;
        }
        else if (moveHorizontal < 0)
        {
            pacman.MoveRotation(90);
            return Vector2.left;
        }
        else if (moveVertical > 0)
        {
            pacman.MoveRotation(0);
            return Vector2.up;
        }
        else if (moveVertical < 0)
        {
            pacman.MoveRotation(180);
            return Vector2.down;
        } else
        {
            return Vector2.zero;
        }
    }

    private void move_(Vector2 direction)
    {

        float horizontal = 0;
        float vertical = 0;
        float rotation = 0;
        if (direction == Vector2.down)
        {
            vertical = -1;
            rotation = 180;
        }
        else if (direction == Vector2.up)
        {
            vertical = 1;
            rotation = 0;
        }
        else if (direction == Vector2.left)
        {
            horizontal = -1;
            rotation = 90;
        }
        else if (direction == Vector2.right)
        {
            horizontal = 1;
            rotation = -90;
        }

        Vector2 movement = new Vector2(pacman.position.x + horizontal * speed,
            pacman.position.y + vertical * speed);
        //pacman.AddForce(movement * speed * SPEED_CONSTANT);
        pacman.MovePosition(movement);
        pacman.MoveRotation(rotation);
        //transform.Translate(movement);

    }
    private void move(Vector2 direction)
    {
        Vector2 nextPosition = direction * speed * Time.deltaTime;
        transform.localPosition += (Vector3)nextPosition;

    }
}
