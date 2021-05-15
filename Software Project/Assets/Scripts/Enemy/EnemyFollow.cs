using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public int hp = 20;
    public float speed, normalSpeed, coldSpeed, stoppingDistance, retreatDistance, attackCooldown, startAtkCooldown, 
        moveCooldown, startMvCooldown, frozenCooldown, tremorCooldown = 0f, confuseCooldown = 0f, avoidCooldown, disappearCooldown = 0f, adsorbCooldown = 0f,
        ghostCooldown = 0.5f;
    float heatTimer = 1f, coldTimer = 1f;
    public bool ranged, frozen = false, bpSpawn = false, Avoid;
    //Enemy Types
    public bool Pyro, Cryo, Geo, Electro, Hypno, Explosive, Laser, Bullet, Shell, Melee;
    public GameObject projectile, confusionProjectile, burnProjectile, sporeProjectile, weakProjectile, splitProjectile, BP, Corpse, hitEffect, adsorbEffect,
        bombProjectile, ghostProjectile;
    public Transform cEnemy;
    private Transform target;
    SpriteRenderer sprite;
    public Color normalColor, frozenColor, confuseColor;
    Vector2 randDirection, randMovement;
    PlayerStat player;
    PlayerMovement playerMove;
    Rigidbody2D rb;
    Log log;
    RoomTypes room;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        log = GameObject.Find("Global").GetComponent<Log>();
        player.cEmenies.Add(gameObject.transform);
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (Hypno)
            player.buffNum++;
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
        if (hp <= 0){
            //spawn corpse
            Instantiate(Corpse, transform.position, Quaternion.identity);
            //spawn bio-point
            Instantiate(BP, transform.position * 1.02f, Quaternion.identity);
            Destroy(gameObject);
            player.cEmenies.Remove(gameObject.transform);
            room.enemyCount--;
            if (Hypno)
                player.buffNum--;
            if (player.killCoolDown > 0)
            {
                log.quickKill++;
                Debug.Log("Quick Kill: " + log.quickKill);
            }
            player.killCoolDown = 0.5f;
        }
        if (confuseCooldown <= 0)
            gameObject.tag = "Enemy";
        // Movement stoppage cooldown
        if (moveCooldown > 0)
            moveCooldown -= Time.deltaTime;
        if (frozenCooldown <= 0 && confuseCooldown <= 0)
        {
            sprite.color = normalColor;
        }
        else if (frozenCooldown > 0)
        {
            frozenCooldown -= Time.deltaTime;
            sprite.color = frozenColor;
        }
        //target change
        if (confuseCooldown <= 0)
            target = (player.pAbilDict["decoy"]) ? GameObject.FindGameObjectWithTag("Decoy").GetComponent<Transform>():
                GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        else if(confuseCooldown > 0)
        {
            while (player.cEmenies.Count > 1 && cEnemy == null)
                cEnemy = GameObject.FindWithTag("Enemy").GetComponent<Transform>();
            transform.gameObject.tag = (player.cEmenies.Count <= 1) ? "Enemy" : "Player";
            confuseCooldown -= Time.deltaTime;
            sprite.color = confuseColor;
            target = (player.cEmenies.Count <= 1) ? gameObject.transform : cEnemy;
        }
        //Laser Behaviour change
        if (Laser && hp < 20 && ranged)
        {
            ranged = false;
            retreatDistance = 25;
            stoppingDistance = 15;
            attackCooldown = 0.5f;
            startAtkCooldown = 0.5f;
            hp += 20;
        }
        //Avoid movement cooldown
        if(avoidCooldown > 0)
            avoidCooldown -= Time.deltaTime;
        // Adsorb bullets cooldown
        if (adsorbCooldown > 0)
            adsorbCooldown -= Time.deltaTime;
        //adsord effect active
        if(Bullet)
            adsorbEffect.SetActive((adsorbCooldown >  0) ? true : false);
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
            }
            else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance)
            {
                // transform.position = this.transform.position;
                if(avoidCooldown <= 0)
                {
                    randDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
                    avoidCooldown = 2f;
                }
                randMovement = randDirection * speed;
                transform.position = new Vector2 (transform.position.x + (randMovement.x * Time.deltaTime), transform.position.y + (randMovement.y * Time.deltaTime));
            }
            else if (Vector2.Distance(transform.position, target.position) < retreatDistance)
            {
                if(moveCooldown <= 0)
                    transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
                if (Melee)
                {
                    Color tmp = GetComponent<SpriteRenderer>().color;
                    tmp.a = 0f;
                    GetComponent<SpriteRenderer>().color = tmp;
                    ghostCooldown -= Time.deltaTime;
                }
            }
            if(adsorbCooldown <= 0)
            {
                enemyRangeAtk();
                if(Bullet)
                    StartCoroutine(adsorb());
            }
        }
    }
    void enemyCloseAtk(){
        if (!player.pAbilDict["decoy"] && confuseCooldown <= 0)
        {
            player.Damage((player.enemyBuff) ?Random.Range(15, 30) : Random.Range(10, 20));
        }
        else if (confuseCooldown > 0)
            target.GetComponent<EnemyFollow>().Damage(10);
        if (Cryo && playerMove.slowCoolDown <= 0)
            playerMove.slowCoolDown = 1f;
        attackCooldown = startAtkCooldown;
    }
    void enemyRangeAtk(){
        // Range attack after cooldown reaches 0
        if (attackCooldown <= 0 && moveCooldown <= 0){
            // Spawn confusion projectile
            if (confuseCooldown > 0)
                Instantiate(confusionProjectile, transform.position, Quaternion.identity);
            // Spawn burn projectile
            else if (Pyro)
                Instantiate(burnProjectile, transform.position, Quaternion.identity);
            // Spawn tangle projectile
            else if (Geo)
                Instantiate(sporeProjectile, transform.position, Quaternion.identity);
            // Spawn weak projectile
            else if (Hypno)
                Instantiate(weakProjectile, transform.position, Quaternion.identity);
            //Spawn Ghost projectile
            else if (Melee && ghostCooldown <= 0)
                StartCoroutine(Disappear());
            // Spawn normal projectile
            else
                Instantiate(projectile, transform.position, Quaternion.identity);
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
        if (player.pAbilDict["shock"] && player.shockDam && player.shockCoolDown <= 0){
            player.shockCoolDown = .2f;
        }
        
       
    }
    public void Damage(int dam)
    {
        GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
        hit.GetComponent<ParticleSystem>().Play();
        Destroy(hit, 1f);
        if(adsorbCooldown <= 0)
            hp -= dam;
        player.threatGauge += 5;
        if (Electro){
            normalSpeed += 10;
            coldSpeed += 10;
        }
        if (Shell)
            splitSpawn(4);
        if (adsorbCooldown > 0)
        {
            Instantiate(bombProjectile, transform.position, Quaternion.identity);
        }

    }
    void tremorKnockback()
    {
        if(tremorCooldown > 0)
        {
            transform.position = (Geo) ? Vector2.MoveTowards(transform.position, -target.position, 50 * Time.deltaTime) :
                Vector2.MoveTowards(transform.position, -target.position, 100 * Time.deltaTime);
            tremorCooldown -= Time.deltaTime;
            moveCooldown = startMvCooldown;
            attackCooldown = startAtkCooldown;
        }
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
    //pause in close range enemy movement
    public IEnumerator stopTimer()
    {
        yield return new WaitForSeconds(Random.Range(2f, 3.1f));
        moveCooldown = 0.2f;
    }
    public IEnumerator Disappear()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        // create projectile
        GameObject g = Instantiate(ghostProjectile, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        // set postion to projectile
        transform.position = g.transform.position;
        tmp.a = 1f;
        GetComponent<SpriteRenderer>().color = tmp;
        //reset ghost cooldown
        ghostCooldown = 0.5f;
    }
    public IEnumerator adsorb()
    {
        yield return new WaitForSeconds(3f);
        adsorbCooldown = 1.0f;
    }
    private void OnTriggerEnter2D(Collider2D other){
        //Get room script
        if (other.CompareTag("Room"))
        {
            room = other.gameObject.GetComponent<RoomTypes>();
        }
        //Hit by bullet object
        if (other.CompareTag("Bullet")){
            Damage(player.damDict["bulletDam"]);
            if(player.wep1Level != 2)
                Destroy(other.gameObject);
            if(!log.del009)
                log.playerAction["bulletHit"]++;
            //log.bulletHit++;
            Debug.Log("Bullet:" + log.playerAction["bulletHit"]);
        }
        //Hit by shell object
        if (other.CompareTag("Shell"))
        {
            Damage(player.damDict["shellDam"]);
            Destroy(other.gameObject);
            //log.shellHit++;
            if (!log.del010)
                log.playerAction["shellHit"]++;
            Debug.Log("Shell:" + log.playerAction["shellHit"]);
        }
        //Hit by laser object
        if (other.CompareTag("Laser"))
        {
            Damage(player.damDict["laserDam"]);
            Destroy(other.gameObject);
            if (!log.del012)
                log.playerAction["laserHit"]++;
            //log.laserHit++;
            Debug.Log("Laser:" + log.playerAction["laserHit"]);
        }
        //Hit by explosive object
        if (other.CompareTag("Bomb"))
        {
            if (!Explosive)
            {
                Damage(player.damDict["explosiveDam"]);
                if (!log.del011)
                    log.playerAction["explosiveHit"]++;
                //log.explosiveHit++;
                Debug.Log("explosive:" + log.playerAction["explosiveHit"]);
            }
        }
        //Hit by melee object
        if (other.CompareTag("Melee"))
        {
            Damage(player.damDict["meleeDam"]);
            if (!log.del008)
                log.playerAction["meleeHit"]++;
            //log.meleeHit++;
            Debug.Log("Melee:" + log.playerAction["meleeHit"]);
        }
        //Hit by active pyro
        if (other.CompareTag("Fire"))
        {
            Damage((Cryo) ? player.damDict["fireDam"] / 2 : player.damDict["fireDam"]);
            if (!log.del003)
                log.playerAction["pyroHit"]++;
            //log.pyroHit++;
            Debug.Log("Pyro:" + log.playerAction["pyroHit"]);
        }
        //Hit by active cryo
        if (other.CompareTag("Freeze"))
        {
            Damage(player.damDict["freezeDam"]);
            frozenCooldown = (Pyro) ? 0.5f : 1.5f;
            Destroy(other.gameObject);
            if (!log.del004)
                log.playerAction["cryoHit"]++;
            //log.cryoHit++;
            Debug.Log("Cryo:" + log.playerAction["cryoHit"]);
        }
        //Hit by active electro
        if (other.CompareTag("Bolt") )
        {
            Damage((Electro) ? player.damDict["boltDam"]/2 : player.damDict["boltDam"]);
            if (!log.del005)
                log.playerAction["electroHit"]++;
            //log.electroHit++;
            Debug.Log("Electro:" + log.playerAction["electroHit"]);
        }
        //Hit by active geo
        if (other.CompareTag("Tremor"))
        {
            Damage(player.damDict["tremorDam"]);
            tremorCooldown = 0.5f;
            if (!log.del006)
                log.playerAction["geoHit"]++;
            //log.geoHit++;
            Debug.Log("Geo:" + log.playerAction["geoHit"]);
        }
        //Hit by active hypno 
        if (other.CompareTag("Confuse"))
        {
            if(!Hypno)
                confuseCooldown = 5f;
            Destroy(other.gameObject);
            if (!log.del007)
                log.playerAction["hypnoHit"]++;
            //log.hypnoHit++;
            Debug.Log("Hypno:" + log.playerAction["hypnoHit"]);
        }
        //Hit by projectiles from confused enemy
        if (other.CompareTag("CBullet") && gameObject.CompareTag("Enemy"))
        {
            Damage(player.damDict["confuseDam"]);
            Destroy(other.gameObject);
            if (!log.del007)
                log.playerAction["hypnoHit"]++;
            Debug.Log("Hypno:" + log.playerAction["hypnoHit"]);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Incase of leaving room glitch
        if (other.CompareTag("Room"))
        {
            hp = 0;
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
