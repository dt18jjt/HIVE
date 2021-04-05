using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreateScript : MonoBehaviour
{
    public GameObject[] Items;
    public int itemChance;
    PlayerStat player;
    // Start is called before the first frame update
    void Start()
    {
        itemChance = Random.Range(0, 6);
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
    }
    void itemDrop()
    {
        if (itemChance <= 4 && player.threatLV == 1)
            Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
        else if (itemChance <= 2 && player.threatLV == 2)
            Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
        else if (itemChance <= 1 && player.threatLV == 3)
            Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Hit by bullet
        if (other.CompareTag("Bullet"))
        {
            itemDrop();
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        //Hit by shell
        if (other.CompareTag("Shell"))
        {
            itemDrop();
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        //Hit by pyro
        if (other.CompareTag("Fire"))
        {
            itemDrop();
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        //Hit by laser
        if (other.CompareTag("Laser"))
        {
            itemDrop();
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        //Hit by melee
        if (other.CompareTag("Melee"))
        {
            itemDrop();
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        //Hit by explosive
        if (other.CompareTag("Bomb"))
        {
            itemDrop();
            Destroy(gameObject, 0.2f);
        }
        //Hit by cryo
        if (other.CompareTag("Freeze"))
        {
            itemDrop();
            Destroy(gameObject, 0.2f);
        }
        //Hit by electro
        if (other.CompareTag("Bolt"))
        {
            itemDrop();
            Destroy(gameObject, 0.2f);
        }
        //Hit by geo
        if (other.CompareTag("Tremor"))
        {
            itemDrop();
            Destroy(gameObject, 0.2f);
        }
        //Hit by hypno
        if (other.CompareTag("Confuse"))
        {
            itemDrop();
            Destroy(gameObject, 0.2f);
        }
        if (other.CompareTag("Pulse"))
        {
            itemDrop();
            Debug.Log(itemChance);
            Destroy(gameObject, 0.2f);
        }
        //When collides with another crate to prevent overlapping
        if (other.CompareTag("Box"))
            Destroy(other.gameObject);
        //When collides with item to prevent overlapping
        if (other.CompareTag("BAmmo"))
            Destroy(gameObject);
        if (other.CompareTag("ShAmmo"))
            Destroy(gameObject);
        if (other.CompareTag("EAmmo"))
            Destroy(gameObject);
        if (other.CompareTag("Health"))
            Destroy(gameObject);
        if (other.CompareTag("BWep0"))
            Destroy(gameObject);
        if (other.CompareTag("SWep0"))
            Destroy(gameObject);
        if (other.CompareTag("EWep0"))
            Destroy(gameObject);
        if (other.CompareTag("LWep0"))
            Destroy(gameObject);
        if (other.CompareTag("MWep0"))
            Destroy(gameObject);
    }
}
