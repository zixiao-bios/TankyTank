using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputterDamage : MonoBehaviour
{
    public int sputterDamge;
    public int sputterSpeed;

    // ���ڼ�¼�����䵽��ʵ��ID���������˺�
    private List<int> instanceIDs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "tank" && !instanceIDs.Contains(coll.GetInstanceID()))
        {
            instanceIDs.Add(coll.GetInstanceID());
            // ̹�˼�Ѫ
            coll.SendMessage("Damage", sputterDamge);

            // ը��̹��
            Vector3 dir = coll.transform.position - this.transform.position;
            Vector3[] arg = new Vector3[2];
            arg[0][0] = 500;
            arg[1] = dir * sputterSpeed;
            coll.SendMessage("SetNoControlTime", arg);
            coll.GetComponent<Rigidbody>().velocity = arg[1];
        }
    }
}
