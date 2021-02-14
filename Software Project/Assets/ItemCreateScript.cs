using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreateScript : MonoBehaviour
{
    public GameObject[] Items;
    public int itemChance;
    // Start is called before the first frame update
    void Start()
    {
        itemChance = Random.Range(0, 6);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if(itemChance <= 4)
                Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
            Debug.Log(itemChance);
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        if (other.CompareTag("Bullet"))
        {
            if (itemChance <= 4)
                Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
            Debug.Log(itemChance);
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        if (other.CompareTag("Shell"))
        {
            if (itemChance <= 4)
                Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
            Debug.Log(itemChance);
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        if (other.CompareTag("Fire"))
        {
            if (itemChance <= 4)
                Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
            Debug.Log(itemChance);
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        if (other.CompareTag("Laser"))
        {
            if (itemChance <= 4)
                Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
            Debug.Log(itemChance);
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        if (other.CompareTag("Melee"))
        {
            if (itemChance <= 4)
                Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
            Debug.Log(itemChance);
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
        if (other.CompareTag("Pulse"))
        {
            if (itemChance <= 4)
                Instantiate(Items[Random.Range(0, Items.Length)], transform.position, Quaternion.identity);
            Debug.Log(itemChance);
            Destroy(gameObject, 0.2f);
        }
        if (other.CompareTag("Box"))
            Destroy(other.gameObject);
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
