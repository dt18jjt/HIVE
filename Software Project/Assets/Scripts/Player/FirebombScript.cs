using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebombScript : MonoBehaviour
{
    public GameObject Area;
    public bool enemy, grenade, mine; // enemy = if fired from enemy
    camShake shake;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        if (grenade)
            Invoke("detonate", 1f);
        if (mine)
            Invoke("deploy", 0.5f);
        rb = GetComponent<Rigidbody2D>();
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<camShake>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void detonate()
    {
        GameObject a = Instantiate(Area, transform.position, Quaternion.identity) as GameObject;
        Destroy(a, 0.3f);
        Destroy(gameObject);
        shake.shakeDuration = 0.5f;

    }
    void deploy()
    {
        rb.velocity = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !enemy)
            detonate();
        if (other.CompareTag("Player") && enemy)
            detonate();
        if (other.CompareTag("Wall") && !mine)
            detonate();
        if (other.CompareTag("Door") && !mine)
            detonate();
        if (other.CompareTag("Blocked") && !mine)
            detonate();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
            rb.velocity = Vector2.zero;
    }
}
