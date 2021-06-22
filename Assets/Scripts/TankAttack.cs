using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankAttack : MonoBehaviour
{
    // 子弹预制体
    public GameObject shell_prefab;

    // 导弹预制体
    public GameObject missile_prefab;

    public int tankID;

    // 子弹冷却所需时间
    public float shell_ready_time_tot;

    // 子弹冷却条
    public Slider shell_ready_slider;

    // 导弹冷却条
    public Slider missile_ready_slider;

    // 子弹最大蓄力时长
    public float maxChargeTime;

    // 导弹冷却所需时间
    public float missile_ready_time_tot;

    // 子弹蓄力槽
    public Slider aimSlider;

    // 导弹速度
    public float missile_speed;

    private KeyCode[] shell_key = new KeyCode[2];
    private KeyCode missile_key;

    // 子弹的发射位置
    private Transform shell_fire_pos;

    // 导弹的发射位置
    private Transform missile_fire_pos;

    // 子弹是否冷却
    private bool shell_ready = true;

    // 子弹当前剩余冷却时间
    private float shell_time_remain;

    // 导弹是否冷却
    private bool missile_ready = true;

    // 导弹剩余冷却时间
    private float missile_time_remain;

    // 子弹蓄力相关变量
    private float minLauchForce;
    private float maxLauchForce;
    private float curLauchForce;
    private float chargeSpeed;
    private AudioSource chargeAudio;

    // 发射音效
    private AudioSource shootAudio;



    // Start is called before the first frame update
    void Start()
    {
        minLauchForce = aimSlider.minValue;
        maxLauchForce = aimSlider.maxValue;
        curLauchForce = minLauchForce;
        chargeSpeed = (maxLauchForce - minLauchForce) / maxChargeTime;

        shootAudio = gameObject.GetComponents<AudioSource>()[2];
        chargeAudio = gameObject.GetComponents<AudioSource>()[3];

        shell_fire_pos = transform.Find("ShellFirePosition");
        missile_fire_pos = transform.Find("MissileFirePosition");

        switch (tankID) {
            case 1:
                shell_key[0] = KeyCode.Space;
                missile_key = KeyCode.Alpha1;
                break;
            case 2:
                shell_key[0] = KeyCode.KeypadEnter;
                shell_key[1] = KeyCode.Return;
                missile_key = KeyCode.Keypad1;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 处理子弹冷却
        if (!shell_ready) {
            // 子弹正在冷却
            shell_time_remain -= Time.deltaTime * 1000;
            shell_ready_slider.value = shell_time_remain / shell_ready_time_tot;
            if (shell_time_remain <= 0) {
                shell_ready = true;
                shell_ready_slider.value = 0;
            }
        }

        // 处理导弹冷却
        if (!missile_ready) {
            // 导弹正在冷却
            missile_time_remain -= Time.deltaTime * 1000;
            missile_ready_slider.value = missile_time_remain / missile_ready_time_tot;
            if (missile_time_remain <= 0) {
                missile_ready = true;
                missile_ready_slider.value = 0;
            }
        }

        // 发射子弹
        if (shell_ready) {
            // 蓄力满自动发射
            if (curLauchForce >= maxLauchForce) {
                curLauchForce = maxLauchForce;
                FireBullet();
            }
            // 按下发射键蓄力
            else if (GetKeys(shell_key))
            {
                curLauchForce += Time.deltaTime * chargeSpeed;
                // 连续发射导弹会导致音效短暂播放，效果不好，因此提前判断蓄力值再播放
                if (curLauchForce >= minLauchForce + (maxLauchForce - minLauchForce) / 5)
                {
                    aimSlider.value = curLauchForce;
                    if (!chargeAudio.isPlaying)
                    {
                        chargeAudio.Play();
                    }
                }
            }
            // 释放发射键发射
            else if (GetKeysUp(shell_key))
            {
                FireBullet();
            }
        }

        // 发射导弹
        if (missile_ready) {
            if (Input.GetKey(missile_key)) {
                shootAudio.Play();
                // 创建导弹
                GameObject missile = GameObject.Instantiate(missile_prefab, missile_fire_pos.position, missile_fire_pos.rotation);

                // 设置导弹的tankID
                missile.SendMessage("SetTankID", tankID);

                // 设置导弹初始朝向
                missile.transform.localEulerAngles = new Vector3(90, missile.transform.localEulerAngles[1], missile.transform.localEulerAngles[2]);

                missile_ready = false;
                missile_time_remain = missile_ready_time_tot;
            }
        }
    }

    private bool GetKeys(KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKey(key))
            {
                return true;
            }
        }
        return false;
    }

    private bool GetKeysUp(KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyUp(key))
            {
                return true;
            }
        }
        return false;
    }

    private void FireBullet()
    {
        chargeAudio.Stop();
        shootAudio.Play();
        GameObject shell = GameObject.Instantiate(shell_prefab, shell_fire_pos.position, shell_fire_pos.rotation);
        shell.GetComponent<Rigidbody>().velocity = shell.transform.forward * curLauchForce;

        shell_ready = false;
        shell_time_remain = shell_ready_time_tot;

        // 发射结束，重置蓄力槽
        curLauchForce = minLauchForce;
        aimSlider.value = minLauchForce;
    }
}
