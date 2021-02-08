using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
    public int hp;
    public int itemChance;
    public GameObject[] Items;
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        hp = Random.Range(2, 5);
        Destroy(gameObject, 3f);
        itemChance = Random.Range(0, 6);
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            if (itemChance <= 3)
                Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            hit.GetComponent<ParticleSystem>().Play();
            Destroy(hit, 1f);
            hp -= 1;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Fire"))
        {
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            hit.GetComponent<ParticleSystem>().Play();
            Destroy(hit, 1f);
            hp -= 1;
        }
        if (other.CompareTag("Freeze"))
        {
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            hit.GetComponent<ParticleSystem>().Play();
            Destroy(hit, 1f);
            hp -= 1;
            Destroy(other.gameObject);
        }
        if (other.name == "Pulse")
        {
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            hit.GetComponent<ParticleSystem>().Play();
            Destroy(hit, 1f);
            hp -= 1;
        }
    }
}
