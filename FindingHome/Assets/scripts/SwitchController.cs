
using UnityEngine;

public class SwitchController : MonoBehaviour {

    //public bool used = false;

    public bool activated;
    private bool sEnabled;
    //SpriteRenderer spriteRender;
    GameController gameController;


    void Start () {
        //spriteRender = gameObject.GetComponent<SpriteRenderer>();
        gameController = GameObject.Find("Game").GetComponent<GameController>();
    }
	
	void FixedUpdate () {

	}

    private Animator GetAnimator()
    {
        return gameObject.GetComponent<Animator>();
    }

    public void ActionSwitch ()
    {
        if (sEnabled)
        {
            if (activated)
            {
                deactivate();
            } else
            {
                activate();
            }
            Debug.Log("Enabled");
            gameController.DoActionWaterLevel();
        }
        Debug.Log("Not Enabled");
    }

    public void setsEnabled(bool enable)
    {
        sEnabled = enable;
        if (sEnabled)
        {            
            if (name == "switch_door")
            {
                //Sprite sprite = Resources.Load<Sprite>("switch_1");
                //spriteRender.sprite = sprite;
                PlayAnimation("deactivated");
            } else
            {
                PlayAnimation("activated");
            }
        } else
        {
            //
            //Sprite sprite = Resources.Load<Sprite>("switch_disabled");
            //spriteRender.sprite = sprite;
            PlayAnimation("disabled");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
    }

    public void activate()
    {
        if (name == "switch_door")
        {
            PlayAnimation("activating");
            Destroy(GameObject.Find("door_level2"));
        } else
        {
            PlayAnimation("activating");
        }
        activated = true;

        //used = true;
    }

    public void deactivate()
    {
        
        if (name == "switch_door")
        {
            PlayAnimation("deactivating");
        } else
        {
            PlayAnimation("deactivating");
        }
        activated = false;

        //used = true;

    }

    private void PlayAnimation (string name)
    {
        if (GameController.usingGameAction)
        {
            Animator animation = GetAnimator();
            animation.Play(name);
        }
    }
}
