using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputterDamage : MonoBehaviour
{
    public float sputterDamge;
    public float sputterForce;
    public float sputterRadius;
    public float sputterContinueTime;

    // ���ڼ�¼������Чʱ��
    private float timer;

    // ���ڼ�¼�����䵽��ʵ��ID���������˺�
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
            // ̹�˼�Ѫ
            coll.SendMessage("Damage", sputterDamge);

            // ը��̹��
            Vector3[] args = new Vector3[2];
            args[0][0] = 500;
            args[0][1] = sputterForce;
            args[0][2] = sputterRadius;
            args[1] = transform.position;
            // ̹�˴��ʱy�ᱻ����������y������ж��
            args[1][1] = coll.transform.position.y;
            coll.SendMessage("SetNoControlTime", args);
        }
    }
}
