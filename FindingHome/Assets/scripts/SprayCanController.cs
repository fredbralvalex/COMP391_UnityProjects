using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayCanController : MonoBehaviour {

    private float level = 10;
    private const float use = 1;

    public bool useSpray()
    {
        if (level <= 0)
        {
            //Destroy(this);
            //gameObject.SetActive(false);
            return false;
        }
        else if (level == 1)
        {
            level = level - use;
            return true;
        }
        else
        {
            level = level - use;
            return true;
        }
    }
}
