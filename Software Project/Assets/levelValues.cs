using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelValues : MonoBehaviour
{
    PlayerStat player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        //Player values carry over from previous
        player.hp = PlayerPrefs.GetInt("HP");
        player.bp = PlayerPrefs.GetInt("BP");
        player.hpMax = PlayerPrefs.GetInt("HPMax");
        player.ppMax = PlayerPrefs.GetInt("PPMax");
        player.threatLV = PlayerPrefs.GetInt("Threat Level");
        player.weapon1 = PlayerPrefs.GetInt("Weapon1");
        player.weapon2 = PlayerPrefs.GetInt("Weapon2");
        player.wep1Level = PlayerPrefs.GetInt("Wep1LV");
        player.wep2Level = PlayerPrefs.GetInt("Wep2LV");
        player.ammoStack1 = PlayerPrefs.GetInt("Ammo Stack 1");
        player.ammoStack2 = PlayerPrefs.GetInt("Ammo Stack 2");
        player.threatGauge = PlayerPrefs.GetFloat("Threat Gauge");
        player.ammoDict["bullet"] = PlayerPrefs.GetFloat("Bullet");
        player.ammoDict["shell"] = PlayerPrefs.GetFloat("Shell");
        player.ammoDict["explosive"] = PlayerPrefs.GetFloat("Explosive");
        player.ammoDict["laser"] = PlayerPrefs.GetFloat("Laser");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
