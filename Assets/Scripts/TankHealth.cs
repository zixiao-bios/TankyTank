using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    // 坦克爆炸动画
    public GameObject tank_explosion;

    // 坦克血量条
    public Slider hp_slider;

    // 爆炸后黑烟
    public ParticleSystem destroySmoke;

    // 坦克总血量
    private int hp_tot = 100;

    // 坦克现在血量
    private int hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = hp_tot;
        hp_slider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 坦克被攻击
    void Damage(int damage_value){
        // 减血
        hp -= damage_value;
        hp_slider.value = (float)hp/hp_tot;

        if(hp <= 0){
            // 坦克死亡
            GameObject.Instantiate(tank_explosion, transform.position + Vector3.up, transform.rotation);
            gameObject.GetComponent<TankMovement>().Freeze();
            destroySmoke.Play();
        }
    }
}
