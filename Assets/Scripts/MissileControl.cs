using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControl : MonoBehaviour
{
    // 发射此导弹的tankID
    public int tankID;

    // 导弹角速度
    public float angularSpeed = 3;

    // 导弹速度
    public float speed = 15;

    private Rigidbody missileRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        missileRigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate(){
        missileRigidbody.velocity = transform.up * speed;

        float h = Input.GetAxis("Player" + tankID + "Missile");
        missileRigidbody.angularVelocity = transform.forward * h * angularSpeed;
    }

    void SetTankID(int id){
        tankID = id;
    }
}
