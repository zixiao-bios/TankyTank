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
        GameObject.Destroy(exp, 1);
        GameObject.Destroy(this.gameObject);

        if(coll.gameObject.tag == "tank"){
            // 坦克减血
            coll.gameObject.SendMessage("Damage", 30);

            // 炸飞坦克
            Vector3 dir = coll.transform.position - this.transform.position;
            Vector3[] arg = new Vector3[2];
            arg[0][0] = 500;
            arg[1] = dir * 30;
            coll.gameObject.SendMessage("SetNoControlTime", arg);
            coll.gameObject.GetComponent<Rigidbody>().velocity = arg[1];

        }
    }
}