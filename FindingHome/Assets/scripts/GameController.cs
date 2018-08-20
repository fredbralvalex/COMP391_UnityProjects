using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour {

    public const float WATER_SPEED_CONSTANT = 0.4f;
    public const float SPEED_CONSTANT = 2f;
    public const float SPEED_RUN_CONSTANT = 3f;
    public const float SPEED_LADDER = 0.2f;
    public const float SPEED_SLIDE_CONSTANT = 6f;
    public const float SPEED_JUMP_CONSTANT = 10f;
    public const float SPEED_FALL_CONSTANT = 16f;
    public const float RUN_JUMP_CONSTANT = 2f;

    public const float SPEED_SAILING_CONSTANT = 0.8f;
    //TODO
    public const KeyCode FIRE = KeyCode.K;
    public const KeyCode UP = KeyCode.W;

    public const KeyCode JUMP = KeyCode.J;
    public const KeyCode DOWN = KeyCode.S;
    public const KeyCode LEFT = KeyCode.A;
    public const KeyCode RIGHT = KeyCode.D;
    public const KeyCode STEALTH = KeyCode.L;
    public const KeyCode ACTION = KeyCode.K;

    public List<GameObject> items;

    SwitchController s1;
    SwitchController s2;
    SwitchController s3;
    SwitchController s4;

    SwitchController sd;

    WaterController w0;
    WaterController w1;
    WaterController w2;

    BoatController bc;
    PlyrMoveController pc;

    CameraController cc;

    GameObject switch01;
    GameObject switch02;
    GameObject switch03;
    GameObject switch04;

    GameObject switchDoor;

    GameObject water00;
    GameObject water01;
    GameObject water02;

    GameObject boat;

    GameObject player;
    GameObject mainCamera;

    GameObject gameElements;
    GameObject menuposition;

    private Vector3 offset;

    public int savePoint = 0;

    Vector3 cameraOffset = new Vector3(3.8f, 1.7f, -31.3f);


    public TextMeshProUGUI counterCoins;
    public TextMeshProUGUI counterDiamonds;

    public static bool usingGameAction = false;

    public void setSavePoint(int sp)
    {
        savePoint = sp;
        items = pc.items;
    }

    void Start () {
        switch01 = GameObject.Find("switch_1");
        switch02 = GameObject.Find("switch_2");
        switch03 = GameObject.Find("switch_3");
        switchDoor = GameObject.Find("switch_door");
        switch04 = GameObject.Find("switch_4");


        water00 = GameObject.Find("water_00");
        water01 = GameObject.Find("water_01");
        water02 = GameObject.Find("water_02");

        boat = GameObject.Find("boat");
        player = GameObject.Find("player");
        mainCamera = GameObject.Find("Main Camera");

        offset = transform.position - mainCamera.transform.position;

        s1 = switch01.GetComponent<SwitchController>();
        s2 = switch02.GetComponent<SwitchController>();
        s3 = switch03.GetComponent<SwitchController>();
        s4 = switch04.GetComponent<SwitchController>();

        sd = switchDoor.GetComponent<SwitchController>();

        w0 = water00.GetComponent<WaterController>();
        w1 = water01.GetComponent<WaterController>();
        w2 = water02.GetComponent<WaterController>();

        bc = boat.GetComponent<BoatController>();
        pc = player.GetComponent<PlyrMoveController>();
        cc = mainCamera.GetComponent<CameraController>();

        gameElements = GameObject.Find("GameElements");
        menuposition = GameObject.Find("menuposition");

        //clone.transform.position = cannon.transform.position;

        items = new List<GameObject>();
        items.Add(createLife());
        items.Add(createLife());
        items.Add(createLife());

        //MountMenu();

        goToSavePoint();        
    }

    GameObject createLife()
    {
        GameObject life = GameObject.Find("life");
        GameObject clone;
        //GameObject projectile = life.GetComponent<Rigidbody2D>();
        clone = Instantiate(life, life.transform.position, life.transform.rotation) as GameObject;
        return clone;
    }


    void AddItemToPlayer(GameObject item)
    {

        item.transform.parent = gameElements.transform;
        pc.items.Add(item);
    }

    void MountMenu () {

        List<GameObject> itemsTemp = new List<GameObject>();

        float offset = 1.2f;
        float var = -8;

        int ndiamond = 0;
        int ncoin = 0;

        foreach (GameObject item in items)
        {
            //Debug.Log(item.tag);
            if (item.tag == "life")
            {
                itemsTemp.Add(item);
            } else if (item.tag == "key")
            {
                itemsTemp.Add(item);
            }
            else if (item.tag == "spray_can")
            {
                itemsTemp.Add(item);
            }
            else if (item.tag == "diamond")
            {
                if (ndiamond == 0)
                {
                    itemsTemp.Add(item);
                }
                ndiamond++;
                counterDiamonds.text = "X" + ndiamond;
            }
            else if (item.tag == "coin")
            {
                if (ncoin == 0)
                {
                    itemsTemp.Add(item);
                }
                ncoin++;
                counterCoins.text = "X"+ncoin;
            }
        }

        foreach (GameObject item in itemsTemp)
        {
            item.transform.parent = gameElements.transform;
            item.transform.localPosition = new Vector3(var, menuposition.transform.localPosition.y, menuposition.transform.localPosition.z);
            item.SetActive(true);
            var = var + offset;
            
            if (item.tag == "diamond")
            {
                GameObject cDiamonds = GameObject.Find("CanvasDiamonds");
                GameObject ccDiamonds = GameObject.Find("CounterDiamonds");
                Vector3 textOffset =  cDiamonds.transform.position - ccDiamonds.transform.position;
                cDiamonds.transform.SetParent(gameElements.transform);
                cDiamonds.transform.localPosition = new Vector3(var - 0.2f, menuposition.transform.localPosition.y - 0.2f, menuposition.transform.localPosition.z) + textOffset;
                cDiamonds.SetActive(true);
                var = var + offset;

            }
            else if (item.tag == "coin")
            {
                GameObject cCoins = GameObject.Find("CanvasCoins");
                GameObject ccCoins = GameObject.Find("CounterCoins");
                Vector3 textOffset = cCoins.transform.position - ccCoins.transform.position;
                cCoins.transform.SetParent(gameElements.transform);
                cCoins.transform.localPosition = new Vector3(var - 0.5f, menuposition.transform.localPosition.y - 0.2f, menuposition.transform.localPosition.z) + textOffset;
                cCoins.SetActive(true);
                var = var + offset;
            }
        }
    }

    void LateUpdate()
    {
        transform.localPosition = new Vector3(mainCamera.transform.position.x, 
            mainCamera.transform.localPosition.y, mainCamera.transform.localPosition.z) + offset;

    }

    void FixedUpdate () {
        MountMenu();

        /*
        
        */
    }

    public void DoActionWaterLevel()
    {
        if (!s1.activated && !s2.activated && !s3.activated)
        {
            sd.setsEnabled(true);
        }

        int count = getActivationCount(s3.activated, s2.activated, s1.activated);

        if (count == 0)
        {
            w0.goToLevel0();
            w1.goToLevel0();
            w2.goToLevel0();
        }
        else if (count == 1)
        {
            w0.goToLevel1();
            w1.goToLevel1();
        }
        else if (count == 2)
        {
            w0.goToLevel2();
            w1.goToLevel2();
        }
        if (count == 3)
        {
            w0.goToLevel3();
            w1.goToLevel3();
        }

        if (sd.activated)
        {
            //bc.goToLeve1();
            w0.goToLevel4Level1();
            w1.goToLevel4Level1();
            w2.goToLevel4();

            s1.setsEnabled(false);
            s2.setsEnabled(false);
            s3.setsEnabled(false);
        } else
        {
            w2.goToLevel0();
        }

        if (s4.activated)
        {
            w2.goToLevel5();
            //bc.goToLevel2();
        }
    }


    private int getActivationCount (bool a1, bool a2, bool a3)
    {
        int count = 0;
        if (!a1)
        {
            count++;
        }
        if (!a2)
        {
            count++;
        }
        if (!a3)
        {
            count++;
        }
        return count;
    }

    private void goToStartGame()
    {
        s1.setsEnabled(true);
        s2.setsEnabled(true);
        s3.setsEnabled(true);

        s1.activate();
        s2.activate();
        s3.activate();

        sd.setsEnabled(false);
        sd.deactivate();
        s4.deactivate();

        pc.transform.position = new Vector3(-3.43f, -0.46f, -4.9f);
        //bc.transform.position = new Vector3(40.1f, -19.07f, -6.1f);        

    }

    [System.Obsolete()]
    private void goToSavePoint01()
    {
        s1.setsEnabled(true);
        s2.setsEnabled(true);
        s3.setsEnabled(true);

        s1.activate();
        s2.activate();
        s3.activate();

        sd.setsEnabled(false);
        sd.deactivate();
        s4.deactivate();
        pc.transform.position = new Vector3(22.4f, -2.4f, -4.9f);
    }


    private void goToSavePoint02()
    {
        s1.ActionSwitch();
        s2.ActionSwitch();
        s3.ActionSwitch();
        sd.setsEnabled(true);
        sd.ActionSwitch();
        s1.setsEnabled(false);
        s2.setsEnabled(false);
        s3.setsEnabled(false);
        bc.sail = false;
        pc.transform.position = new Vector3(31.34f, 12.8f, -4.9f);
        bc.transform.position = new Vector3(40.1f, 2.1f, -6.1f);
    }

    private void goToSavePoint03()
    {
        s1.deactivate();
        s2.deactivate();
        s3.deactivate();
        sd.setsEnabled(true);
        sd.activate();
        pc.transform.position = new Vector3(60.8f, 2.91f, -4.9f);
        bc.transform.position = new Vector3(55.75f, 1.52f, -6.1f);
    }

    private void goToSavePoint04()
    {
        s1.deactivate();
        s2.deactivate();
        s3.deactivate();
        sd.setsEnabled(true);
        sd.activate();
        s4.setsEnabled(true);
        s4.activate();
        pc.transform.position = new Vector3(88.34f, 15.37f, -4.9f);
        bc.transform.position = new Vector3(75.9f, -3.49f, -6.1f);
    }

    private void goToSavePoint() {
        MountMenu();
        pc.items = items;
        cc.transform.localPosition = pc.transform.localPosition + cameraOffset;

        usingGameAction = false;
        if (savePoint == 1)
        {
            //goToSavePoint01();
        }
        else if (savePoint == 2)
        {
            goToSavePoint02();
        }
        else if (savePoint == 3)
        {
            goToSavePoint03();
        }
        else if (savePoint == 4)
        {
            goToSavePoint04();
        }
        else
        {
            goToStartGame();
        }

        DoActionWaterLevel();
        //usingGameAction = true;
    }

    public void lostLife()
    {
        GameObject life = pc.GetLife();
        if (life == null)
        {
            endGame();
        } else
        {
            pc.items.Remove(life);
            Destroy(life);
            goToSavePoint();
        }
    }

    public void endGame()
    {
        SceneManager.LoadScene(2);
    }
}
