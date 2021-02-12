using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    public float runSpeed = 100.0f;
    private float dashSpeed = 30f;
    public bool isDash;
    bool mapOn = false;
    bool pressed = false;
    public bool controller = false;
    public GameObject miniMap;
    public GameObject Map;
    public GameObject crosshair;
    public GameObject crosshair2;
    public GameObject shCrosshair;
    public GameObject shCrosshair2;
    public GameObject cam;
    public GameObject[] ammoPrefabs;
    public GameObject firePrefab;
    public GameObject freezePrefab;
    public GameObject bulletStart;
    public GameObject afterImage;
    public GameObject BoltArea;
    public GameObject tremorArea;
    public float bulletSpeed = 100.0f;
    public float expolsiveSpeed = 80.0f;
    public float laserSpeed = 60.0f;
    private Vector3 target;
    private Vector2 lStickInput;
    private Vector2 rStickInput;
    private Vector3 moveDir;
    PlayerStat stat;
    RoomTemplates templates;
    [SerializeField] LayerMask dashLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        body = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(10, 11, true);
        stat = GetComponent<PlayerStat>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stat.hp > 0){
            lStickInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector3 velocity = lStickInput.normalized * runSpeed;
            if (templates.waitTime <= 0)
                transform.position += velocity * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.Joystick1Button6))
        {
            mapOn = !mapOn;
        }
        if (mapOn){
            miniMap.SetActive(false);
            Map.SetActive(true);
        }
        if (!mapOn){
            miniMap.SetActive(true);
            Map.SetActive(false);
        }
        //Detect input method
        controllerDetection();
        if (!controller)
        {
            mouseAim();
            crosshair.SetActive(true);
            crosshair2.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (controller)
        {
            stickAim();
            crosshair.SetActive(false);
            crosshair2.GetComponent<SpriteRenderer>().enabled = true;
        }
        //Direction of player
        moveDirection();
        
    }
    private void FixedUpdate()
    {
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
        
        target = cam.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        crosshair.transform.position = new Vector3(target.x, target.y, 10);

        Vector3 difference = target - gameObject.transform.position;
        Vector3 shDifference = crosshair2.transform.position - gameObject.transform.position;
        Vector3 shDifference2 = shCrosshair.transform.position - gameObject.transform.position;
        Vector3 shDifference3 = shCrosshair2.transform.position - gameObject.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        if (Input.GetMouseButtonUp(0)){
            if(stat.ammo1 > 0 && !stat.wepJam){
                float distance = difference.magnitude;
                float shDistance = shDifference.magnitude;
                Vector2 direction = difference / distance;
                Vector2 shDirection = shDifference / shDistance;
                Vector2 shDirection2 = shDifference2 / shDistance;
                Vector2 shDirection3 = shDifference3 / shDistance;
                direction.Normalize();
                if (stat.weapon1 == 1){
                    bulletFire(direction, rotationZ);
                    stat.ammoDict["bullet"]--;
                }
                if (stat.weapon1 == 2){
                    //ShellFire(shDirection, rotationZ);
                    shellFire(shDirection2, rotationZ);
                    shellFire(shDirection3, rotationZ);
                    stat.ammoDict["shell"]--;
                }
                if (stat.weapon1 == 3){
                    expolsiveFire(direction, rotationZ);
                    stat.ammoDict["expolsive"]--;
                }
                if (stat.weapon1 == 4){
                    laserFire(direction, rotationZ);
                    stat.ammoDict["laser"]--;
                    stat.laserCooldown = 2f;
                }
            }
            //laser overload
            if(stat.ammo1 <= 0 && stat.weapon1 == 4)
                stat.laserCooldown = 4f;
            if (stat.weapon1 == 5)
            {
                meleeFire(rotationZ);
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
            Vector3 difference = Vector3.down * rStickInput.x + Vector3.left * rStickInput.y;
            Quaternion rotationZ = Quaternion.LookRotation(difference, Vector3.forward);
            body.SetRotation(rotationZ);
            crosshair2.SetActive(true);
        }
        else
            crosshair2.SetActive(false);
        target = cam.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(rStickInput.x, rStickInput.y, 10));
        //Right trigger (Weapon)
        if (Input.GetAxis("Fire1") == 1 && !pressed)
        {
            pressed = true;
            if (stat.ammo1 > 0 && !stat.wepJam)
            {
                Vector3 difference = crosshair2.transform.position - gameObject.transform.position;
                Vector3 difference2 = shCrosshair.transform.position - gameObject.transform.position;
                Vector3 difference3 = shCrosshair2.transform.position - gameObject.transform.position;
                float distance = difference.magnitude;
                Vector2 direction = difference / distance;
                Vector2 direction2 = difference2 / distance;
                Vector2 direction3 = difference3 / distance;
                direction.Normalize();
                if (stat.weapon1 == 1){
                    bulletFire(direction, body.rotation);
                    stat.ammoDict["bullet"]--;
                }
                if (stat.weapon1 == 2){
                    //ShellFire(direction, body.rotation);
                    shellFire(direction2, body.rotation);
                    shellFire(direction3, body.rotation);
                    stat.ammoDict["shell"]--;
                }
                if (stat.weapon1 == 3){
                    expolsiveFire(direction, body.rotation);
                    stat.ammoDict["expolsive"]--;
                }
                if (stat.weapon1 == 4){
                    laserFire(direction, body.rotation);
                    stat.ammoDict["laser"]--;
                    stat.laserCooldown = 2f;
                }
                
            }
            if (stat.ammo1 <= 0 && stat.weapon1 == 4)
                stat.laserCooldown = 4f;
            if (stat.weapon1 == 5)
            {
                meleeFire(body.rotation);
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
        GameObject b = Instantiate(ammoPrefabs[0]) as GameObject;
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
    //Expolsive spawn
    void expolsiveFire(Vector2 direction, float rotationZ)
    {
        GameObject e = Instantiate(ammoPrefabs[2]) as GameObject;
        e.transform.position = bulletStart.transform.position;
        e.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        e.GetComponent<Rigidbody2D>().velocity = direction * expolsiveSpeed;
        Destroy(e, 1f);
    }
    //laser spawn
    void laserFire(Vector2 direction, float rotationZ)
    {
        GameObject l = Instantiate(ammoPrefabs[3]) as GameObject;
        l.transform.position = bulletStart.transform.position;
        l.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        l.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;
        Destroy(l, 0.8f);
    }
    //melee spawn
    void meleeFire(float rotationZ)
    {
        GameObject m = Instantiate(ammoPrefabs[4]) as GameObject;
        m.transform.position = bulletStart.transform.position;
        m.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        Destroy(m, 0.5f);
    }

    //Firebomb (Ability)
    void Firebomb()
    {
        GameObject b = Instantiate(firePrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(b, 0.5f);
    }
    void freezeBlast(Vector2 direction, float rotationZ)
    {
        GameObject b = Instantiate(freezePrefab) as GameObject;
        b.transform.position = bulletStart.transform.position;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        b.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        Destroy(b, 0.7f);
    }
    void tremor()
    {
        GameObject t = Instantiate(tremorArea, transform.position, Quaternion.identity) as GameObject;
        Destroy(t, 0.4f);
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

}
