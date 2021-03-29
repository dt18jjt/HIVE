using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public int hp = 20;
    public float speed, normalSpeed, coldSpeed, stoppingDistance, retreatDistance, attackCooldown, startAtkCooldown, 
        moveCooldown, startMvCooldown, frozenCooldown, tremorCooldown = 0f, confuseCooldown = 0f;
    float heatTimer = 1f, coldTimer = 1f;
    public bool ranged, frozen = false, bpSpawn = false;
    //Enemy Types
    public bool Pyro, Cryo, Geo, Electro, Hypno, Explosive, Laser, Bullet, Shell, Melee;
    public GameObject projectile, confusionProjectile, burnProjectile, sporeProjectile, weakProjectile, splitProjectile, BP, Corpse, hitEffect;
    public Transform cEnemy;
    private Transform target;
    public Color normalColor, frozenColor, confuseColor;
    PlayerStat player;
    PlayerMovement playerMove;
    Rigidbody2D rb;
    static EnemyFollow instance;
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
        if (Hypno)
            player.buffNum++;
        //stoppingDistance = Random.Range(25, 31);
        //Physics2D.IgnoreLayerCollision(10, 10, true); 
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
            Instantiate(Corpse, transform.position, Quaternion.identity);
            Instantiate(BP, transform.position * 1.02f, Quaternion.identity);
            Destroy(gameObject);
            player.cEmenies.Remove(gameObject.transform);
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
            target = (player.pAbilDict["decoy"]) ? GameObject.FindGameObjectWithTag("Decoy").GetComponent<Transform>():
                GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        else if(confuseCooldown > 0)
        {
            while (player.cEmenies.Count > 1 && cEnemy == null)
                cEnemy = GameObject.FindWithTag("Enemy").GetComponent<Transform>();
            transform.gameObject.tag = (player.cEmenies.Count <= 1) ? "Enemy" : "Player";
            confuseCooldown -= Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = confuseColor;
            target = (player.cEmenies.Count <= 1) ? gameObject.transform : cEnemy;
        }
        if (Laser && hp < 20 && ranged)
        {
            ranged = false;
            retreatDistance = 25;
            stoppingDistance = 15;
            attackCooldown = 0.5f;
            startAtkCooldown = 0.5f;
            hp += 20;
        }
            
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
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, target.position) < retreatDistance && moveCooldown <= 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
            }
            enemyRangeAtk();
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
            if(confuseCooldown > 0)
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
            player.shockCoolDown = 0.2f;
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            hit.GetComponent<ParticleSystem>().Play();
            hp -= (Electro) ? 5 : 10;
        }
        
       
    }
    void Damage(int dam)
    {
        GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
        hit.GetComponent<ParticleSystem>().Play();
        Destroy(hit, 1f);
        hp -= dam;
        player.threatGauge += 5;
        if (Electro)
        {
            normalSpeed += 10;
            coldSpeed += 10;
        }
        if (Shell)
        {
            splitSpawn(4);
        }
            
    }
    void tremorKnockback()
    {
        if(tremorCooldown > 0)
        {
            transform.position = (Geo) ? Vector2.MoveTowards(transform.position, -target.position, 100 * Time.deltaTime) :
                Vector2.MoveTowards(transform.position, -target.position, 200 * Time.deltaTime);
            tremorCooldown -= Time.deltaTime;
            moveCooldown = startMvCooldown;
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
    private void OnTriggerEnter2D(Collider2D other){
        //Hit by bullet object
        if(other.CompareTag("Bullet")){
            Damage(player.damDict["bulletDam"]);
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
            if (!Explosive)
            {
                Damage(player.damDict["explosiveDam"]);
                log.explosiveHit++;
                Debug.Log("explosive:" + log.shellHit);
            }
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
            Damage((Cryo) ? player.damDict["fireDam"] / 2 : player.damDict["fireDam"]);
            log.pyroHit++;
            Debug.Log("Pyro:" + log.pyroHit);
        }
        //Hit by active cryo
        if (other.CompareTag("Freeze"))
        {
            Damage(player.damDict["freezeDam"]);
            frozenCooldown = (Pyro) ? 0.5f : 1.5f;
            Destroy(other.gameObject);
            log.cryoHit++;
            Debug.Log("Cryo:" + log.cryoHit);
        }
        //if (other.CompareTag("Pulse"))
        //{
        //    Damage(player.damDict["pulseDam"]);
        //}
        //Hit by active electro
        if (other.CompareTag("Bolt") )
        {
            Damage((Electro) ? player.damDict["boltDam"]/2 : player.damDict["boltDam"]);
            log.electroHit++;
            Debug.Log("Electro:" + log.electroHit);
        }
        //Hit by active geo
        if (other.CompareTag("Tremor"))
        {
            Damage(player.damDict["tremorDam"]);
            tremorCooldown = 0.5f;
            log.geoHit++;
            Debug.Log("Geo:" + log.geoHit);
        }
        //Hit by active hypno 
        if (other.CompareTag("Confuse"))
        {
            if(!Hypno)
                confuseCooldown = 5f;
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
    
}
