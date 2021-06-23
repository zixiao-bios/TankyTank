using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float speed;
    public float angularSpeed;
    public int playerID;
    private Rigidbody tankRigidbody;

    // 冻结判断
    private bool isFrozen = false;

    // 坦克剩余不受控制的时间（ms)
    private float noControlTime = 0;

    // 坦克不受控的总时间（ms）
    private float noControlTimeTotal;

    // 坦克是否不受控制
    private bool noControl =  false;

    // 坦克不受控时的初速度
    private Vector3 noControlSpeed;

    // 引擎声音
    private AudioSource idleAudio;
    private AudioSource drivingAudio;

    // Start is called before the first frame update
    void Start()
    {
        tankRigidbody = this.GetComponent<Rigidbody>();

        idleAudio = gameObject.GetComponents<AudioSource>()[0];
        drivingAudio = gameObject.GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if(noControl)
        {
            noControlTime -= Time.deltaTime * 1000;
            tankRigidbody.velocity = noControlSpeed * noControlTime / noControlTimeTotal;
            if(noControlTime <= 0){
                noControl = false;
            }
        }
    }
    void FixedUpdate()
    {
        if(noControl)
        {
            return;
        }

        if (!isFrozen)
        {
            float v = Input.GetAxis("Player" + playerID + "Vertical");
            tankRigidbody.velocity = transform.forward * v * speed;

            float h = Input.GetAxis("Player" + playerID + "Horizontal");
            tankRigidbody.angularVelocity = transform.up * h * angularSpeed;

            if(v != 0 || h != 0)
            {
                idleAudio.Stop();
                if (!drivingAudio.isPlaying)
                {
                    drivingAudio.Play();
                }
            }
            else
            {
                drivingAudio.Stop();
                if (!idleAudio.isPlaying)
                {
                    idleAudio.Play();
                }
            }
        }
    }

    void SetNoControlTime(Vector3[] arg){
        noControlTimeTotal = arg[0][0];
        noControlTime = noControlTimeTotal;
        noControlSpeed = arg[1];
        if(noControlTime > 0){
            noControl = true;
        }
    }

    public void SetFreeze(bool flag)
    {
        isFrozen = flag;
    }
}