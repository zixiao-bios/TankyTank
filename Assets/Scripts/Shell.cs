using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject shellExplosion;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision coll){
        GameObject exp = GameObject.Instantiate(shellExplosion, transform.position, transform.rotation);
        GameObject.Destroy(exp, 2f);
        GameObject.Destroy(this.gameObject);

        if(coll.gameObject.tag == "tank"){
            // 坦克减血
            coll.gameObject.SendMessage("Damage", 20);
        }
    }
}
