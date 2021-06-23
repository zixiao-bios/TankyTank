using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    // 坦克爆炸动画
    public GameObject tankExplosion;

    // 坦克血量条
    public Slider hpSlider;

    // 爆炸后黑烟
    public ParticleSystem destroySmoke;

    // 坦克总血量
    private int hpTotal = 100;

    // 坦克现在血量
    private int hp;

    private bool isDead = false;

    private bool invincible = false;

    // Start is called before the first frame update
    void Start()
    {
        hp = hpTotal;
        hpSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 坦克被攻击
    void Damage(int damage_value){
        // 无敌检测
        if (!invincible)
        {
            // 减血
            hp -= damage_value;
            hpSlider.value = (float)hp / hpTotal;

            if (hp <= 0 && !isDead)
            {
                // 坦克死亡
                isDead = true;
                GameObject.Instantiate(tankExplosion, transform.position + Vector3.up, transform.rotation);
                gameObject.SendMessage("SetFreeze", true);
                destroySmoke.Play();
                foreach(GameObject tank in GameObject.FindGameObjectsWithTag("tank"))
                {
                    tank.SendMessage("SetInvincible", true);
                }
            }
        }
    }

    void SetInvincible(bool flag)
    {
        invincible = flag;
    }
}
