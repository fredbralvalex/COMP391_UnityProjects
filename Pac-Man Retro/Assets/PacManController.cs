using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManController : MonoBehaviour {


    //Speed of PacMan and Phantoms
    public const float SPEED_CONSTANT = 1f;

    public enum Direction { Stoped, Up, Down, Left, Right };

    public float speed = 0.05f;

    private Rigidbody2D pacman;

    void Start()
    {
        pacman = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {
        //get move directions
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Direction direction = GetDirection(moveHorizontal, moveVertical);
        float horizontal = 0;
        float vertical = 0;
        float rotation = 0;
        if (direction == Direction.Down)
        {
            vertical = -1;
            rotation = 180;
        } else if (direction == Direction.Up)
        {
            vertical = 1;
            rotation = 0;
        }
        else if (direction == Direction.Left)
        {
            horizontal = -1;
            rotation = 90;
        }
        else if (direction == Direction.Right)
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

    Direction GetDirection(float moveHorizontal, float moveVertical)
    {
        if (moveHorizontal > 0)
        {
            return Direction.Right;
        }
        else if (moveHorizontal < 0)
        {
            return Direction.Left;
        }
        else if (moveVertical > 0)
        {
            return Direction.Up;
        }
        else if (moveVertical < 0)
        {
            return Direction.Down;
        } else
            return Direction.Stoped;
        {

        }
    }
}
