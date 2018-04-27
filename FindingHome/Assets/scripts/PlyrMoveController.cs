using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyrMoveController : MonoBehaviour {

    public const float FIRE_OFFSET = 1;
    public enum Movement { Walk, Idle, Jump, Down, Ladder, Run, Slide, Fall, ReachGround, Action, FireSpray};
    Movement state = Movement.Idle;
    bool stateMovement = false;
    bool stateRun = false;
    public enum Direction { Left, Right };
    public enum LadderDirection { Up, Down, None };
    Direction lastStateMove = Direction.Right;
    Direction stateMove = Direction.Right;
    LadderDirection ladderDirection = LadderDirection.None;
    bool stateStop = true;
    bool touchingGround = false;
    bool touchingBoat = false;

    LadderPosition touchingLadder = LadderPosition.None;
    public enum LadderPosition { None, Top, Botton, Middle};
    public List<LadderPosition> positionsTouches;

    public List<GameObject> items;

    Animator animator;
    SpriteRenderer sprite;
    Rigidbody2D character;

    //time in seconds to wait actions
    private double time;
    private double timeToUseLadder = 0.5;
    private double timeToDown = 0.5;
    private double timeToReachGround = 0.3;
    private double timeToAction = 0.3;
    private double timeToFireSpray = 0.3;
    //private double timeToJump = 8;
    private double timeToSlide = 1;
    private int countLadderUse = 0;

    float maxJumpHigh;
    bool moveCameraDown = false;


    GameObject switchEntered = null;

    public Movement GetMovement()
    {
        return state;
    }

    public GameObject GetSprayCan ()
    {
        foreach (GameObject item in items)
        {
            if (item.tag == "spray_can")
            {
                return item;
            }
        }
        return null;
    }

    public GameObject GetKey()
    {
        foreach (GameObject item in items)
        {
            //in case of more keys and lockers check the names
            if (item.tag == "key")
            {
                return item;
            }
        }
        return null;
    }

    public GameObject GetLife()
    {
        foreach (GameObject item in items)
        {
            //in case of more keys and lockers check the names
            if (item.tag == "life")
            {
                return item;
            }
        }
        return null;
    }

    public int GetCount(string name)
    {
        int count = 0;
        foreach (GameObject item in items)
        {
            
            if (item.tag == name)
            {
                count++;
            }
        }
        return count;
    }

    public bool hasSprayCan()
    {        
        return GetSprayCan() != null;
    }

    public bool MoveCameraDown()
    {
        return moveCameraDown;
    }

    void Start () {
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        character = gameObject.GetComponent<Rigidbody2D>();
        positionsTouches = new List<LadderPosition>();
        items = new List<GameObject>();
        state = Movement.Fall;
    }

    void FixedUpdate()
    {
        
        sail();
        run();
        time += Time.deltaTime;
        if (switchEntered != null)
        {
            performActionSwitch(switchEntered);
        }

        if (positionsTouches.Count > 0)//(state == Movement.Ladder)
        {
            
            //in the ladder the character can move only at top or botton
            if (positionsTouches.Contains(LadderPosition.Botton) || positionsTouches.Contains(LadderPosition.Top))
            {
                move();
            }
            performeUpDownLadder();
            return;
        }

        if (state == Movement.Slide)
        {
            //check for jumping;
            calcMaxJumpHigh();
            jump();
            //Debug.Log("Sliding");
            if (time > timeToSlide)
            {
                //Debug.Log("Stop Sliding");
                state = Movement.Fall;
            } else
            {
                //Debug.Log("Still Sliding");
                time = 0;
                slide();
            }
            return;
        }

        if (validateTimeToWait())
        {
            return;
        }
        moveCameraDown = true;
        time = 0;
        if (state == Movement.FireSpray)
        {
            fireSpray();
        }

        if (!touchingGround && state != Movement.Jump && state != Movement.Ladder && state != Movement.Slide)
        {
            //Debug.Log("Falling");
            state = Movement.Fall;
            performeJumpFall();
            return;
        }        

        //waiting the player reach the ground
        if (state != Movement.Jump && state != Movement.Fall)
        {
            if(state == Movement.Slide)
            {
                slide();
            }
            else if (state == Movement.Down)
            {
                down();
            }
            else
            {
                moveCameraDown = false;
                down();
                move();
                jump();
                fireSpray();
                
            }            
        }
        else
        {
            /*Debug.Log("max" + maxJumpHigh);
            Debug.Log("actual" + transform.localPosition.y);
            Debug.Log("size" + sprite.bounds.size.y);*/
            performeJumpFall();
        }        
    }

    void sail()
    {
        if (touchingBoat)
        {
            GameObject boat = GameObject.Find("boat");
            BoatController controller = boat.GetComponent<BoatController>();
                transform.localPosition = 
                    new Vector3(transform.position.x + Mathf.Abs(controller.getDeltaPosition()), transform.localPosition.y, transform.localPosition.z);
        }
    }

    bool validateTimeToWait()
    {
        bool wait = false;
        if (state == Movement.Down)
        {
            wait =  time <= timeToDown;
        } else if (state == Movement.ReachGround)
        {
            //Debug.Log("wait reach ground");
            wait = time <= timeToReachGround;
        }
        else if (state == Movement.FireSpray)
        {
            //Debug.Log("wait use spray");
            wait = time <= timeToFireSpray;
        }

        return wait;       
    }


    private void performeUpDownLadder()
    {


        if (time <= timeToUseLadder)
        {
            return;
        }        
        time = 0;

        Animator animation = animator.GetComponent<Animator>();
        if (!positionsTouches.Contains(LadderPosition.Botton)
            && !positionsTouches.Contains(LadderPosition.Top))
        {
            // middle
            if (countLadderUse > 0)
            {
                if (countLadderUse % 2 == 0)
                {
                    animation.Play("ladder_middle_left");
                } else
                {
                    animation.Play("ladder_middle_right"); 
                }

            }
            countLadderUse++;
        }
        else if (!positionsTouches.Contains(LadderPosition.Middle)
          && !positionsTouches.Contains(LadderPosition.Top))
        {
            // botton
            animation.Play("ladder_begin");
            countLadderUse = 0;
        }
        else if (!positionsTouches.Contains(LadderPosition.Middle)
        && !positionsTouches.Contains(LadderPosition.Botton))
        {
            //top
            animation.Play("ladder_end");
            countLadderUse = 0;
        }



        Vector2 nextPositionVertical;
        if (Input.GetKey(GameController.UP))
        {
            ladderDirection = LadderDirection.Up;
            nextPositionVertical = Vector2.up * GameController.SPEED_LADDER * Time.deltaTime;
            
        }
        else if (Input.GetKey(GameController.DOWN))
        {
            ladderDirection = LadderDirection.Down;
            nextPositionVertical = Vector2.down * GameController.SPEED_LADDER * Time.deltaTime;
        }
        else
        {
            ladderDirection = LadderDirection.None;
            nextPositionVertical = Vector2.zero;
        }
        transform.localPosition += (Vector3)nextPositionVertical;
    }

    private void performeJumpFall()
    {
        float varRun = 1;

        if (stateRun)
        {
            varRun = GameController.RUN_JUMP_CONSTANT;
        }

        Vector2 nextPositionHorizontal;
        if (Input.GetKey(GameController.LEFT))
        {
            stateMove = Direction.Left;
            stateStop = false;
            nextPositionHorizontal = Vector2.left * GameController.SPEED_CONSTANT * varRun * Time.deltaTime;
        }
        else if (Input.GetKey(GameController.RIGHT))
        {
            stateMove = Direction.Right;
            stateStop = false;
            nextPositionHorizontal = Vector2.right* GameController.SPEED_CONSTANT * varRun * Time.deltaTime;            
        } else
        {
            nextPositionHorizontal = Vector2.zero;
            stateStop = true;
        }

        Vector2 nextPositionVertical = Vector2.zero;               

        if (state == Movement.Jump)
        {
            if (maxJumpHigh >= sprite.transform.localPosition.y)
            {
                //going high       
                //state = Movement.Jump;
                nextPositionVertical = Vector2.up * GameController.SPEED_JUMP_CONSTANT * Time.deltaTime;            
            }else
            {
                state = Movement.Fall;                
            }
        }

        if (state == Movement.Fall)
        {
            nextPositionVertical = Vector2.up *2 * Time.deltaTime;
            fall();
        }

        transform.localPosition += (Vector3)nextPositionVertical;
        if (!stateStop)
        {
            transform.localPosition += (Vector3)nextPositionHorizontal;
        }
    }    

    private void run ()
    {
        if (Input.GetKey(GameController.STEALTH))
        {
            stateRun = false;
            if (touchingGround)
            {
                calcMaxJumpHigh();
            }
        } else if (Input.GetKeyUp(GameController.STEALTH))
        {
            stateRun = true;
        }
        else
        {
            stateRun = true;
        }
    }

    private void move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(GameController.LEFT) || moveHorizontal < 0)
        {
            lastStateMove = stateMove;
            //move left
            stateMove = Direction.Left;
            Animator animation = animator.GetComponent<Animator>();   
            if (stateRun)
            {
                animation.Play("run_left");
            } else
            {
                animation.Play("walk_left");
            }
            moveTransform(Vector2.left);
        }
        else if (Input.GetKeyDown(GameController.RIGHT) || moveHorizontal > 0)
        {
            lastStateMove = stateMove;
            //move right
            stateMove = Direction.Right;
            Animator animation = animator.GetComponent<Animator>();
            if (stateRun)
            {
                animation.Play("run_right");
            }
            else
            {
                animation.Play("walk_right");
            }
            moveTransform(Vector2.right);
        }
        else
        {
            //stateMove = Direction.Stoped;
            stateStop = true;
            if (state != Movement.Ladder)
            {
                Animator animation = animator.GetComponent<Animator>();
                if (lastStateMove == Direction.Left)
                {
                    animation.Play("idle_left");
                } else //if (lastStateMove == Direction.Right)
                {
                    animation.Play("idle_right");
                }

            }
        }       
    }

    private void slide()
    {
        run();
        if (state == Movement.Slide)
        {
            float varRun = 1;
            /*if (stateRun)
            {
                varRun = GameController.SPEED_RUN_CONSTANT;
            }*/
            Animator animation = animator.GetComponent<Animator>();
            if (stateMove == Direction.Left)
            {
                animation.Play("slide_left");
                transform.localPosition += (Vector3)Vector2.left * GameController.SPEED_SLIDE_CONSTANT * varRun * Time.deltaTime;

            }
            else
            {
                animation.Play("slide_right");
                transform.localPosition += (Vector3)Vector2.right * GameController.SPEED_SLIDE_CONSTANT * varRun * Time.deltaTime;
            }
            transform.localPosition += (Vector3)Vector2.down * GameController.SPEED_FALL_CONSTANT * Time.deltaTime; ;
        }
    }


    private void fall()
    {
        if (state == Movement.Fall)
        {
            Animator animation = animator.GetComponent<Animator>();
            if (stateMove == Direction.Left)
            {
                animation.Play("fall_left");
            }
            else
            {
                animation.Play("fall_right");
            }
        }
    }

    private void jump()
    {
        if (Input.GetKeyDown(GameController.JUMP))
        {
            run();
            //Jump
            state = Movement.Jump;
            Animator animation = animator.GetComponent<Animator>();            
            if (stateMove == Direction.Left)
            {
                animation.Play("jump_left");
            }
            else
            {
                animation.Play("jump_right");
            }
        }
    }


    private void fireSpray()
    {
        GameObject sprayCan = GetSprayCan();
        float fireOffset = 0;
        if (Input.GetKeyDown(KeyCode.K) && sprayCan != null)
        {
            SprayCanController controller = sprayCan.GetComponent<SprayCanController>();
            //use spray
            
            bool hasContent = controller.useSpray();
            Animator animation = animator.GetComponent<Animator>();
            if (hasContent)
            {
                state = Movement.FireSpray;                
                if (stateMove == Direction.Left)
                {
                    //fireOffset = -FIRE_OFFSET;
                    animation.Play("fire_spray_left");
                }
                else
                {
                    //fireOffset = FIRE_OFFSET;
                    animation.Play("fire_spray_right");
                }
            } else
            {
                state = Movement.Idle;
                if (stateMove == Direction.Left)
                {
                    animation.Play("idle_left");
                }
                else
                {
                    animation.Play("idle_right");
                }
                items.Remove(sprayCan);
                Destroy(sprayCan);
            }
        }
        transform.localPosition = new Vector3(transform.transform.position.x + fireOffset, transform.transform.localPosition.y, transform.transform.localPosition.z);
    }

    private void performActionSwitch(GameObject switchObj)
    {
        SwitchController controller = switchObj.GetComponent<SwitchController>();
        if (Input.GetKeyDown(KeyCode.K))
        {            
                //Debug.Log("ACTION Switch");
            if (switchObj.name == "switch_1" || switchObj.name == "switch_2" || switchObj.name == "switch_3")
            {
                controller.deactivate();
            } else
            {
                controller.activate();
            }   
        }
    }


    private void down()
    {
        if (Input.GetKey(GameController.DOWN))
        {
            //Down
            //lastStateMove = stateMove;
            Animator animation = animator.GetComponent<Animator>();
            if (lastStateMove == Direction.Left)
            {
                animation.Play("down_left");
            }
            else
            {
                animation.Play("down_right");
            }
            state = Movement.Down;
            //move camera a little down
            //move head collision offset a little down
            return;
        }
            
        state = Movement.Idle;
    }

    private void moveTransform(Vector2 direction)
    {
        float var = 1;
        if (stateRun)
        {
            var = GameController.SPEED_RUN_CONSTANT;
        }
            Vector2 nextPosition = direction * var * GameController.SPEED_CONSTANT * Time.deltaTime;
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
        return true;// !(Input.GetKeyUp(GameController.LEFT) || Input.GetKeyUp(GameController.LEFT));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "water")
        {
            Debug.Log("enter Water");
            GameObject.Find("Game").GetComponent<GameController>().lostLife();
        } else if (collider.gameObject.name == "door_end")
        {
            //End Level
            GameObject.Find("Game").GetComponent<GameController>().endGame();
        }

        //Add more items
        if (collider.gameObject.tag == "coin" || collider.gameObject.tag == "diamond")
        {
            items.Add(collider.gameObject);
            collider.gameObject.SetActive(false);
        }

        if (collider.gameObject.tag == "spray_can")
        {
            items.Add(collider.gameObject);
            collider.gameObject.SetActive(false);

        }
        else if (collider.gameObject.tag == "key")
        {
            items.Add(collider.gameObject);
            collider.gameObject.SetActive(false);
        }
        else if (collider.gameObject.tag == "locker")
        {
            GameObject key = GetKey();
            //If more than one check the names
            //can open the locker
            if (key != null)
            {                                
                Destroy(collider.gameObject);
                items.Remove(key);
                Destroy(key);
                Destroy(GameObject.Find("door_01"));
                Destroy(GameObject.Find("dark_01"));
            }
        } else if (collider.gameObject.tag == "switch")
        {
            //performActionSwitch(collider.gameObject);
            //Debug.Log("enter Switch");
            switchEntered = collider.gameObject;
        }

        if (collider.gameObject.name == "stair")
        {
            //touchingGround = true;
            Debug.Log("stair");
            //touchingLadder = LadderPosition.None;
            state = Movement.Ladder;
            character.gravityScale = 0;
            transform.localPosition = new Vector3(collider.gameObject.transform.position.x, transform.localPosition.y, transform.localPosition.z);
        }
        
        if (collider.gameObject.tag == "upstair")
        {
            //touchingGround = true;
            Debug.Log("up");
            touchingLadder = LadderPosition.Botton;
            state = Movement.Ladder;
            character.gravityScale = 0;
            positionsTouches.Add(LadderPosition.Botton);
        }
        if (collider.gameObject.tag == "downstair")
        {
            //touchingGround = true;
            Debug.Log("down");
            touchingLadder = LadderPosition.Top;
            state = Movement.Ladder;
            character.gravityScale = 0;
            positionsTouches.Add(LadderPosition.Top);
        }
        if (collider.gameObject.tag == "middlestair")
        {
            //touchingGround = true;
            Debug.Log("middle");
            touchingLadder = LadderPosition.Middle;
            state = Movement.Ladder;
            character.gravityScale = 0;
            positionsTouches.Add(LadderPosition.Middle);
        }

    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "switch")
        {
            Debug.Log("stay Switch");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
         if (collision.gameObject.tag == "switch")
        {
            //performActionSwitch(collider.gameObject);
            //Debug.Log("exit Switch");
            switchEntered = null;
        }


        if (collision.gameObject.tag == "upstair")
        {
            Debug.Log("out up");
            //touchingLadder = LadderPosition.None;
            positionsTouches.Remove(LadderPosition.Botton);
            //rigidbody2D.gravityScale = 1;
            //state = Movement.Walk;
        }

        if (collision.gameObject.tag == "downstair")
        {
            Debug.Log("out down");
            //touchingLadder = LadderPosition.None;
            positionsTouches.Remove(LadderPosition.Top);
            //rigidbody2D.gravityScale = 1;
            //state = Movement.Walk;
        }

        if (collision.gameObject.tag == "middlestair")
        {
            Debug.Log("out  middle");
            //touchingLadder = LadderPosition.None;
            positionsTouches.Remove(LadderPosition.Middle);
        }

        if (positionsTouches.Count == 0)
        {
            //Debug.Log("no touch");
            touchingLadder = LadderPosition.None;
            character.gravityScale = 1;
            state = Movement.Idle;
            //countLadderUse = 0;

        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {        
        if (collision.gameObject.name == "somethingelse")
        {
        }
        else if (collision.gameObject.tag == "ground")
        {
//            Debug.Log("not touching the ground");
            touchingGround = false;
        }
        else if (collision.gameObject.tag == "boat")
        {
            //            Debug.Log("not touching the ground");
            touchingGround = false;
            touchingBoat = false;
        }
        else if (collision.gameObject.tag == "slider_right")
        {
            //state = Movement.Slide;
            calcMaxJumpHigh();
            //stateMove = Direction.Right;
        }
        else if (collision.gameObject.tag == "slider_left")
        {
            //state = Movement.Slide;
            calcMaxJumpHigh();
            //stateMove = Direction.Left;
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("on collision stay");
        if (collision.gameObject.name == "somethingelse")
        {
        }
        else if (collision.gameObject.tag == "ground")
        {
            touchingGround = true;
        }
        else if (collision.gameObject.tag == "boat")
        {
            touchingGround = true;
            touchingBoat = true;
            //boat.transform.position.x;
            //state = Movement.Sail;
        }
        else if (collision.gameObject.tag == "slider_left" || collision.gameObject.tag == "slider_right")
        {
            touchingGround = true;
        }
        //touchingGround = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.collider.name);

        if (collision.gameObject.name == "somethingelse")
        {
        }
        else if (collision.gameObject.tag == "wall")
        {
            state = Movement.Fall;
        }
        else if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "boat")
        {
            if (collision.gameObject.name == "ground_saveState_4")
            {
                GameObject.Find("Game").GetComponent<GameController>().setSavePoint(4);
            } else if (collision.gameObject.name == "ground_saveState_3")
            {
                GameObject.Find("Game").GetComponent<GameController>().setSavePoint(3);
            }
            else if (collision.gameObject.name == "ground_saveState_2")
            {                
                GameObject.Find("Game").GetComponent<GameController>().setSavePoint(2);
            }
            else if (collision.gameObject.name == "ground_saveState_1")
            {
                GameObject.Find("Game").GetComponent<GameController>().setSavePoint(1);
            }

            if (hitHead(collision))
            {
                state = Movement.Fall;
                return;
            }

            touchingGround = true;
            if (collision.gameObject.tag == "boat")
            {
                touchingBoat = true;
            }
            //Debug.Log("on collision");
            if (state == Movement.Jump || state == Movement.Fall || state == Movement.Slide)
            {
                //state = Movement.Down;
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
                state = Movement.ReachGround;

            }

            calcMaxJumpHigh();

        }
        else if (collision.gameObject.tag == "slider_right")
        {
            state = Movement.Slide;
            calcMaxJumpHigh();
            stateMove = Direction.Right;
        }
        else if (collision.gameObject.tag == "slider_left")
        {
            state = Movement.Slide;
            calcMaxJumpHigh();
            stateMove = Direction.Left;
        }
    }

    private bool hitHead(Collision2D collision)
    { 
        Collider2D colliderGround = collision.collider;
        Collider2D colliderChar = GetComponent<CircleCollider2D>();
        float RectWidth = colliderGround.bounds.size.x;
        float RectHeight = colliderGround.bounds.size.y;

        ContactPoint2D[] contact = new ContactPoint2D[1];

        colliderChar.GetContacts(contact);
        if (contact[0].collider == colliderGround)
        {
            //Debug.Log("HIT HEAD");
            return true;
        }
        return false;
        /*
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = colliderChar.bounds.center;

        if (contactPoint.y > center.y &&
            (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
        {

            System.Console.WriteLine("HIT TOP");            
        }
        else if (contactPoint.y < center.y &&
           (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
        {

            System.Console.WriteLine("HIT BOTTOM");
        }*/
    }

    private void calcMaxJumpHigh()
    {
        float varRun = 1;
        float varSlide = 1;
        if (stateRun)
        {
            varRun = GameController.RUN_JUMP_CONSTANT;
        }

        if (state == Movement.Slide)
        {
            varSlide = GameController.SPEED_SLIDE_CONSTANT;
        }

        maxJumpHigh = transform.localPosition.y + sprite.bounds.size.y * varRun * varSlide;// * 2.415f; 
        // 2.6535f; //3.204646f + transform.localPosition.y;//2.6535f;// sprite.transform.localPosition.y + 2 * sprite.bounds.size.y;
        /*
        Debug.Log("max jump high: " + maxJumpHigh);
        Debug.Log("actual position: " + transform.localPosition.y);
        Debug.Log("size: " + sprite.bounds.size.y);
        Debug.Log("differ: " + (maxJumpHigh - transform.localPosition.y));
        Debug.Log("----------------------------");            
        Debug.Log("actual position: " + transform.localPosition.y);
        Debug.Log("size: " + sprite.bounds.size.y);
        Debug.Log("position + size: " + (transform.localPosition.y + sprite.bounds.size.y * 2.415));
        */
    }
}
