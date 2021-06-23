using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPorps : MonoBehaviour
{
    private int tank_id;

    public GameObject enemy_tank;

    // 计时道具的持续时间（ms）
    public double prop_time_tot;

    // 计时道具的剩余时间（ms）
    private double prop_time_left;

    // 加速后的坦克速度
    public float speed_up_speed;

    // 减速后的坦克速度
    public float slow_down_speed;

    // 坦克的原始速度
    private float original_tank_speed;

    // 坦克炮弹的原始冷却时间
    private float original_shell_cooling_time;

    // 当前道具id
    private int prop_id;

    // 道具种类
    private Dictionary<string,int> prop_map;

    // Start is called before the first frame update
    void Start()
    {
        tank_id = GetInstanceID();
        original_tank_speed = GetComponent<TankMovement>().speed;
        original_shell_cooling_time = GetComponent<TankAttack>().shell_ready_time_tot;

        // 定义道具种类
        prop_map = new Dictionary<string, int>();
        prop_map["no_prop"] = 0;
        prop_map["speed_up"] = 1;
        prop_map["enemy_slow_down"] = 2;
        prop_map["no_cooling_time"] = 3;
        prop_map["shield"] = 4;
    }

    // Update is called once per frame
    void Update()
    {
        // -----------------------用按键激发道具，用于测试------------------------
        if (Input.GetKey(KeyCode.Alpha2))
        {
            get_prop("speed_up");
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            get_prop("enemy_slow_down");
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            get_prop("no_cooling_time");
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            get_prop("shield");
        }
        // ---------------------------------------------------------------------


        // 无道具直接返回
        if(prop_id == 0){
            return;
        }
        
        if (prop_time_left <= 0)
        {
            // 有道具，且时间到了，取消道具效果
            cancel_prop_effect(prop_id);
            prop_id = 0;
        }
        else
        {
            // 道具时间没到，继续计时
            prop_time_left -= Time.deltaTime * 1000;
        }
    }

    // 接收道具，由SendMessage调用
    void get_prop(string prop_name){
        prop_id = prop_map[prop_name];
        prop_time_left = prop_time_tot;
        set_prop_effect(prop_id);
    }

    // 设置指定id的道具效果
    void set_prop_effect(int id){
        switch (id)
        {
            case 1:
                GetComponent<TankMovement>().speed = speed_up_speed;
                break;
            case 2:
                enemy_tank.GetComponent<TankMovement>().speed = slow_down_speed;
                break;
            case 3:
                GetComponent<TankAttack>().shell_ready_time_tot = 0;
                break;
            case 4:
                GetComponent<TankHealth>().invincible = true;
                break;
            default:
                break;
        }
    }

    // 取消指定id的道具效果
    void cancel_prop_effect(int id){
        switch (id)
        {
            case 1:
                GetComponent<TankMovement>().speed = original_tank_speed;
                break;
            case 2:
                if (enemy_tank.GetComponent<TankMovement>().speed == slow_down_speed)
                {
                    enemy_tank.GetComponent<TankMovement>().speed = original_tank_speed;
                }
                break;
            case 3:
                GetComponent<TankAttack>().shell_ready_time_tot = original_shell_cooling_time;
                break;
            case 4:
                GetComponent<TankHealth>().invincible = false;
                break;
            default:
                break;
        }
    }
}
