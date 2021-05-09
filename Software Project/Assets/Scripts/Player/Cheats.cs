using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    public EnemyFollow[] enemy;
    PlayerStat player;
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player").GetComponent<PlayerStat>();

    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemy = new EnemyFollow[enemies.Length];
        for (int i = 0; i < enemies.Length; ++i)
            enemy[i] = enemies[i].GetComponent<EnemyFollow>();
        if (Input.GetKey(KeyCode.F1))
        {
            
        }
        //Active switch cheat
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            player.actNum = 0;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            player.actNum = 1;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            player.actNum = 2;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            player.actNum = 3;
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            player.actNum = 4;
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            player.pasNum = 0;
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            player.pasNum = 1;
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            player.pasNum = 2;
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            player.pasNum = 3;
        }
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            player.pasNum = 4;
        }
        //Ammo cheat
        if (Input.GetKeyUp(KeyCode.F2))
        {
            player.hp = player.hpMax;
            player.ammoDict["bullet"] = player.ammoMaxDict["bulletMax"];
            player.ammoDict["shell"] = player.ammoMaxDict["shellMax"];
            player.ammoDict["explosive"] = player.ammoDict["explosiveMax"];
        }
    }
}
