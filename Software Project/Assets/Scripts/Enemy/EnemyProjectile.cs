using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed, normalSpeed, targetModfier = 1f;
    public float minDam, maxDam, minDamFinal, maxDamFinal;
    float minBuff, maxBuff;
    public bool confused, weak, noPath, bomb, ghost, bossAlpha, bossSigma;
    private Transform player;
    private Transform enemy;
    private Transform decoy;
    private Vector2 target;
    PlayerStat stat;
    Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start(){
        stat = GameObject.Find("Player").GetComponent<PlayerStat>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        enemy = GameObject.FindWithTag("Enemy").transform;
        rb2D = GetComponent<Rigidbody2D>();
        //modifies its postion for the Alpha boss
        List<float> newModifer = new List<float> { 1f, 1.25f, .75f };
        targetModfier = (bossAlpha) ? newModifer[Random.Range(0, 3)] : 1f;
        //Bullet set to target
        target = (confused) ? (enemy.transform.position - transform.position).normalized * speed : ((stat.pAbilDict["decoy"] && !weak) ?
            (GameObject.FindWithTag("Decoy").transform.position - transform.position).normalized * speed : 
            (player.transform.position * targetModfier - transform.position).normalized * speed);
        //Go to target pos
        if(!noPath)
            rb2D.velocity = new Vector2(target.x, target.y);
        // destroy after a few seconds
        Destroy(gameObject, (bossSigma) ? 2f : 1f);
        //set buff damage
        minBuff = minDam + (minDam/2);
        maxBuff = maxDam + (maxDam / 2);
    }
    private void Update()
    {
        minDamFinal = (!weak) ? ((!stat.enemyBuff) ? minDam : minBuff) : minDam;
        maxDamFinal = (!weak) ? ((!stat.enemyBuff) ? maxDam : maxBuff) : maxDam;
        speed = (stat.pAbilDict["cold"]) ? normalSpeed / 2 : normalSpeed;
    }
    public void OnTriggerEnter2D(Collider2D other){
        //Hit katana
        if (other.CompareTag("Melee") && stat.wep1Level == 3)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Room")
            Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
            rb2D.velocity = Vector3.zero;
    }
}
