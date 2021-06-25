using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveProps : MonoBehaviour
{
    public Text activePropText;

    public GameObject minePrefab;

    private Dictionary<int, KeyCode> keyDict = new Dictionary<int, KeyCode>();

    private List<int> validActivePropID = new List<int> {-1, -2};

    private int curPropID;

    private int playerID;

    private KeyCode propKey;

    // Start is called before the first frame update
    void Start()
    {
        keyDict[1] = KeyCode.LeftControl;
        keyDict[2] = KeyCode.RightArrow;

        playerID = gameObject.GetComponent<TankMovement>().playerID;

        propKey = keyDict[playerID];
    }

    // Update is called once per frame
    void Update()
    {
        if (curPropID != 0)
        {
            if(Input.GetKeyDown(propKey))
            {
                switch(curPropID)
                {
                    case -1:
                        GameObject mine = GameObject.Instantiate(minePrefab, transform.position, transform.rotation);
                        mine.SendMessage("SetOwner", playerID);
                        break;
                }
                curPropID = 0;
            }
        }
    }

    // 触发宝箱时回调此函数
    public void GetProp(int propID)
    {
        switch (propID)
        {
            case -1:
                activePropText.text = "地雷";
                curPropID = propID;
                break;
            default:
                curPropID = 0;
                break;
        }
    }

    public void Respawn()
    {
        curPropID = 0;
    }
}
