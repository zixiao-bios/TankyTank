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
    private bool noControl = false;

    // 导致坦克不受控的爆炸原点
    private Vector3 groundZero;

    // 不受控爆炸的作用力
    private float explosionForce;
    private Vector3 explosionTorque;

    // 不受控爆炸的范围
    private float explosionRadius;

    // 引擎声音
    private AudioSource idleAudio;
    private AudioSource drivingAudio;

    // 记录初始位置
    private Vector3 initPosition;
    private Vector3 initScale;
    private Quaternion initRotation;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = gameObject.transform.position;
        initScale = gameObject.transform.localScale;
        initRotation = gameObject.transform.rotation;
        tankRigidbody = this.GetComponent<Rigidbody>();

        idleAudio = gameObject.GetComponents<AudioSource>()[0];
        drivingAudio = gameObject.GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (noControl)
        {
            // 仅在开始时触发一次爆炸受力
            if (noControlTime == noControlTimeTotal)
            {
                // 坦克存活时y轴被锁定，将原爆点设置为与坦克水平，以免y轴卸力
                if (!gameObject.GetComponent<TankHealth>().isDead)
                {
                    groundZero.y = gameObject.transform.position.y;
                }
                // 死亡后适当降低y轴，实现坦克被炸飞的效果
                else
                {
                    groundZero.y = -explosionRadius / 2f;
                }
                tankRigidbody.AddExplosionForce(explosionForce, groundZero, explosionRadius);
                tankRigidbody.AddTorque(explosionTorque);
            }
            noControlTime -= Time.deltaTime * 1000;
            if (noControlTime <= 0) {
                noControl = false;
            }
        }
    }
    void FixedUpdate()
    {
        if (noControl)
        {
            return;
        }

        if (!isFrozen)
        {
            float v = Input.GetAxis("Player" + playerID + "Vertical");
            tankRigidbody.velocity = transform.forward * v * speed;

            float h = Input.GetAxis("Player" + playerID + "Horizontal");
            tankRigidbody.angularVelocity = transform.up * h * angularSpeed;

            if (v != 0 || h != 0)
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

    public void SetNoControlTime(Vector3[] args) {
        noControlTimeTotal = args[0][0];
        noControlTime = noControlTimeTotal;
        explosionForce = args[0][1];
        explosionRadius = args[0][2];
        explosionTorque = args[1];
        groundZero = args[2];
        if (noControlTime > 0) {
            noControl = true;
        }
    }

    public void FreeConstrains()
    {
        // 解除轴向锁定
        Rigidbody tankRigidbody = gameObject.GetComponent<Rigidbody>();
        tankRigidbody.constraints = RigidbodyConstraints.None;
    }

    public void SetFreeze(bool flag)
    {
        isFrozen = flag;
    }

    public void Respawn()
    {
        gameObject.transform.position = initPosition;
        gameObject.transform.rotation = initRotation;
        gameObject.transform.localScale = initScale;
        // 重新锁定刚体
        Rigidbody tankRigidbody = gameObject.GetComponent<Rigidbody>();
        tankRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        SetFreeze(false);
    }
}