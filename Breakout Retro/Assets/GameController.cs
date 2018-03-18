using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private int lives = 3;
    public long points = 0;
    public long hiPoints = 0;
    public UnityEngine.UI.Text score;
    public UnityEngine.UI.Text hiScore;

    SpriteRenderer life1;
    SpriteRenderer life2;
    SpriteRenderer life3;

    private GameObject life;

    void Start () {
        life = GameObject.Find("life");
        //Debug.Log(life);
        StartLife();
        StartHiScore();
    }


    void Restart()
    {
        StartLife();
        StartHiScore();
    }

    void StartHiScore()
    {
        if (points > hiPoints)
        {
            hiPoints = points;
        }
        points = 0;
        SetHiScoreText();
        SetScoreText();
    }

    void StartLife()
    {
        SpriteRenderer liveVar = life.GetComponent<SpriteRenderer>();
        //Debug.Log(life);
        //Debug.Log(liveVar);
        life1 = Instantiate(liveVar, liveVar.transform.position, liveVar.transform.rotation) as SpriteRenderer;
        life1.transform.position = new Vector3(9.24f, 0.55f, 0);

        life2 = Instantiate(liveVar, liveVar.transform.position, liveVar.transform.rotation) as SpriteRenderer;
        life2.transform.position = new Vector3(10.14f, 0.55f, 0);

        life3 = Instantiate(liveVar, liveVar.transform.position, liveVar.transform.rotation) as SpriteRenderer;
        life3.transform.position = new Vector3(11.04f, 0.55f, 0);

    }

    void FixedUpdate()
    {
       
    }

    public void removeLife()
    {
        lives = lives - 1;
        if (lives == 2)
        {
            Destroy(life3.gameObject);
        }
        else if (lives == 1)
        {
            Destroy(life2.gameObject);
        }
        else if (lives == 0)
        {
            Destroy(life1.gameObject);
        }
        else
        {
            Restart();
        }
    }

    public void SetScoreText()
    {
        score.text = points.ToString();
    }

    void SetHiScoreText()
    {
        hiScore.text = hiPoints.ToString();
    }

}
