using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public Material redMaterial;

    public Material greenMaterial;

    private int ownerID;

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
            int playerID = other.GetComponent<TankMovement>().playerID;
            if (playerID != ownerID)
            {
                // º¥À¿
                other.SendMessage("Kill", 1000);
                Destroy(gameObject);
            }
        }
    }

    public void SetOwner(int playerID)
    {
        GameObject mineLight = transform.GetChild(0).gameObject;
        ownerID = playerID;
        Color lightColor;
        Material lightMaterial;
        if (ownerID == 1)
        {
            lightColor = new Color(1, 0, 0);
            lightMaterial = redMaterial;
        }
        else
        {
            lightColor = new Color(0, 1, 0);
            lightMaterial = greenMaterial;
        }
        mineLight.GetComponent<Light>().color = lightColor;
        mineLight.GetComponent<MeshRenderer>().material = lightMaterial;
    }
}
