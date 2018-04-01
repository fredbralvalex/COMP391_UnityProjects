using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.scripts;
using TMPro;

public class GameController : MonoBehaviour {

    private EnemiesBlockController eBlockController;    

    private GameObject shipBlocks;
    private GameObject cannon;
    private int lives = 0;

    private GameObject life;

    SpriteRenderer life1;
    SpriteRenderer life2;
    SpriteRenderer life3;

    public long points = 0;
        
    public TextMeshProUGUI score;
    public TextMeshProUGUI hiScore;


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
    }

    void StartHiScore()
    {
        if (points > GameState.getInstance().hiPoints)
        {
            GameState.getInstance().hiPoints = points;
        }
        points = 0;
        SetHiScoreText();
    }

    void StartLife()
    {
        SpriteRenderer liveVar = life.GetComponent<SpriteRenderer>();
        if (lives > 0)
        {
            life1 = Instantiate(liveVar, liveVar.transform.position, liveVar.transform.rotation) as SpriteRenderer;
            life1.transform.position = new Vector3(-6.53f, -6.53f, 0);
        }

        if (lives > 1)
        {
            life2 = Instantiate(liveVar, liveVar.transform.position, liveVar.transform.rotation) as SpriteRenderer;
            life2.transform.position = new Vector3(-5.03f, -6.53f, 0);
        }

        if (lives > 2)
        {
            life3 = Instantiate(liveVar, liveVar.transform.position, liveVar.transform.rotation) as SpriteRenderer;
            life3.transform.position = new Vector3(-3.53f, -6.53f, 0);
        }

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
                if (enemy == null)//end Game
                {
                    SceneManager.LoadScene(2);
            }
                else
                { 
                    enemy.fire();
                }
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
            //Restart();
            if (points > GameState.getInstance().hiPoints)
            {
                GameState.getInstance().hiPoints = points;
            }
            SceneManager.LoadScene(2);
        }
    }

    public void SetScoreText()
    {
        score.text = points.ToString().PadLeft(4, '0');
    }

    void SetHiScoreText()
    {
        hiScore.text = GameState.getInstance().hiPoints.ToString().PadLeft(4, '0');
    }

}
