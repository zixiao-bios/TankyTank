using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public Sprite speedUpSprite;
    public Sprite slowDownSprite;
    public Sprite noCDSprite;
    public Sprite shieldSprite;
    public Sprite mineSprite;
    public Sprite exchangeSprite;
    public Sprite randomSprite;

    public SpriteRenderer icon;

    public int propID;

    public bool randomPick;

    // Start is called before the first frame update
    void Start()
    {
        if (randomPick)
        {
            propID = Random.Range(-1, 5);
            icon.sprite = randomSprite;
        }
        else
        {
            icon.sprite = GetSprite(propID);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "tank")
        {
            other.SendMessage("GetProp", propID);
            Destroy(gameObject);
        }
    }

    Sprite GetSprite(int propID)
    {
        switch(propID)
        {
            case 1:
                return speedUpSprite;
            case 2:
                return slowDownSprite;
            case 3:
                return noCDSprite;
            case 4:
                return shieldSprite;
            case -1:
                return mineSprite;
            case -2:
                return exchangeSprite;
            default:
                return randomSprite;
        }
    }
}
