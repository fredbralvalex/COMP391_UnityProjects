using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManController : MonoBehaviour {


    //Speed of PacMan and Phantoms
    public const float SPEED_CONSTANT = 1f;

    public float speed = 1;

    private Rigidbody2D pacman;

    void Start()
    {
        pacman = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {
        //get move directions
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        pacman.AddForce(movement * speed * SPEED_CONSTANT);
    }
}
