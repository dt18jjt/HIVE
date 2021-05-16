using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebombScript : MonoBehaviour
{
    public GameObject Area;
    public bool enemy, grenade, mine, seeker; // enemy = if fired from enemy
    camShake shake;
    Rigidbody2D rb;
    Transform enemyPos;
    private Vector2 target;
    float seekerSpeed = 80f;
    PlayerStat player;
    GameObject pObj;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        pObj = GameObject.Find("Player");
        //grenade action
        if (grenade)
            Invoke("detonate", 1f);
        //mine action
        if (mine)
            Invoke("deploy", 0.5f);
        rb = GetComponent<Rigidbody2D>();
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<camShake>();
        //Heat seeker
        if (seeker)
        {
            enemyPos = GameObject.FindWithTag("Enemy").GetComponent<Transform>();
            //if a enemy is found
            if (enemyPos != null)
            {
                target = (enemyPos.position - transform.position).normalized* seekerSpeed;
                //move to enemy
                rb.velocity = new Vector2(target.x, target.y);
                //angle facing the enemy
                float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
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
        pObj.GetComponent<AudioSource>().PlayOneShot(player.bombSound);

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
