using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boltAreaScript : MonoBehaviour
{
    EnemyFollow enemy;
    PlayerStat player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject.GetComponent<EnemyFollow>();
            enemy.Damage((enemy.Electro) ? player.damDict["boltDam"] / 2 : player.damDict["boltDam"]);

        }
    }
}
