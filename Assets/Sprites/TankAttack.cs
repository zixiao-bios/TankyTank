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

    // 子弹初速度
    public float shellSpeed;

    // 导弹冷却所需时间
    public float missile_ready_time_tot;

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

    // Start is called before the first frame update
    void Start()
    {
        shell_fire_pos = transform.Find("ShellFirePosition");
        missile_fire_pos = transform.Find("MissileFirePosition");

        switch(tankID){
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
        if(!shell_ready){
            // 子弹正在冷却
            shell_time_remain -= Time.deltaTime * 1000;
            shell_ready_slider.value = shell_time_remain / shell_ready_time_tot;
            if (shell_time_remain <= 0){
                shell_ready = true;
                shell_ready_slider.value = 0;
            }
        }

        // 处理导弹冷却
        if(!missile_ready){
            // 导弹正在冷却
            missile_time_remain -= Time.deltaTime * 1000;
            missile_ready_slider.value = missile_time_remain / missile_ready_time_tot;
            if(missile_time_remain <= 0){
                missile_ready = true;
                missile_ready_slider.value = 0;
            }
        }
        
        // 发射子弹
        if(shell_ready){
            foreach (KeyCode key in shell_key)
                if (Input.GetKey(key)){
                    GameObject shell = GameObject.Instantiate(shell_prefab, shell_fire_pos.position, shell_fire_pos.rotation);
                    shell.GetComponent<Rigidbody>().velocity = shell.transform.forward * shellSpeed;

                    shell_ready = false;
                    shell_time_remain = shell_ready_time_tot;
                    break;
                }
        }

        // 发射导弹
        if(missile_ready){
            if(Input.GetKey(missile_key)){
                // 创建导弹
                GameObject missile = GameObject.Instantiate(missile_prefab, missile_fire_pos.position, missile_fire_pos.rotation);

                // 设置导弹的tankID
                missile.SendMessage("setTankID", tankID);

                // 设置导弹初始朝向
                missile.transform.localEulerAngles = new Vector3(90, missile.transform.localEulerAngles[1], missile.transform.localEulerAngles[2]);

                missile_ready = false;
                missile_time_remain = missile_ready_time_tot;
            }
        }
    }
}
