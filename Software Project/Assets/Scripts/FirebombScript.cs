using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebombScript : MonoBehaviour
{
    public GameObject Area;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject a = Instantiate(Area, transform.position, Quaternion.identity) as GameObject;
            Destroy(a, 0.3f);
            Destroy(gameObject);
        }
        if (other.CompareTag("Wall"))
        {
            GameObject a = Instantiate(Area, transform.position, Quaternion.identity) as GameObject;
            Destroy(a, 0.3f);
            Destroy(gameObject);
        }
        if (other.CompareTag("Door"))
        {
            GameObject a = Instantiate(Area, transform.position, Quaternion.identity) as GameObject;
            Destroy(a, 0.3f);
            Destroy(gameObject);
        }
        if (other.CompareTag("Blocked"))
        {
            GameObject a = Instantiate(Area, transform.position, Quaternion.identity) as GameObject;
            Destroy(a, 0.3f);
            Destroy(gameObject);
        }
    }
}
