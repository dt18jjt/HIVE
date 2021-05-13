using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    public float runSpeed = 100.0f;
    private float dashSpeed = 30f, rotationZ, distance;
    public bool isDash, controller = false;
    bool mapOn = false, pressed = false, stopMovement;
    public GameObject miniMap, Map, crosshair, crosshair2, shCrosshair, shCrosshair2, cam, firePrefab, freezePrefab, 
        confusePrefab, bulletStart, afterImage, BoltArea, tremorArea;
    public GameObject spriteObj;
    public GameObject[] ammoPrefabs;
    public float bulletSpeed = 100.0f, explosiveSpeed = 80.0f, laserSpeed = 60.0f, slowCoolDown;
    private Vector3 target, moveDir, velocity, difference;
    private Vector2 lStickInput, rStickInput, direction;
    PlayerStat stat;
    RoomTemplates templates;
    camShake shake;
    public Sprite[] playerSprite;
    [SerializeField] LayerMask dashLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        body = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(10, 11, true);
        stat = GetComponent<PlayerStat>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<camShake>();
        spriteObj = GameObject.Find("P.Sprite");
    }

    // Update is called once per frame
    void Update()
    {
        //Sprite same pos as object
        spriteObj.transform.position = transform.position;
        //Change sprite based on rotation
        if (transform.rotation.z <= 0 && transform.rotation.z > -.6f)
            spriteObj.GetComponent<SpriteRenderer>().sprite = playerSprite[0];
        if (transform.rotation.z <= -.6f && transform.rotation.z >= -1)
            spriteObj.GetComponent<SpriteRenderer>().sprite = playerSprite[1];
        if (transform.rotation.z <= 1f && transform.rotation.z >= .6f)
            spriteObj.GetComponent<SpriteRenderer>().sprite = playerSprite[2];
        if (transform.rotation.z <= .6f && transform.rotation.z >= 0)
            spriteObj.GetComponent<SpriteRenderer>().sprite = playerSprite[3];
        //allow movement and actions after the level starts
        stopMovement = (templates.waitTime <= 0) ? false : true;
        //Minimap
        if (Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.Joystick1Button6))
            mapOn = !mapOn;
        miniMap.SetActive((mapOn) ? false : true);
        Map.SetActive((mapOn) ? true : false);
        //Detect input method
        controllerDetection();
        //Direction of player
        moveDirection();
        //shop cheat
        if (Input.GetKeyUp(KeyCode.P) && !stat.inStore)
        {
           store();
           stat.inStore = true;
            Cursor.visible = true;
        }            
        else if (Input.GetKeyUp(KeyCode.P) && stat.inStore)
        {
            stat.inStore = false;
            SceneManager.UnloadSceneAsync("shop");
            Cursor.visible = false;
        }
        runSpeed = (slowCoolDown > 0) ? 30 : 50;
        if (slowCoolDown > 0)
            slowCoolDown -= Time.deltaTime;
    }
    void store()
    {
        SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
    }
    private void FixedUpdate()
    {
        if (stat.hp > 0)
        {
            lStickInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            velocity = lStickInput.normalized * runSpeed;
            if (!stopMovement && !stat.inStore && stat.tangleCooldown <= 0)
                transform.position += velocity * Time.deltaTime;
        }
        if (stat.Active == "Bolt Dash")
            boltDash();
    }

    void moveDirection()
    {
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetAxisRaw("Vertical") > 0)
            moveY = +1f;
        if (Input.GetAxisRaw("Vertical") < 0)
            moveY = -1f;
        if (Input.GetAxisRaw("Horizontal") < 0)
            moveX = -1f;
        if (Input.GetAxisRaw("Horizontal") > 0)
            moveX = +1f;
        moveDir = new Vector3(moveX, moveY).normalized;
    }
    //Aim with mouse
    void mouseAim(){
        //Setting the crosshair to the mouse
        target = cam.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        crosshair.transform.position = new Vector3(target.x, target.y, 10);
        //distance between the crosshair and player
        difference = target - gameObject.transform.position;
        Vector3 shDifference = crosshair2.transform.position - gameObject.transform.position;
        Vector3 shDifference2 = shCrosshair.transform.position - gameObject.transform.position;
        Vector3 shDifference3 = shCrosshair2.transform.position - gameObject.transform.position;
        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        if (Input.GetMouseButtonUp(0))
        {
            if(!stat.wepJam){
                distance = difference.magnitude;
                float shDistance = shDifference.magnitude;
                direction = difference / distance;
                //Vector2 shDirection = shDifference / shDistance;
                Vector2 shDirection2 = shDifference2 / shDistance;
                Vector2 shDirection3 = shDifference3 / shDistance;
                direction.Normalize();
                if (stat.weapon1 == 1){
                    //Ammo taken
                    switch (stat.wep1Level)
                    {
                        case 0:
                            if(stat.ammo1 > 0)
                            {
                                bulletFire(direction, rotationZ);
                                stat.ammoDict["bullet"]--;
                                
                            }
                            break;
                        case 1:
                            if (stat.ammo1 > 0)
                            {
                                bulletFire(direction, rotationZ);
                                stat.ammoDict["bullet"]--;

                            }
                            break;
                        case 2:
                            if (stat.ammo1 > 0)
                            {
                                bulletFire(direction, rotationZ);
                                stat.ammoDict["bullet"]--;

                            }
                            break;
                        case 3:
                            if (stat.ammo1 >= 3)
                                StartCoroutine(burstFire());
                            break;
                    }
                }
                if (stat.weapon1 == 2){
                    //Ammo taken and Projectiles
                    switch (stat.wep1Level)
                    {
                        case 0:
                            if(stat.ammo1 >= 2)
                            {
                                stat.ammoDict["shell"] -= 2;
                                shellFire(shDirection2, body.rotation);
                                shellFire(shDirection3, body.rotation);
                            }
                            break;
                        case 1:
                            if (stat.ammo1 >= 2)
                            {
                                stat.ammoDict["shell"] -= 2;
                                shellFire(shDirection2, body.rotation);
                                shellFire(shDirection3, body.rotation);
                            }
                            break;
                        case 2:
                            if (stat.ammo1 >= 3)
                            {
                                stat.ammoDict["shell"] -= 3;
                                shellFire(direction, body.rotation);
                                shellFire(shDirection2, body.rotation);
                                shellFire(shDirection3, body.rotation);
                            }
                            break;
                        case 3:
                            if (stat.ammo1 >= 3)
                            {
                                stat.ammoDict["shell"] -= 3;
                                shellFire(direction, body.rotation);
                                shellFire(shDirection2, body.rotation);
                                shellFire(shDirection3, body.rotation);
                            }
                            break;
                    }
                }
                if (stat.weapon1 == 3){
                    //Projectile
                    if(stat.ammo1 > 0)
                    {
                        explosiveFire(direction, rotationZ);
                        //Ammo Taken
                        stat.ammoDict["explosive"]--;
                    }
                }
                if (stat.weapon1 == 4){
                    //Ammo taken
                    switch (stat.wep1Level)
                    {
                        case 0:
                            if(stat.ammo1 > 0)
                            {
                                laserFire(direction, rotationZ);
                                stat.ammoDict["laser"]--;
                            }
                            break;
                        case 1:
                            if(stat.ammo1 >= 2)
                            {
                                laserFire(direction, rotationZ);
                                stat.ammoDict["laser"]-= 2;
                            }
                            break;
                        case 2:
                            if (stat.ammo1 >= 6)
                            {
                                laserFire(direction, rotationZ);
                                stat.ammoDict["laser"] -= 6;
                            }
                            break;
                        case 3:
                            if (stat.ammo1 >= 12)
                            {
                                laserFire(direction, rotationZ);
                                stat.ammoDict["laser"] -= 12;
                            }
                            break;
                    }
                    stat.laserCooldown = 2f;
                }
            }
            //laser overload
            if(stat.ammo1 <= 0 && stat.weapon1 == 4)
                stat.laserCooldown = 3f;
            if (stat.weapon1 == 5 && stat.meleeCooldown <= 0){
                //Projectile
                meleeFire(rotationZ);
                //cooldowns
                switch (stat.wep1Level)
                {
                    case 0:
                        stat.meleeCooldown = 1f;
                        break;
                    case 1:
                        stat.meleeCooldown = 0.5f;
                        break;
                    case 2:
                        stat.meleeCooldown = 2f;
                        break;
                    case 3:
                        stat.meleeCooldown = 1f;
                        break;
                }
            }
        }
        if (Input.GetMouseButtonUp(1) && stat.activeCooldown <= 0 && stat.passiveCooldown <= 0)
        {
            if(stat.pp >= 30 && !stat.powBlock)
            {
                float distance = difference.magnitude;
                Vector2 direction = difference / distance;
                direction.Normalize();
                if(stat.Active == "Firebomb")
                {
                    Firebomb();
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                }
                if (stat.Active == "Freeze Blast")
                {
                    freezeBlast(direction, rotationZ);
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                }
                if (stat.Active == "Bolt Dash")
                {
                    isDash = true;
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                }
                if (stat.Active == "Tremor")
                {
                    tremor();
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                    stat.tangleCooldown = 0;
                }
                if (stat.Active == "Confusion")
                {
                    confusion(direction, rotationZ);
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                }
            }
        }
    }
    //Aim with joystick
    void stickAim()
    {
        rStickInput = new Vector2(Input.GetAxisRaw("RightStickX"), Input.GetAxisRaw("RightStickY"));
        if (rStickInput.magnitude > 0f)
        {
            difference = Vector3.down * rStickInput.x + Vector3.left * rStickInput.y;
            Quaternion rotationZ = Quaternion.LookRotation(difference, Vector3.forward);
            body.SetRotation(rotationZ);
            crosshair2.SetActive(true);
        }
        else
            crosshair2.SetActive(false);
        target = cam.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(rStickInput.x, rStickInput.y, 10));
        //Right trigger (Weapon)
        if (Input.GetAxis("Fire1") == 1 && !pressed )
        {
            pressed = true;
            if (!stat.wepJam)
            {
                //distance between the crosshair and player
                difference = crosshair2.transform.position - gameObject.transform.position;
                Vector3 difference2 = shCrosshair.transform.position - gameObject.transform.position;
                Vector3 difference3 = shCrosshair2.transform.position - gameObject.transform.position;
                distance = difference.magnitude;
                direction = difference / distance;
                Vector2 direction2 = difference2 / distance;
                Vector2 direction3 = difference3 / distance;
                direction.Normalize();
                if (stat.weapon1 == 1){
                    //Ammo taken
                    switch (stat.wep1Level)
                    {
                        case 0:
                            if (stat.ammo1 > 0)
                            {
                                bulletFire(direction, body.rotation);
                                stat.ammoDict["bullet"]--;

                            }
                            break;
                        case 1:
                            if (stat.ammo1 > 0)
                            {
                                bulletFire(direction, body.rotation);
                                stat.ammoDict["bullet"]--;

                            }
                            break;
                        case 2:
                            if (stat.ammo1 > 0)
                            {
                                bulletFire(direction, body.rotation);
                                stat.ammoDict["bullet"]--;

                            }
                            break;
                        case 3:
                            if (stat.ammo1 >= 0)
                                StartCoroutine(burstFire());
                            break;
                    }
                }
                if (stat.weapon1 == 2){
                    //Ammo taken and Projectiles
                    switch (stat.wep1Level)
                    {
                        case 0:
                            if (stat.ammo1 >= 2)
                            {
                                stat.ammoDict["shell"] -= 2;
                                shellFire(direction2, body.rotation);
                                shellFire(direction3, body.rotation);
                            }
                            break;
                        case 1:
                            if (stat.ammo1 >= 2)
                            {
                                stat.ammoDict["shell"] -= 2;
                                shellFire(direction2, body.rotation);
                                shellFire(direction3, body.rotation);
                            }
                            break;
                        case 2:
                            if (stat.ammo1 >= 3)
                            {
                                stat.ammoDict["shell"] -= 3;
                                shellFire(direction, body.rotation);
                                shellFire(direction2, body.rotation);
                                shellFire(direction3, body.rotation);
                            }
                            break;
                        case 3:
                            if (stat.ammo1 >= 3)
                            {
                                stat.ammoDict["shell"] -= 3;
                                shellFire(direction, body.rotation);
                                shellFire(direction2, body.rotation);
                                shellFire(direction3, body.rotation);
                            }
                            break;
                    }
                }
                if (stat.weapon1 == 3){
                    if (stat.ammo1 > 0)
                    {
                        explosiveFire(direction, body.rotation);
                        //Ammo taken
                        stat.ammoDict["explosive"]--;
                    }
                }
                if (stat.weapon1 == 4){
                    //Projectile 
                    laserFire(direction, body.rotation);
                    //Ammo taken
                    switch (stat.wep1Level)
                    {
                        case 0:
                            if (stat.ammo1 > 0)
                            {
                                laserFire(direction, body.rotation);
                                stat.ammoDict["laser"]--;
                            }
                            break;
                        case 1:
                            if (stat.ammo1 >= 2)
                            {
                                laserFire(direction, body.rotation);
                                stat.ammoDict["laser"] -= 2;
                            }
                            break;
                        case 2:
                            if (stat.ammo1 >= 6)
                            {
                                laserFire(direction, body.rotation);
                                stat.ammoDict["laser"] -= 6;
                            }
                            break;
                        case 3:
                            if (stat.ammo1 >= 12)
                            {
                                laserFire(direction, body.rotation);
                                stat.ammoDict["laser"] -= 12;
                            }
                            break;
                    }
                            stat.laserCooldown = 2f;
                }
                
            }
            if (stat.ammo1 <= 0 && stat.weapon1 == 4)
                stat.laserCooldown = 3f;
            if (stat.weapon1 == 5 && stat.meleeCooldown <= 0){
                //Projectile 
                meleeFire(body.rotation);
                //Cooldowns
                switch (stat.wep1Level){
                    case 0:
                        stat.meleeCooldown = 1f;
                        break;
                    case 1:
                        stat.meleeCooldown = 0.5f;
                        break;
                    case 2:
                        stat.meleeCooldown = 2f;
                        break;
                    case 3:
                        stat.meleeCooldown = 1f;
                        break;
                }
            }
        }
        //Left trigger (Ability)
        else if (Input.GetAxis("Fire1") == -1 && !pressed && stat.activeCooldown <= 0 && stat.passiveCooldown <= 0)
        {
            pressed = true;
            if (stat.pp >= 30 && !stat.powBlock)
            {
                Vector3 difference = crosshair2.transform.position - gameObject.transform.position;
                float distance = difference.magnitude;
                Vector2 direction = difference / distance;
                direction.Normalize();
                if (stat.Active == "Firebomb")
                {
                    Firebomb();
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                }
                if (stat.Active == "Freeze Blast")
                {
                    freezeBlast(direction, body.rotation);
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                }
                if (stat.Active == "Bolt Dash")
                {
                    isDash = true;
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                }
                if (stat.Active == "Tremor")
                {
                    tremor();
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                    stat.tangleCooldown = 0f;
                }
                if (stat.Active == "Confusion")
                {
                    confusion(direction, body.rotation);
                    stat.pp -= 30;
                    stat.activeCooldown = 2;
                }
            }


        }
        else if (Input.GetAxis("Fire1") == 0 && pressed)
        {
            pressed = false;
        }
    }
    //Bullets spawn
    void bulletFire(Vector2 direction, float rotationZ){
        GameObject b = Instantiate((stat.wep1Level == 2) ? ammoPrefabs[7] : ammoPrefabs[0]) as GameObject;
        b.transform.position = bulletStart.transform.position;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        b.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        Destroy(b, 0.7f);
    }
    //Shells spawn
    void shellFire(Vector2 direction, float rotationZ){
        GameObject s = Instantiate(ammoPrefabs[1]) as GameObject;
        s.transform.position = bulletStart.transform.position;
        s.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        s.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        Destroy(s, 0.5f);
    }
    //Explosive spawn
    void explosiveFire(Vector2 direction, float rotationZ)
    {
        //different types spawned based on level
        GameObject e;
        switch (stat.wep1Level)
        {
            case 0:
                e = Instantiate(ammoPrefabs[2]) as GameObject;
                e.transform.position = bulletStart.transform.position;
                e.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                e.GetComponent<Rigidbody2D>().velocity = direction * explosiveSpeed;
                break;
            case 1:
                e = Instantiate(ammoPrefabs[5]) as GameObject;
                e.transform.position = bulletStart.transform.position;
                e.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                e.GetComponent<Rigidbody2D>().velocity = direction * explosiveSpeed;
                break;
            case 2:
                e = Instantiate(ammoPrefabs[6]) as GameObject;
                e.transform.position = bulletStart.transform.position;
                e.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                e.GetComponent<Rigidbody2D>().velocity = direction * explosiveSpeed;
                break;
            case 3:
                e = Instantiate(ammoPrefabs[6]) as GameObject;
                e.transform.position = bulletStart.transform.position;
                e.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                e.GetComponent<Rigidbody2D>().velocity = direction * explosiveSpeed;
                break;
        }
    }
    //laser spawn
    void laserFire(Vector2 direction, float rotationZ)
    {
        //different types spawned based on level
        GameObject l;
        switch (stat.wep1Level)
        {
            case 0:
                l = Instantiate(ammoPrefabs[3]) as GameObject;
                l.transform.position = bulletStart.transform.position;
                l.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                l.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;
                Destroy(l, 0.8f);
                break;
            case 1:
                l = Instantiate(ammoPrefabs[8]) as GameObject;
                l.transform.position = bulletStart.transform.position;
                l.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                l.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;
                Destroy(l, 0.8f);
                break;
            case 2:
                l = Instantiate(ammoPrefabs[9]) as GameObject;
                l.transform.position = bulletStart.transform.position;
                l.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                l.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;
                Destroy(l, 0.8f);
                break;
            case 3:
                l = Instantiate(ammoPrefabs[10]) as GameObject;
                l.transform.position = bulletStart.transform.position;
                l.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                l.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;
                Destroy(l, 0.8f);
                break;
        }
    }
    //melee spawn
    void meleeFire(float rotationZ)
    {
        GameObject m = Instantiate(ammoPrefabs[4]) as GameObject;
        m.transform.position = bulletStart.transform.position;
        m.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        Destroy(m, 0.5f);
    }
    //Level 3 bullet weapon burst fire
    IEnumerator burstFire()
    {
        if(stat.ammo1 > 0)
        {
            bulletFire(direction, (controller) ? body.rotation : rotationZ);
            stat.ammoDict["bullet"]--;
        }
        yield return new WaitForSeconds(0.2f);
        if (stat.ammo1 > 0)
        {
            if(controller)
                difference = crosshair2.transform.position - gameObject.transform.position;
            direction = difference / distance;
            bulletFire(direction, (controller) ? body.rotation : rotationZ);
            stat.ammoDict["bullet"]--;
        }
        yield return new WaitForSeconds(0.2f);
        if (stat.ammo1 > 0)
        {
            if (controller)
                difference = crosshair2.transform.position - gameObject.transform.position;
            direction = difference / distance;
            bulletFire(direction, (controller) ? body.rotation : rotationZ);
            stat.ammoDict["bullet"]--;
        }
    }
    //Firebomb (Ability)
    void Firebomb()
    {
        GameObject b = Instantiate(firePrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(b, 0.5f);
    }
    //Freeze Blast (Ability)
    void freezeBlast(Vector2 direction, float rotationZ)
    {
        GameObject b = Instantiate(freezePrefab) as GameObject;
        b.transform.position = bulletStart.transform.position;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        b.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        Destroy(b, 0.7f);
    }
    //Confusion (Ability)
    void confusion(Vector2 direction, float rotationZ)
    {
        GameObject c = Instantiate(confusePrefab) as GameObject;
        c.transform.position = bulletStart.transform.position;
        c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        c.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        Destroy(c, 0.7f);
    }
    //Tremor (Ability)
    void tremor()
    {
        GameObject t = Instantiate(tremorArea, transform.position, Quaternion.identity) as GameObject;
        Destroy(t, 0.4f);
        shake.shakeDuration = 0.6f;
    }
    void boltDash()
    {
        if (isDash)
        {
            var aImage = Instantiate(afterImage, transform.position, Quaternion.identity);
            Destroy(aImage, 1f);
            StartCoroutine(boltArea());
            Vector3 dashPos = transform.position + moveDir * dashSpeed;
            RaycastHit2D raycast2D = Physics2D.Raycast(transform.position, moveDir, dashSpeed, dashLayerMask);
            if(raycast2D.collider != null){
                dashPos = raycast2D.point;
            }
            body.MovePosition(dashPos);
            isDash = false;
        }
       
    }
    IEnumerator boltArea()
    {
        BoltArea.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        BoltArea.SetActive(false);
    }
    void controllerDetection()
    {
        if (!controller && !stat.inStore && stat.storeCoolDown <= 0 && !stopMovement){
            mouseAim();
            crosshair.SetActive(true);
            crosshair2.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (controller && !stat.inStore && stat.storeCoolDown <= 0 && !stopMovement){
            stickAim();
            crosshair.SetActive(false);
            crosshair2.GetComponent<SpriteRenderer>().enabled = true;
        }
        //mouse detection
        if (Input.GetAxisRaw("Mouse X") != 0.0f || Input.GetAxisRaw("Mouse Y") != 0.0f)
            controller = false;
        if(Input.anyKey)
            controller = false;
        //joystick detection
        if (Input.GetAxisRaw("RightStickX") != 0.0f || Input.GetAxisRaw("RightStickY") != 0.0f || Input.GetAxis("Fire1") != 0 || Input.GetAxis("Fire2") != 0)
            controller = true;
        if (Input.GetKey(KeyCode.Joystick1Button0) ||
            Input.GetKey(KeyCode.Joystick1Button1) ||
            Input.GetKey(KeyCode.Joystick1Button2) ||
            Input.GetKey(KeyCode.Joystick1Button3) ||
            Input.GetKey(KeyCode.Joystick1Button4) ||
            Input.GetKey(KeyCode.Joystick1Button5) ||
            Input.GetKey(KeyCode.Joystick1Button6) ||
            Input.GetKey(KeyCode.Joystick1Button7) ||
            Input.GetKey(KeyCode.Joystick1Button8) ||
            Input.GetKey(KeyCode.Joystick1Button9) ||
            Input.GetKey(KeyCode.Joystick1Button10))
            controller = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
            velocity = Vector3.zero;
    }
}
