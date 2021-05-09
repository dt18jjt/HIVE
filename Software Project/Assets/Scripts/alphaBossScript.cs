using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphaBossScript : MonoBehaviour
{
    public int hp = 20, moveAngle = 0;
    public float speed, normalSpeed, coldSpeed, stoppingDistance, retreatDistance, attackCooldown, startAtkCooldown,
        moveCooldown, startMvCooldown, frozenCooldown, tremorCooldown = 0f, confuseCooldown = 0f, avoidCooldown, shockWaveCooldown, startSWCooldown;
    float heatTimer = 1f, coldTimer = 1f;
    public bool ranged, frozen = false;
    bool blastSpawned = false;
    //Enemy Types
    public GameObject hitEffect, bombProjectile, warningArea, blastArea;
    public Transform cEnemy;
    private Transform target;
    public Color normalColor, frozenColor, confuseColor;
    Vector2 randDirection, randMovement;
    PlayerStat player;
    PlayerMovement playerMove;
    RoomTemplates templates;
    Rigidbody2D rb;
    Log log;
    camShake shake;
    public GameObject[] deathDrop;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        log = GameObject.Find("Global").GetComponent<Log>();
        player.cEmenies.Add(gameObject.transform);
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<camShake>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }
    // Update is called once per frame
    void Update()
    {
        //Kill Cheat
        if (Input.GetKey(KeyCode.F1))
        {
            hp = 0;
        }
        movement();
        heatstroke();
        coldZone();
        staticShock();
        tremorKnockback();
        //Death
        if (hp <= 0)
        {
            Instantiate(deathDrop[Random.Range(0, deathDrop.Length)], transform.position, Quaternion.identity);
            Destroy(gameObject);
            player.cEmenies.Remove(gameObject.transform);
        }
        if (confuseCooldown <= 0)
            gameObject.tag = "Enemy";
        // Movement stoppage cooldown
        if (moveCooldown > 0)
            moveCooldown -= Time.deltaTime;
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
            confuseCooldown -= Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = confuseColor;
        }
        //Avoid movement cooldown
        if (avoidCooldown > 0)
            avoidCooldown -= Time.deltaTime;
        //Alpha shockwave blast
        if (shockWaveCooldown <= 0 && !blastSpawned)
        {
            blastSpawned = true;
            StartCoroutine(shockWave());
        }
            
        warningArea.SetActive(((Vector2.Distance(transform.position, target.position) < retreatDistance) && !blastSpawned) ? true : false);
    }
    private void FixedUpdate()
    {
        //if(knockedBack)
        //    StartCoroutine(tremorKnockback(0.5f, 50f));
    }
    void movement()
    {
        //Close Range 
        if (!ranged && frozenCooldown <= 0 && tremorCooldown <= 0)
        {
            if (Vector2.Distance(transform.position, target.position) > stoppingDistance && moveCooldown <= 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                attackCooldown = startAtkCooldown;
                //StartCoroutine(stopTimer());
            }
            else if (Vector2.Distance(transform.position, target.position) <= stoppingDistance)
            {
                attackCooldown -= Time.deltaTime;
                moveCooldown = startMvCooldown;
                if (attackCooldown <= 0 && player.hp > 0 && Vector2.Distance(transform.position, target.position) <= stoppingDistance)
                    enemyCloseAtk();
            }
        }
        //Long Range
        if (ranged && frozenCooldown <= 0 && tremorCooldown <= 0)
        {
            if (Vector2.Distance(transform.position, target.position) > stoppingDistance && moveCooldown <= 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                shockWaveCooldown = startSWCooldown;
            }
            else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance)
            {
                if (avoidCooldown <= 0)
                {
                    randDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
                    avoidCooldown = 2f;
                }
                randMovement = randDirection * speed;
                transform.position = new Vector2(transform.position.x + (randMovement.x * Time.deltaTime), transform.position.y + (randMovement.y * Time.deltaTime));
            }
            else if (Vector2.Distance(transform.position, target.position) < retreatDistance)
            {
                transform.position = this.transform.position;
                //if (moveCooldown <= 0)
                //    transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
                Debug.Log("Close");
                shockWaveCooldown -= Time.deltaTime;
                
            }
            if(shockWaveCooldown > 0)
                enemyRangeAtk();
        }
    }
    void enemyCloseAtk()
    {
        if (!player.pAbilDict["decoy"] && confuseCooldown <= 0)
        {
            player.Damage((player.enemyBuff) ? Random.Range(15, 30) : Random.Range(10, 20));
        }
        else if (confuseCooldown > 0)
            Damage(10);
        attackCooldown = startAtkCooldown;
    }
    void enemyRangeAtk()
    {
        // Range attack after cooldown reaches 0
        if (attackCooldown <= 0 && moveCooldown <= 0)
        {
            // Spawn normal projectile
            Instantiate(bombProjectile, transform.position, Quaternion.identity);
            // Reset projectile
            attackCooldown = startAtkCooldown;
            moveCooldown = startMvCooldown;
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
            speed = coldSpeed;
            if (coldTimer > 0)
                coldTimer -= Time.deltaTime;
            else if (coldTimer <= 0)
                coldTimer = 2f;
        }
        if (!player.pAbilDict["cold"])
        {
            coldTimer = 1f;
            speed = normalSpeed;
        }
    }
    void staticShock()
    {
        if (player.pAbilDict["shock"] && player.shockDam && player.shockCoolDown <= 0)
        {
            player.shockCoolDown = 0.2f;
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            hit.GetComponent<ParticleSystem>().Play();
            hp -= 5;
        }


    }
    void Damage(int dam)
    {
        GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
        hit.GetComponent<ParticleSystem>().Play();
        Destroy(hit, 1f);
        hp -= dam;
        //player.threatGauge += 5;
    }
    void tremorKnockback()
    {
        if (tremorCooldown > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, -target.position, 100 * Time.deltaTime);
            tremorCooldown -= Time.deltaTime;
            moveCooldown = startMvCooldown;
            attackCooldown = startAtkCooldown;
        }
    }
    IEnumerator shockWave()
    {
        GameObject b = Instantiate(blastArea, transform.position, Quaternion.identity) as GameObject;
        Destroy(b, .5f);
        shake.shakeDuration = .5f;
        yield return new WaitForSeconds(.5f);
        shockWaveCooldown = startSWCooldown;
        blastSpawned = false;
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Hit by bullet object
        if (other.CompareTag("Bullet"))
        {
            Damage(player.damDict["bulletDam"]);
            if (player.wep1Level != 2)
                Destroy(other.gameObject);
            //log.bulletHit++;
            //Debug.Log("Bullet:" + log.bulletHit);
        }
        //Hit by shell object
        if (other.CompareTag("Shell"))
        {
            Damage(player.damDict["shellDam"]);
            Destroy(other.gameObject);
            //log.shellHit++;
            //Debug.Log("Shell:" + log.shellHit);
        }
        //Hit by laser object
        if (other.CompareTag("Laser"))
        {
            Damage(player.damDict["laserDam"]);
            Destroy(other.gameObject);
            //log.laserHit++;
            //Debug.Log("Laser:" + log.laserHit);
        }
        //Hit by explosive object
        if (other.CompareTag("Bomb"))
        {
            Damage(player.damDict["explosiveDam"]);
            //log.explosiveHit++;
            //Debug.Log("explosive:" + log.shellHit);
        }
        //Hit by melee object
        if (other.CompareTag("Melee"))
        {
            Damage(player.damDict["meleeDam"]);
            //log.meleeHit++;
            //Debug.Log("Melee:" + log.meleeHit);
        }
        //Hit by active pyro
        if (other.CompareTag("Fire"))
        {
            Damage(player.damDict["fireDam"] / 2);
            //log.pyroHit++;
            //Debug.Log("Pyro:" + log.pyroHit);
        }
        //Hit by active cryo
        if (other.CompareTag("Freeze"))
        {
            Damage(player.damDict["freezeDam"]);
            frozenCooldown = 0.5f;
            Destroy(other.gameObject);
            //log.cryoHit++;
            //Debug.Log("Cryo:" + log.cryoHit);
        }
        //Hit by active electro
        if (other.CompareTag("Bolt"))
        {
            Damage(player.damDict["boltDam"] / 2);
            //log.electroHit++;
            //Debug.Log("Electro:" + log.electroHit);
        }
        //Hit by active geo
        if (other.CompareTag("Tremor"))
        {
            Damage(player.damDict["tremorDam"]);
            tremorCooldown = 0.5f;
            //log.geoHit++;
            //Debug.Log("Geo:" + log.geoHit);
        }
        //Hit by active hypno 
        if (other.CompareTag("Confuse"))
        {
            confuseCooldown = 2f;
            Destroy(other.gameObject);
            //log.hypnoHit++;
            //Debug.Log("Shell:" + log.hypnoHit);
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
