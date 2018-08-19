using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject createFire()
    {        
        GameObject clone;
        //GameObject projectile = life.GetComponent<Rigidbody2D>();
        clone = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        return clone;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "fire")
        {
            Debug.Log("fired");
        }
    }
}
