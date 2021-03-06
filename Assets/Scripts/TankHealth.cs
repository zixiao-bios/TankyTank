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

    // 游戏总体控制对象
    public GameObject gameController;

    // 坦克总血量
    private int hpTotal = 100;

    // 坦克现在血量
    private int hp;

    [HideInInspector]
    public bool isDead = false;

    // 坦克无敌
    [HideInInspector]
    public bool invincible = false;

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
    public void Damage(int damage_value){
        // 无敌检测
        if (!invincible)
        {
            // 减血
            hp -= damage_value;
            hpSlider.value = (float)hp / hpTotal;

            if (hp <= 0 && !isDead)
            {
                Kill(0);
            }
        }
    }

    public void Kill(float force)
    {
        // 坦克死亡
        hpSlider.value = 0;
        isDead = true;
        GameObject tankExp = GameObject.Instantiate(tankExplosion, transform.position + Vector3.up, transform.rotation);
        GameObject.Destroy(tankExp, 2f);
        gameObject.SendMessage("SetFreeze", true);
        destroySmoke.Play();
        foreach (GameObject tank in GameObject.FindGameObjectsWithTag("tank"))
        {
            tank.SendMessage("SetInvincible", true);
        }
        // 解锁刚体坐标轴
        gameObject.SendMessage("FreeConstrains");
        // 胜利信号
        Vector3[] args = new Vector3[2];
        args[0][0] = gameObject.GetComponent<TankMovement>().playerID;
        args[1] = gameObject.transform.position;
        gameController.SendMessage("SetVictory", args);
    }

    public void SetInvincible(bool flag)
    {
        invincible = flag;
    }

    public void Respawn()
    {
        invincible = false;
        isDead = false;
        destroySmoke.Stop();
        hp = hpTotal;
        hpSlider.value = (float)hp / hpTotal;
    }
}
