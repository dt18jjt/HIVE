using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelValues : MonoBehaviour
{
    PlayerStat player;
    Log log; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        log = log = GameObject.Find("Global").GetComponent<Log>();
        //Player values carry over from previous
        player.hp = PlayerPrefs.GetFloat("HP");
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
        //player.ammoDict["bullet"] = PlayerPrefs.GetFloat("Bullet");
        //player.ammoDict["shell"] = PlayerPrefs.GetFloat("Shell");
        //player.ammoDict["explosive"] = PlayerPrefs.GetFloat("Explosive");
        //added enemies values
        log.add003 = (PlayerPrefs.GetInt("003") != 0);
        log.add004 = (PlayerPrefs.GetInt("004") != 0);
        log.add005 = (PlayerPrefs.GetInt("005") != 0);
        log.add006 = (PlayerPrefs.GetInt("006") != 0);
        log.add007 = (PlayerPrefs.GetInt("007") != 0);
        log.add008 = (PlayerPrefs.GetInt("008") != 0);
        log.add009 = (PlayerPrefs.GetInt("009") != 0);
        log.add010 = (PlayerPrefs.GetInt("010") != 0);
        log.add011 = (PlayerPrefs.GetInt("011") != 0);
        log.add012 = (PlayerPrefs.GetInt("012") != 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
