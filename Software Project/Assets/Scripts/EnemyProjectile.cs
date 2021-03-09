using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public int minDam, maxDam, minDamFinal, maxDamFinal;
    int minBuff, maxBuff;
    public bool confused, weak;
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
        //Bullet set to target
        target = (confused) ? (enemy.transform.position - transform.position).normalized * speed : ((!stat.pAbilDict["decoy"]) ?
            (player.transform.position - transform.position).normalized * speed : 
            (GameObject.FindWithTag("Decoy").transform.position - transform.position).normalized * speed);
        rb2D.velocity = new Vector2(target.x, target.y);
        Destroy(gameObject, 1f);
        minBuff = minDam + (minDam/2);
        maxBuff = maxDam + (maxDam / 2);
    }
    private void Update()
    {
        minDamFinal = (!weak) ? ((!stat.enemyBuff) ? minDam : minBuff) : minDam;
        maxDamFinal = (!weak) ? ((!stat.enemyBuff) ? maxDam : maxBuff) : maxDam;
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.name == "Player"){
            if(!stat.pAbilDict["earth"] && !confused)
                stat.Damage(Random.Range(minDamFinal, maxDamFinal));
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Room")
            Destroy(gameObject);
    }
}
