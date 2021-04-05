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
            player.Active = "Firebomb";
        if (Input.GetKeyUp(KeyCode.Alpha2))
            player.Active = "Freeze Blast";
        if (Input.GetKeyUp(KeyCode.Alpha3))
            player.Active = "Bolt Dash";
        if (Input.GetKeyUp(KeyCode.Alpha4))
            player.Active = "Tremor";
        if (Input.GetKeyUp(KeyCode.Alpha5))
            player.Active = "Confusion";
        if (Input.GetKeyUp(KeyCode.Alpha6))
            player.Passive = "Heatstroke";
        if (Input.GetKeyUp(KeyCode.Alpha7))
            player.Passive = "Cold Zone";
        if (Input.GetKeyUp(KeyCode.Alpha8))
            player.Passive = "Static Shock";
        if (Input.GetKeyUp(KeyCode.Alpha9))
            player.Passive = "Earth Barrier";
        if (Input.GetKeyUp(KeyCode.Alpha0))
            player.Passive = "Issuion Decoy";
        //Ammo cheat
        if (Input.GetKeyUp(KeyCode.F2))
        {
            player.hp = player.hpMax;
            player.ammoDict["bullet"] = player.ammoMaxDict["bulletMax"];
            player.ammoDict["shell"] = player.ammoMaxDict["shellMax"];
            player.ammoDict["expolsive"] = player.ammoDict["expolsiveMax"];
        }
    }
}
