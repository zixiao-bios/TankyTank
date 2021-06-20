using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControl : MonoBehaviour
{
    // 发射此导弹的tankID
    public int tankID;

    // test
    private float angle = (float)0.1;

    // 导弹角速度
    public float angularSpeed = 3;

    // 导弹速度
    public float speed = 15;

    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate(){
        rigidbody.velocity = transform.up * speed;

        float h = Input.GetAxis("Player" + tankID + "Missile");
        rigidbody.angularVelocity = transform.forward * h * angularSpeed;
    }

    void setTankID(int id){
        tankID = id;
    }
}
