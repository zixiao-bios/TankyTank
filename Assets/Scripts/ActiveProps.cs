using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveProps : MonoBehaviour
{
    public Sprite mineSprite;
    public Sprite exchangeSprite;
    public Sprite UIDefault;

    public GameObject activePropIcon;

    public GameObject minePrefab;

    public GameObject enemyTank;

    private Dictionary<int, KeyCode> keyDict = new Dictionary<int, KeyCode>();

    private List<int> validActivePropID = new List<int> {-1, -2};

    private int curPropID;

    private int playerID;

    private KeyCode propKey;

    // Start is called before the first frame update
    void Start()
    {
        keyDict[1] = KeyCode.LeftControl;
        keyDict[2] = KeyCode.RightShift;

        playerID = gameObject.GetComponent<TankMovement>().playerID;

        propKey = keyDict[playerID];
    }

    // Update is called once per frame
    void Update()
    {
        // ID小于0说明存在主动道具
        if (curPropID < 0)
        {
            if(Input.GetKeyDown(propKey))
            {
                switch(curPropID)
                {
                    case -1:
                        GameObject mine = GameObject.Instantiate(minePrefab, transform.position, transform.rotation);
                        mine.SendMessage("SetOwner", playerID);
                        break; 
                    case -2:
                        Vector3 positionTmp = gameObject.transform.position;
                        Quaternion rotationTmp = gameObject.transform.rotation;
                        gameObject.transform.position = enemyTank.transform.position;
                        gameObject.transform.rotation = enemyTank.transform.rotation;
                        enemyTank.transform.position = positionTmp;
                        enemyTank.transform.rotation = rotationTmp;
                        break;
                    default:
                        break;
                }
                GetProp(0);
            }
        }
    }

    // 接收道具，由SendMessage调用
    public void GetProp(int propID)
    {
        // 主动ID小于等于0
        if (propID <= 0)
        {
            curPropID = propID;
            activePropIcon.GetComponent<Image>().sprite = GetSprite(propID);
        }
    }

    public void Respawn()
    {
        GetProp(0);
    }

    Sprite GetSprite(int propID)
    {
        switch (propID)
        {
            case -1:
                return mineSprite;
            case -2:
                return exchangeSprite;
            default:
                return UIDefault;
        }
    }
}
