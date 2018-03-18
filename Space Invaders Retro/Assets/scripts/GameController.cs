using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private EnemiesBlockController eBlockController;    

    private GameObject shipBlocks;
    private GameObject cannon;
    private int lives = 3;

    private GameObject life;

    SpriteRenderer life1;
    SpriteRenderer life2;
    SpriteRenderer life3;

    public long points = 0;
    public long hiPoints = 0;
    public UnityEngine.UI.Text score;
    public UnityEngine.UI.Text hiScore;

    private Vector3 initialEnemyPosition;
    private Vector3 initialCannonPosition;

    //time in seconds
    private double time;
	void Start () {
        cannon = GameObject.Find("cannon");
        shipBlocks = GameObject.Find("Enemies");
        life = GameObject.Find("life");

        initialEnemyPosition = shipBlocks.transform.position;
        initialCannonPosition = cannon.transform.position;
        eBlockController = shipBlocks.GetComponent<EnemiesBlockController>();

        StartLife();
        StartHiScore();
    }


    void Restart()
    {

        cannon.transform.position = initialCannonPosition;
        shipBlocks.transform.position = initialEnemyPosition;

        StartLife();
        StartHiScore();
        RespawnEnemies();
    }

    void StartHiScore()
    {
        if (points > hiPoints)
        {
            hiPoints = points;
        }
        points = 0;
        SetHiScoreText();
    }

    void RespawnEnemies()
    {
        EShipController[] enemies05 = GameObject.FindObjectsOfType<EShipController>();
        foreach (EShipController ship in enemies05)
        {            
            ship.Respawn();
        }
    }

    void StartLife()
    {
        SpriteRenderer liveVar = life.GetComponent<SpriteRenderer>();

        life1 = Instantiate(liveVar, liveVar.transform.position, liveVar.transform.rotation) as SpriteRenderer;
        life1.transform.position = new Vector3(-6.53f, -6.53f, 0);

        life2 = Instantiate(liveVar, liveVar.transform.position, liveVar.transform.rotation) as SpriteRenderer;
        life2.transform.position = new Vector3(-5.03f, -6.53f, 0);

        life3 = Instantiate(liveVar, liveVar.transform.position, liveVar.transform.rotation) as SpriteRenderer;
        life3.transform.position = new Vector3(-3.53f, -6.53f, 0);

    }

    void FixedUpdate() {
        //System.Threading.Thread.Sleep(5000);
        //Debug.Log("FixedUpdate time :" + Time.deltaTime);

            time += Time.deltaTime;
            //Debug.Log("time :" + time);
        
            if (time > 1)
            {
                //Debug.Log("moving");
                eBlockController.moveUsingTransform();

                EShipController enemy = eBlockController.getOneEnemy();
                enemy.fire();
                time =  0;
            }
    }
    
    public void removeLife()
    {
        lives = lives - 1;
        if (lives == 2)
        {
            Destroy(life3.gameObject);
        } else if (lives == 1)
        {
            Destroy(life2.gameObject);
        }
        else if (lives == 0)
        {
            Destroy(life1.gameObject);
        } else
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
