using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveProps : MonoBehaviour
{
    public Sprite speedUpSprite;
    public Sprite slowDownSprite;
    public Sprite noCDSprite;
    public Sprite shieldSprite;

    public GameObject passivePropIcon;

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

    // 记录上帧技能图标旋转值
    private Vector3 lastRotation;

    // Start is called before the first frame update
    void Start()
    {
        original_tank_speed = GetComponent<TankMovement>().speed;
        original_shell_cooling_time = GetComponent<TankAttack>().shell_ready_time_tot;

        // 定义道具种类
        prop_map = new Dictionary<string, int>();
        prop_map["no_prop"] = 0;
        prop_map["speed_up"] = 1;
        prop_map["enemy_slow_down"] = 2;
        prop_map["no_cooling_time"] = 3;
        prop_map["shield"] = 4;

        lastRotation = passivePropIcon.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // -----------------------用按键激发道具，用于测试------------------------
        // if (Input.GetKey(KeyCode.Alpha2))
        // {
        //     GetProp(1);
        // }
        // else if (Input.GetKey(KeyCode.Alpha3))
        // {
        //     GetProp(2);
        // }
        // else if (Input.GetKey(KeyCode.Alpha4))
        // {
        //     GetProp(3);
        // }
        // else if (Input.GetKey(KeyCode.Alpha5))
        // {
        //     GetProp(4);
        // }
        // ---------------------------------------------------------------------

        // 技能图标旋转
        Vector3 curRotation = lastRotation + new Vector3(0.0f, 0.5f, 0.0f);
        passivePropIcon.transform.rotation = Quaternion.Euler(curRotation);
        lastRotation = curRotation;

        // 无道具直接返回
        if (prop_id == 0){
            return;
        }
        
        if (prop_time_left <= 0)
        {
            // 有道具，且时间到了，取消道具效果
            CancelPropEffect(prop_id);
            prop_id = 0;
        }
        else
        {
            // 道具时间没到，继续计时
            prop_time_left -= Time.deltaTime * 1000;
        }
    }

    // 接收道具，由SendMessage调用
    void GetProp(int id){
        // 被动ID大于等于0
        if (id >= 0)
        {
            // 取消现有道具效果
            CancelPropEffect(prop_id);

            // 设置新的道具
            prop_time_left = prop_time_tot;
            prop_id = id;
            SetPropEffect(prop_id);
        }
    }

    // 设置指定id的道具效果
    void SetPropEffect(int id){
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
        passivePropIcon.GetComponent<SpriteRenderer>().sprite = GetSprite(id);
    }

    // 取消指定id的道具效果
    void CancelPropEffect(int id){
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

    public void Respawn()
    {
        GetProp(0);
    }

    Sprite GetSprite(int propID)
    {
        switch (propID)
        {
            case 1:
                return speedUpSprite;
            case 2:
                return slowDownSprite;
            case 3:
                return noCDSprite;
            case 4:
                return shieldSprite;
            default:
                return null;
        }
    }
}
