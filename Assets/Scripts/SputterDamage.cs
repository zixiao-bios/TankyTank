using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputterDamage : MonoBehaviour
{
    public int sputterDamge;
    public int sputterSpeed;
    public float sputterContinueTime;

    // 用于记录溅射有效时间
    private float timer;

    // 用于记录被溅射到的实例ID，避免多次伤害
    private List<int> instanceIDs = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        timer = sputterContinueTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (timer > 0 && coll.tag == "tank" && !instanceIDs.Contains(coll.GetInstanceID()))
        {
            instanceIDs.Add(coll.GetInstanceID());
            // 坦克减血
            coll.SendMessage("Damage", sputterDamge);

            // 炸飞坦克
            Vector3 dir = coll.transform.position - this.transform.position;
            Vector3[] arg = new Vector3[2];
            arg[0][0] = 500;
            arg[1] = dir * sputterSpeed;
            coll.SendMessage("SetNoControlTime", arg);
            coll.GetComponent<Rigidbody>().velocity = arg[1];
        }
    }
}
