using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputterDamage : MonoBehaviour
{
    public float sputterDamge;
    public float sputterForce;
    public float sputterRadius;
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
            Vector3[] args = new Vector3[2];
            args[0][0] = 500;
            args[0][1] = sputterForce;
            args[0][2] = sputterRadius;
            args[1] = transform.position;
            // 坦克存活时y轴被锁定，忽略y轴以免卸力
            args[1][1] = coll.transform.position.y;
            coll.SendMessage("SetNoControlTime", args);
        }
    }
}
