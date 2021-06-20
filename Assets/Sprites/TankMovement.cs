using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float speed;
    public float angularSpeed;
    public float tankID;
    private Rigidbody rigidbody;

    // 坦克剩余不受控制的时间（ms)
    private float no_control_time = 0;

    // 坦克不受控的总时间（ms）
    private float no_control_time_tot;

    // 坦克是否不受控制
    private bool no_control =  false;

    // 坦克不受控时的初速度
    private Vector3 no_control_speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(no_control){
            no_control_time -= Time.deltaTime * 1000;
            rigidbody.velocity = no_control_speed * no_control_time / no_control_time_tot;
            if(no_control_time <= 0){
                no_control = false;
            }
        }
    }
    void FixedUpdate()
    {
        if(no_control){
            return;
        }

        float v = Input.GetAxis("Player" + tankID + "Vertical");
        rigidbody.velocity = transform.forward * v * speed;

        float h = Input.GetAxis("Player" + tankID + "Horizontal");
        rigidbody.angularVelocity = transform.up * h * angularSpeed;
    }

    void set_no_control_time(Vector3[] arg){
        no_control_time_tot = arg[0][0];
        no_control_time = no_control_time_tot;
        no_control_speed = arg[1];
        if(no_control_time > 0){
            no_control = true;
        }
    }
}