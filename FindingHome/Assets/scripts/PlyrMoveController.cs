using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyrMoveController : MonoBehaviour {
    
    bool stateMovement = true;
    Direction stateMove = Direction.Stoped;
    public enum Direction { Stoped, Left, Right };
    bool stateJump = false;
    Animator animator;
    SpriteRenderer sprite;
    //time in seconds to wait actions
    private double time;
    float maxJumpHigh;

    void Start () {
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        //waiting the player reach the ground
        if (!stateJump)
        {
            move();
        } else
        {
            //maxJumpHigh = transform.localPosition.y + 2*sprite.bounds.size.y;
            /*
            Debug.Log("max" + maxJumpHigh);
            Debug.Log("actual" + transform.localPosition.y);
            Debug.Log("size" + sprite.bounds.size.y);
            */
            performeJump();
        }
    }

    private void performeJump()
    {
        
        Vector2 nextPositionHorizontal;
        if (stateMove == Direction.Left)
        {
            nextPositionHorizontal = Vector2.left * GameController.SPEED_CONSTANT * Time.deltaTime;         
        }
        else
        {
            nextPositionHorizontal = Vector2.right* GameController.SPEED_CONSTANT * Time.deltaTime;
        }


        Vector2 nextPositionVertical;
        if (maxJumpHigh > transform.localPosition.y)
        {
            nextPositionVertical = Vector2.up * GameController.SPEED_JUMP_CONSTANT * Time.deltaTime;            
        } else
        {
            //falling
            nextPositionVertical = Vector2.down * GameController.SPEED_FALL_CONSTANT* Time.deltaTime;
            Animator animation = animator.GetComponent<Animator>();
            if (stateMove == Direction.Left)
            {
                animation.Play("jump_fall_left");
            }
            else
            {
                animation.Play("jump_fall_right");
            }
        }
        transform.localPosition += (Vector3)nextPositionVertical;
        if (stateMove != Direction.Stoped)
        {
            transform.localPosition += (Vector3)nextPositionHorizontal;
        }
    }

    private void move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.A) || moveHorizontal < 0)
        {
            //move left
            stateMove = Direction.Left;
            Animator animation = animator.GetComponent<Animator>();
            animation.Play("walk_left");
            moveTransform(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || moveHorizontal > 0)
        {
            //move right
            stateMove = Direction.Right;
            Animator animation = animator.GetComponent<Animator>();
            animation.Play("walk_right");
            moveTransform(Vector2.right);
        }
        else
        {
            stateMove = Direction.Stoped;
            Animator animation = animator.GetComponent<Animator>();
            animation.Play("idle");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //Jump
            stateJump = true;
            Animator animation = animator.GetComponent<Animator>();
            maxJumpHigh = transform.localPosition.y + 3*sprite.bounds.size.y;
            if (stateMove == Direction.Left)
            {
                animation.Play("jump_left");
            } else
            {
                animation.Play("jump_right");
            }
        }
        
    }

    private void moveTransform(Vector2 direction)
    {
        Vector2 nextPosition = direction * GameController.SPEED_CONSTANT * Time.deltaTime;
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

    private bool keyValid()
    {
        return !(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("on Trigger");
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        
        if (collision.gameObject.name == "somethingelse")
        {
        }
        else if (collision.gameObject.tag == "ground")
        {
            //Debug.Log("on collision");
            if (stateJump == true)
            {
                //Debug.Log("on collision - reach ground");
                Animator animation = animator.GetComponent<Animator>();
                if (stateMove == Direction.Left)
                {
                    animation.Play("reach_ground_left");
                }
                else
                {
                    animation.Play("reach_ground_right");
                }
                stateJump = false;
            }
        }
    }
}
