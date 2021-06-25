using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    // 爆炸延时(ms)
    public float explosion_delay;

    // 导弹爆炸动画
    public GameObject explosion_effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        explosion_delay -= Time.deltaTime * 1000;
        if (explosion_delay <= 0){
            Explode();
            return;
        }
    }

    public void OnCollisionEnter(Collision coll){
        Explode();
        if(coll.gameObject.tag == "tank"){
            // 坦克减血
            coll.gameObject.SendMessage("Damage", 50);

            // 炸飞坦克
            Vector3 dir = coll.transform.position - this.transform.position;
            Vector3[] arg = new Vector3[2];
            arg[0][0] = 500;
            arg[1] = dir * 30;
            coll.gameObject.SendMessage("SetNoControlTime", arg);
            coll.gameObject.GetComponent<Rigidbody>().velocity = arg[1];
        }
    }

    // 导弹爆炸
    void Explode(){
        GameObject exp = GameObject.Instantiate(explosion_effect, this.transform.position, this.transform.rotation);
        GameObject.Destroy(exp, 2f);
        GameObject.Destroy(this.gameObject);
    }
}
