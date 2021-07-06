using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public Text victoryText;

    public Text versusText;

    public float gameSetTimeGap;

    public List<GameObject> respawnList = new List<GameObject>();

    // 落地成盒
    public GameObject treasurePrefab;

    private Dictionary<int, int> victoryCounter = new Dictionary<int, int>();

    private Dictionary<int, string> victoryColor = new Dictionary<int, string>();

    private float timeCounter = 0.0f;

    private bool gameSet = false;

    private List<Vector3> loserPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        victoryColor[1] = "F50000";
        victoryColor[2] = "00F500";
        victoryCounter[1] = 0;
        victoryCounter[2] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSet)
        {
            if (timeCounter > 0.0)
            {
                timeCounter -= Time.deltaTime;
            }
            else
            {
                victoryText.text = string.Empty;
                // 使得两个坦克重生
                foreach (GameObject respawnObj in respawnList)
                {
                    respawnObj.SendMessage("Respawn");
                }
                // 落地成盒
                foreach(Vector3 pos in loserPositions)
                {
                    GameObject.Instantiate(treasurePrefab, pos, Quaternion.identity);
                }
                gameSet = false;
            }
        }

        // 退出游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SetVictory(Vector3[] args)
    {
        int loserID = (int)args[0][0];
        loserPositions.Add(args[1]);
        int winnerID = loserID == 1 ? 2 : 1;
        victoryCounter[winnerID]++;
        victoryText.text = string.Format("<size=120><color=#{0}>胜利</color></size>\n<size=30><color=#white>" +
            "当前比分：</color><color=#F50000>{1}</color> <color=#white>-</color> <color=#00F500>{2}</color></size>", victoryColor[winnerID], victoryCounter[1], victoryCounter[2]);
        versusText.text = string.Format("<color=#F50000>{0}</color> <color=#FFB900>VS</color> <color=#00F500>{1}</color>", victoryCounter[1], victoryCounter[2]);
        timeCounter = gameSetTimeGap;
        gameSet = true;
    }
}
