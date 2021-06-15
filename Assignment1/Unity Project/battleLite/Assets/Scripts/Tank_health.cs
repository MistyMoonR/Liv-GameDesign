using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameObject = UnityEngine.GameObject;

public class Tank_health : MonoBehaviour
{
    public TankType tankType;
    // Start is called before the first frame update
    public int hp = 100;
    public GameObject tankExplosion;
    public AudioClip tankExplosionAudio;
    public Slider hpSlider;
    public static Tank_health instance;

    private int hpTotal;

    void Start()
    {
        hpTotal = hp;
        if (tankType == TankType.player)
        {
            instance = this;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }

   public  void TakeDamage()
    {
        if (hp <= 0)
        {
            return;
        }
        hp -= Random.Range(15, 25);
        hpSlider.value = (float) hp / hpTotal;

        if (hp <= 0)
        {
            AudioSource.PlayClipAtPoint(tankExplosionAudio, transform.position);
            GameObject.Instantiate(tankExplosion, transform.position + Vector3.up, transform.rotation);
            if (tankType== TankType.enemy)
            {
                GameManager.GetInstance().killNum++;
            }
            GameObject.Destroy(this.gameObject);
        }
    }
}
