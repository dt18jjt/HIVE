using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public bool confused;
    private Transform player;
    private Transform enemy;
    private Vector2 target;
    PlayerStat stat;
    Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start(){
        stat = GameObject.Find("Player").GetComponent<PlayerStat>();
        player = GameObject.Find("Player").transform;
        enemy = GameObject.FindWithTag("Enemy").transform;
        //target = new Vector2(player.position.x, player.position.y);
        rb2D = GetComponent<Rigidbody2D>();
        if(!confused)
            target = (player.transform.position - transform.position).normalized * speed;
        if (confused)
            target = (enemy.transform.position - transform.position).normalized * speed;
        rb2D.velocity = new Vector2(target.x, target.y);
        Destroy(gameObject, 1f);

    }
    
    private void OnTriggerEnter2D(Collider2D other){
        if(other.name == "Player"){
            if(!stat.pAbilDict["earth"] && !confused)
                stat.Damage(Random.Range(10,15));
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Room")
            Destroy(gameObject);
    }
}
