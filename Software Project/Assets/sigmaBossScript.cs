using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sigmaBossScript : MonoBehaviour
{
    public int hp = 20, moveAngle = 0;
    public float attackCooldown, startAtkCooldown, confuseCooldown = 0f, frozenCooldown;
    float heatTimer = 1f, coldTimer = 1f;
    public bool ranged, frozen = false;
    public GameObject splitProjectile, hitEffect;
    public Transform cEnemy;
    private Transform target;
    public Color normalColor, frozenColor, confuseColor;
    Vector2 randDirection, randMovement;
    PlayerStat player;
    PlayerMovement playerMove;
    Rigidbody2D rb;
    Log log;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        log = GameObject.Find("Global").GetComponent<Log>();
        player.cEmenies.Add(gameObject.transform);
 
    }
    // Update is called once per frame
    void Update()
    {
        //Kill Cheat
        if (Input.GetKey(KeyCode.F1))
        {
            hp = 0;
        }
        heatstroke();
        coldZone();
        staticShock();
        //Death
        if (hp <= 0)
        {
            Destroy(gameObject);
            player.cEmenies.Remove(gameObject.transform);
        }
        if (confuseCooldown <= 0)
            gameObject.tag = "Enemy";
        if (frozenCooldown <= 0 && confuseCooldown <= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = normalColor;
        }
        else if (frozenCooldown > 0)
        {
            frozenCooldown -= Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = frozenColor;
        }
        //target change
        if (confuseCooldown <= 0)
            target = (player.pAbilDict["decoy"]) ? GameObject.FindGameObjectWithTag("Decoy").GetComponent<Transform>() :
                GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        else if (confuseCooldown > 0)
        {
            while (player.cEmenies.Count > 1 && cEnemy == null)
                cEnemy = GameObject.FindWithTag("Enemy").GetComponent<Transform>();
            transform.gameObject.tag = (player.cEmenies.Count <= 1) ? "Enemy" : "Player";
            confuseCooldown -= Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = confuseColor;
            target = (player.cEmenies.Count <= 1) ? gameObject.transform : cEnemy;
        }
        enemyRangeAtk();
    }
    void enemyRangeAtk()
    {

        // Range attack after cooldown reaches 0
        if (attackCooldown <= 0)
        {
            splitSpawn(12);
            // Reset projectile
            attackCooldown = startAtkCooldown;
        }
        else
            attackCooldown -= Time.deltaTime;
    }
    void heatstroke()
    {
        if (player.pAbilDict["heat"])
        {
            if (heatTimer > 0)
                heatTimer -= Time.deltaTime;
            else if (heatTimer <= 0)
            {
                GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
                hit.GetComponent<ParticleSystem>().Play();
                heatTimer = 1f;
                hp -= 3;
            }
        }
        if (!player.pAbilDict["heat"])
            heatTimer = 2f;
    }
    void coldZone()
    {
        if (player.pAbilDict["cold"])
        {
            if (coldTimer > 0)
                coldTimer -= Time.deltaTime;
            else if (coldTimer <= 0)
                coldTimer = 2f;
        }
        if (!player.pAbilDict["cold"])
        {
            coldTimer = 1f;
        }
    }
    void staticShock()
    {
        if (player.pAbilDict["shock"] && player.shockDam && player.shockCoolDown <= 0)
        {
            player.shockCoolDown = 0.2f;
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            hit.GetComponent<ParticleSystem>().Play();
            hp -= 10;
        }


    }
    public void Damage(int dam)
    {
        GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
        hit.GetComponent<ParticleSystem>().Play();
        Destroy(hit, 1f);
        hp -= dam;
    }
    void splitSpawn(int numberOfProjectiles)
    {
        float radius, splitSpeed = 80f;
        radius = GetComponent<CircleCollider2D>().radius;
        //starting point for projectiles
        Vector2 startPoint = gameObject.transform.position;
        // angle difference between projectiles
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;
        //spawning projectiles
        for (int i = 0; i <= numberOfProjectiles - 1; i++)
        {

            float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
            Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * splitSpeed;

            var proj = Instantiate(splitProjectile, startPoint, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity =
                new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            angle += angleStep;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Hit by bullet object
        if (other.CompareTag("Bullet"))
        {
            Damage(player.damDict["bulletDam"]);
            if (player.wep1Level != 2)
                Destroy(other.gameObject);
            log.bulletHit++;
            Debug.Log("Bullet:" + log.bulletHit);
        }
        //Hit by shell object
        if (other.CompareTag("Shell"))
        {
            Damage(player.damDict["shellDam"]);
            Destroy(other.gameObject);
            log.shellHit++;
            Debug.Log("Shell:" + log.shellHit);
        }
        //Hit by laser object
        if (other.CompareTag("Laser"))
        {
            Damage(player.damDict["laserDam"]);
            Destroy(other.gameObject);
            log.laserHit++;
            Debug.Log("Laser:" + log.laserHit);
        }
        //Hit by explosive object
        if (other.CompareTag("Bomb"))
        {
            Damage(player.damDict["explosiveDam"]);
            log.explosiveHit++;
            Debug.Log("explosive:" + log.shellHit);
        }
        //Hit by melee object
        if (other.CompareTag("Melee"))
        {
            Damage(player.damDict["meleeDam"]);
            log.meleeHit++;
            Debug.Log("Melee:" + log.meleeHit);
        }
        //Hit by active pyro
        if (other.CompareTag("Fire"))
        {
            Damage(player.damDict["fireDam"] / 2);
            log.pyroHit++;
            Debug.Log("Pyro:" + log.pyroHit);
        }
        //Hit by active cryo
        if (other.CompareTag("Freeze"))
        {
            Damage(player.damDict["freezeDam"]);
            frozenCooldown =  1f;
            Destroy(other.gameObject);
            log.cryoHit++;
            Debug.Log("Cryo:" + log.cryoHit);
        }
        //if (other.CompareTag("Pulse"))
        //{
        //    Damage(player.damDict["pulseDam"]);
        //}
        //Hit by active electro
        if (other.CompareTag("Bolt"))
        {
            Damage(player.damDict["boltDam"] / 2);
            log.electroHit++;
            Debug.Log("Electro:" + log.electroHit);
        }
        //Hit by active geo
        if (other.CompareTag("Tremor"))
        {
            Damage(player.damDict["tremorDam"]);
            log.geoHit++;
            Debug.Log("Geo:" + log.geoHit);
        }
        //Hit by active hypno 
        if (other.CompareTag("Confuse"))
        {
            confuseCooldown = 3f;
            Destroy(other.gameObject);
            log.hypnoHit++;
            Debug.Log("Shell:" + log.hypnoHit);
        }
        //Hit by projectiles from confused enemy
        if (other.CompareTag("CBullet") && gameObject.CompareTag("Enemy"))
        {
            Damage(player.damDict["confuseDam"]);
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            rb.velocity = Vector3.zero;

        }
    }
}
