using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
    public int hp;
    public int itemChance;
    public GameObject BP;
    public GameObject hitEffect;
    PlayerStat player;
    // Start is called before the first frame update
    void Start()
    {
        hp = Random.Range(2, 5); //hit needed to destroy
        Destroy(gameObject, 3f); //destroyd after seconds
        itemChance = Random.Range(0, 10); //chance of bp spawning
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            //Spawn a bio point
            if (itemChance <= 5 && PlayerPrefs.GetInt("Threat Level") == 1)
                Instantiate(BP, transform.position, Quaternion.identity);
            else if (itemChance <= 3 && PlayerPrefs.GetInt("Threat Level") == 2)
                Instantiate(BP, transform.position, Quaternion.identity);
            else if (itemChance <= 1 && PlayerPrefs.GetInt("Threat Level") == 3)
                Instantiate(BP, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    void dam()
    {
        GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
        hit.GetComponent<ParticleSystem>().Play();
        Destroy(hit, 1f);
        hp -= 1;
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Hit by bullet
        if (other.CompareTag("Bullet"))
        {
            dam();
            Destroy(other.gameObject);
        }
        //Hit by shell
        if (other.CompareTag("Shell"))
        {
            dam();
            Destroy(other.gameObject);
        }
        //Hit by melee
        if (other.CompareTag("Melee"))
        {
            dam();
            Destroy(other.gameObject);
        }
        //Hit by laser
        if (other.CompareTag("Laser"))
        {
            dam();
            Destroy(other.gameObject);
        }
        //Hit by explosive
        if (other.CompareTag("Bomb"))
        {
            dam();
        }
        //Hit by pyro
        if (other.CompareTag("Fire"))
        {
            dam();
            Destroy(other.gameObject);
        }
        //Hit by cryo
        if (other.CompareTag("Freeze"))
        {
            dam();
            Destroy(other.gameObject);
        }
        //Hit by electro
        if (other.CompareTag("Bolt"))
        {
            dam();
            Destroy(other.gameObject);
        }
        //Hit by geo
        if (other.CompareTag("Tremor"))
        {
            dam();
            Destroy(other.gameObject);
        }
        if (other.name == "Pulse")
        {
            dam();
            Destroy(other.gameObject);
        }
    }
}
