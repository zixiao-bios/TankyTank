using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public int propID;

    public bool randomPick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "tank")
        {
            int targetID;
            if (randomPick)
            {
                targetID = Random.Range(-1, 5);
            }
            else
            {
                targetID = propID;
            }
            other.SendMessage("GetProp", targetID);
            Destroy(gameObject);
        }
    }
}
