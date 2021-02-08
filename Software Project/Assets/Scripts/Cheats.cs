using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    public EnemyFollow[] enemy;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemy = new EnemyFollow[enemies.Length];
        for (int i = 0; i < enemies.Length; ++i)
            enemy[i] = enemies[i].GetComponent<EnemyFollow>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            
        }
            
    }
}
