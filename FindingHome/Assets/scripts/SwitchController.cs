
using UnityEngine;

public class SwitchController : MonoBehaviour {

    Animator animator;
    public bool activated;
    public bool used = false;
    public bool sEnabled;
    SpriteRenderer spriteRender;
    

    void Start () {
        animator = gameObject.GetComponent<Animator>();
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
    }
	
	void FixedUpdate () {

	}

    public void setsEnabled(bool enable)
    {
        sEnabled = enable;
        Animator animation = animator.GetComponent<Animator>();
        if (enable)
        {            
            if (name == "switch_door" && !used)
            {
                animation.Play("deactive");
            }
        } else
        {
            animation.Play("disabled");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
    }

    public void activate()
    {
        if (sEnabled && !used && !activated)
        {
            if (animator == null)
            {
                animator = gameObject.GetComponent<Animator>();
            }
            Animator animation = animator.GetComponent<Animator>();
            if (name == "switch_door")
            {
                animation.Play("activating");
                //animation.Play("activating2");
                //animation.Play("activated");
                Destroy(GameObject.Find("door_level2"));
            } else
            {
                animation.Play("activating");
            }

            activated = true;
            used = true;
        }
    }

    public void deactivate()
    {
        if (sEnabled && !used && activated)
        {
            Animator animation = animator.GetComponent<Animator>();

            animation.Play("deactivating");
            activated = false;
            used = true;

            if(name == "switch_1")
            {
                GameObject.Find("Game").GetComponent<GameController>().setSavePoint(1);
            }
        }
    }
}
