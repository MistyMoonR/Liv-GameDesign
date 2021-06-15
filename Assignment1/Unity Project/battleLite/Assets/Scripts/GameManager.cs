using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager GetInstance()
    {
        return instance;
    }


    private bool isPlayerDead = false;

    public GameObject[] scenes;
    private GameObject curScene;
    public int curLevel = 1;
    public Text time;
    public Text killNUm;
    public bool isStart;
    public GameObject tipsView;
    public GameObject gameOver;
    public int killNum;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        curScene = scenes[0];
        overPage.SetActive((false));
        time.text = "Time:"+((int)fullTime).ToString();
    }

    public void GameStart()
    {
        isStart = true;
    }
    public float fullTime = 120f;
    // Update is called once per frame
    void Update()
    {
        if (Tank_health.instance.hp <= 0)
        {
            isPlayerDead = true;

        }


        if (!isPlayerDead)
        {
            if (isStart)
            {
                fullTime -= Time.deltaTime;
                if (fullTime >= 0)
                {

                    time.text = "Time : " + (int)fullTime;
                }
                else
                {
                    isPlayerDead = true;

                }

                killNUm.text = "killNUm:" + killNum;

                if (killNum == curLevel * 2)
                {
                    isStart = false;
                    tipsView.SetActive(true);
                    tipsView.transform.Find("Stage").GetComponent<Text>().text = "Stage: " + curLevel;
                }
            }
            else
            {
                time.text = "Time:0";
                if (Input.GetKeyDown(KeyCode.N) && !isStart)
                {
                    if (curLevel == 3)
                    {
                        gameOver.SetActive(true);
                        return;
                    }
                    curLevel += 1;
                    NextScene(scenes[curLevel - 1]);
                    isStart = true;
                }
            }

        }
        else
        {
            overPage.SetActive((true));


        }
    }


    public GameObject overPage;


    public void NextScene(GameObject scene)
    {
        scene.SetActive(true);
        curScene.SetActive(false);
        curScene = scene;
        tipsView.SetActive(false);
        killNum = 0;
    }

    public void ReStart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
