using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed, normalSpeed, targetModfier = 1f;
    public int minDam, maxDam, minDamFinal, maxDamFinal;
    int minBuff, maxBuff;
    public bool confused, weak, noPath, bomb, ghost, bossAlpha, bossSigma;
    private Transform player;
    private Transform enemy;
    private Transform decoy;
    private Vector2 target;
    PlayerStat stat;
    Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start(){
        List<float> newModifer = new List<float> { 1f, 1.25f, .75f };
        targetModfier = (bossAlpha) ? newModifer[Random.Range(0, 3)] : 1f;
        stat = GameObject.Find("Player").GetComponent<PlayerStat>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        enemy = GameObject.FindWithTag("Enemy").transform;
        rb2D = GetComponent<Rigidbody2D>();
        //Bullet set to target
        target = (confused) ? (enemy.transform.position - transform.position).normalized * speed : ((!stat.pAbilDict["decoy"]) ?
            (player.transform.position * targetModfier - transform.position).normalized * speed : 
            (GameObject.FindWithTag("Decoy").transform.position - transform.position).normalized * speed);
        if(!noPath)
            rb2D.velocity = new Vector2(target.x, target.y);
        Destroy(gameObject, (bossSigma) ? 2f : 1f);
        minBuff = minDam + (minDam/2);
        maxBuff = maxDam + (maxDam / 2);
    }
    private void Update()
    {
        minDamFinal = (!weak) ? ((!stat.enemyBuff) ? minDam : minBuff) : minDam;
        maxDamFinal = (!weak) ? ((!stat.enemyBuff) ? maxDam : maxBuff) : maxDam;
        speed = (stat.pAbilDict["cold"]) ? normalSpeed / 2 : normalSpeed;
    }
    private void OnTriggerEnter2D(Collider2D other){
        //Hits player
        if(other.name == "Player" && !bomb && !ghost){
            if(!stat.pAbilDict["earth"] && !confused)
                stat.Damage(Random.Range(minDamFinal, maxDamFinal));
            Destroy(gameObject);
        }
        //Hit Melee
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
