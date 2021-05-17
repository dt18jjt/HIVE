using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStat : MonoBehaviour
{
    public int hpMax = 100, ppMax = 100, bp = 0, buffNum, threatLV = 2, actNum, pasNum, gameLevel = 0;
    public float hp = 100, pp = 100, threatGauge = 50;
    //Ammo display
    public float ammo1 , ammo2;
    //Weapon values
    public int weapon1, weapon2, wep1Level = 0, wep2Level = 0 , ammoStack1 = 0, ammoStack2 = 0, wepDropNum, wepDropNum2;
    //cooldowns
    public float damCooldown, pulseCooldown, meleeCooldown, laserCooldown = 0f, activeCooldown = 0f, passiveCooldown = 0f,
        shockCoolDown = 0f, passiveTimer = 1f, burnCoolDown, tangleCooldown, storeCoolDown = 0f, killCoolDown = 0f;
    public string Active, Passive;
    //Room conditions
    public bool wepJam = false, powBlock = false, shockDam = false, inStore = false, enemyBuff = false, onLab = false, bossFight, storeFound = false;
    bool pickedUp = false, showEffect = false, stackWep = false, dead;
    int tempWep;
    public Dictionary<string, bool> pAbilDict = new Dictionary<string, bool>(); // Passive abilities Dictionary
    public Dictionary<string, bool> wepPickupDict = new Dictionary<string, bool>(); // Passive abilities Dictionary
    public Dictionary<string, int> damDict = new Dictionary<string, int>(); // Damage Dictionary
    public Dictionary<string, int> wepLevelDict = new Dictionary<string, int>(); // Damage Dictionary
    public Dictionary<string, float> ammoDict = new Dictionary<string, float>(); // Ammo Dictionary
    public Dictionary<string, int> ammoMaxDict = new Dictionary<string, int>(); // Max Ammo Dictionary
    public Text activeText, passiveText;
    Text hpText, ppText, a1Text, a2Text, threatText, bpText;
    private Image threatImg;
    public Color activeColor, passiveColor;
    public GameObject hitEffect, deadEffect; //hit particle
    public GameObject jamImage, blockImage, hazardImage, pickupText, decoy;
    public GameObject[] glitchItems; // items dropped from glitches
    public GameObject[] cacheItems; // items dropped from cache
    public GameObject[] crateItems; // items dropped from crate
    public GameObject[] wepDrop; //weapon the player can drop
    public Slider hpBar, ppBar; // sliders for the player HP and PP
    public List<Transform> cEmenies;
    GameObject crate, glitchObj, cacheObj;
    Log log;
    PlayerMovement player;
    EnemyProjectile eBullet;
    RoomTemplates templates;
    public AudioClip hitSound, doorSound, bpSound, pickupSound, boxSound, exitSound, bombSound, upSound, downSound;
    // Start is called before the first frame update
    void Start(){
        player = GetComponent<PlayerMovement>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        //Set text at start
        pickupText.SetActive(false);
        //Setting Damage Values
        {
            damDict.Add("bulletDam", 10);
            damDict.Add("shellDam", 10);
            damDict.Add("explosiveDam", 20);
            damDict.Add("fireDam", 20);
            damDict.Add("freezeDam", 10);
            damDict.Add("boltDam", 15);
            damDict.Add("confuseDam", 5);
            damDict.Add("tremorDam", 10);
            damDict.Add("laserDam", 5);
            damDict.Add("meleeDam", 10);
        }
        //Setting Max Ammo Values
        {
            ammoMaxDict.Add("bulletMax", 32);
            ammoMaxDict.Add("shellMax", 16);
            ammoMaxDict.Add("explosiveMax", 8);
            ammoMaxDict.Add("laserMax", 24);
        }
        //Setting Passive Abilites Values
        {
            pAbilDict.Add("heat", false);
            pAbilDict.Add("cold", false);
            pAbilDict.Add("shock", false);
            pAbilDict.Add("earth", false);
            pAbilDict.Add("decoy", false);
        }
        //Setting pickup values
        {
            wepPickupDict.Add("b0", false);
            wepPickupDict.Add("s0", false);
            wepPickupDict.Add("e0", false);
            wepPickupDict.Add("l0", false);
            wepPickupDict.Add("m0", false);
            wepPickupDict.Add("b1", false);
            wepPickupDict.Add("s1", false);
            wepPickupDict.Add("e1", false);
            wepPickupDict.Add("l1", false);
            wepPickupDict.Add("m1", false);
            wepPickupDict.Add("b2", false);
            wepPickupDict.Add("s2", false);
            wepPickupDict.Add("e2", false);
            wepPickupDict.Add("l2", false);
            wepPickupDict.Add("m2", false);
            wepPickupDict.Add("b3", false);
            wepPickupDict.Add("s3", false);
            wepPickupDict.Add("e3", false);
            wepPickupDict.Add("l3", false);
            wepPickupDict.Add("m3", false);
            wepPickupDict.Add("hp", false);
            wepPickupDict.Add("hUP", false);
            wepPickupDict.Add("pUP", false);
            wepPickupDict.Add("cache", false);
            wepPickupDict.Add("glitch", false);
            wepPickupDict.Add("bAmmo", false);
            wepPickupDict.Add("eAmmo", false);
            wepPickupDict.Add("shAmmo", false);
            wepPickupDict.Add("box", false);
        }
        //set default ammo at the start of the game
        if (gameLevel == 0)
        {
            //Setting Ammo Values
            {
                ammoDict.Add("bullet", 24);
                ammoDict.Add("shell", 10);
                ammoDict.Add("explosive", 4);
                ammoDict.Add("laser", 24);
            }
        }
        //Carry over ammo to next level
        else if (gameLevel > 0)
        {
            ammoDict.Add("bullet", PlayerPrefs.GetFloat("Bullet"));
            ammoDict.Add("shell", PlayerPrefs.GetFloat("Shell"));
            ammoDict.Add("explosive", PlayerPrefs.GetFloat("Explosive"));
            ammoDict.Add("laser", 24);
        }
        decoy.SetActive(false);
        //finding class values
        log = GameObject.Find("Global").GetComponent<Log>();
        hpText = GameObject.Find("hpText").GetComponent<Text>();
        ppText = GameObject.Find("ppText").GetComponent<Text>();
        a1Text = GameObject.Find("ammo1Text").GetComponent<Text>();
        a2Text = GameObject.Find("ammo2Text").GetComponent<Text>();
        if (!bossFight)
        {
            threatImg = GameObject.Find("TLvlImage").GetComponent<Image>();
            threatText = GameObject.Find("TLvlText").GetComponent<Text>();
            bpText = GameObject.Find("bpText").GetComponent<Text>();
        }
       

    }

    // Update is called once per frame
    void Update(){
        Vector2 pickupPos = new Vector2(transform.position.x, transform.position.y + 15);
        pickupText.transform.position = pickupPos;
        //keeping value player preferences
        {
            if(templates.waitTime <= 0)
            {
                PlayerPrefs.SetInt("BP", bp);
                PlayerPrefs.SetFloat("HP", hp);
                PlayerPrefs.SetInt("HPMax", hpMax);
                PlayerPrefs.SetInt("PPMax", ppMax);
                PlayerPrefs.SetInt("Threat Level", threatLV);
                PlayerPrefs.SetInt("Weapon1", weapon1);
                PlayerPrefs.SetInt("Weapon2", weapon2);
                PlayerPrefs.SetInt("Wep1LV", wep1Level);
                PlayerPrefs.SetInt("Wep2LV", wep2Level);
                PlayerPrefs.SetInt("Ammo Stack 1", ammoStack1);
                PlayerPrefs.SetInt("Ammo Stack 2", ammoStack2);
                PlayerPrefs.SetFloat("Threat Gauge", threatGauge);
                PlayerPrefs.SetFloat("Bullet", ammoDict["bullet"]);
                PlayerPrefs.SetFloat("Shell", ammoDict["shell"]);
                PlayerPrefs.SetFloat("Explosive", ammoDict["explosive"]);
                PlayerPrefs.SetFloat("Laser", ammoDict["laser"]);
            }
        }
        setText();
        itemPickup();
        //Player updates
        {
            //Player Health set to zero and max
            if (hp <= 0)
            {
                hp = 0;
                if (!dead)
                {
                    StartCoroutine(death());
                    dead = true;
                }
            }
            if (hp > hpMax)
                hp = 100;
            //Player psyhic set to zero
            if (pp <= 0)
            {
                pp = 0;
                pAbilDict["heat"] = false;
                pAbilDict["cold"] = false;
                pAbilDict["shock"] = false;
                pAbilDict["earth"] = false;
                pAbilDict["decoy"] = false;
                decoy.SetActive(false);
            }
            if (pp > ppMax)
                pp = ppMax;
            //Passive decrease
            if (pAbilDict["heat"] || pAbilDict["cold"] || pAbilDict["shock"] || pAbilDict["earth"] || pAbilDict["decoy"])
            {
                passiveCooldown = 1;
                passiveTimer -= Time.deltaTime;
                if (passiveTimer <= 0)
                {
                    pp -= 20;
                    passiveTimer = 1f;
                }
            }
            //Psyhic recharge
            if (pp < ppMax && activeCooldown > 0)
                activeCooldown -= Time.deltaTime;
            if (pp < ppMax && passiveCooldown > 0)
            {
                if (!pAbilDict["heat"] || !pAbilDict["cold"] || pAbilDict["shock"] || pAbilDict["earth"] || pAbilDict["decoy"])
                    passiveCooldown -= Time.deltaTime;
            }
            if (pp < ppMax && activeCooldown <= 0 && passiveCooldown <= 0)
            {
                activeCooldown = 0;
                passiveCooldown = 0;
                pp += Time.deltaTime * 10f;

            }
            //set ammo to weapon equipped
            equippedWeapon();
            controlInputs();

            //Damage CoolDown
            player.spriteObj.GetComponent<SpriteRenderer>().color = (damCooldown > 0) ? Color.gray : Color.white;
            if (damCooldown > 0)
                damCooldown -= Time.deltaTime;
        }
        //Cooldowns
        {
            //Pulse CoolDown
            if (pulseCooldown > 0)
                pulseCooldown -= Time.deltaTime;
            //shockCooldown
            if (shockCoolDown > 0)
                shockCoolDown -= Time.deltaTime;
            if (shockCoolDown <= 0)
                shockDam = false;
            //storeCooldown
            if (storeCoolDown > 0)
                storeCoolDown -= Time.deltaTime;
            //Weapon level up
            if (wep1Level == 0 && ammoStack1 >= 2)
            {
                pickupText.GetComponent<TextMesh>().text = "Weapon Level Up!";
                wep1Level++;
                ammoStack1 = 0;
            }
            wepPickup();
            //Burn cooldown
            if (burnCoolDown > 0)
            {
                burnCoolDown -= Time.deltaTime;
                StartCoroutine(burnDam());
            }
            //tangle cooldown
            if (tangleCooldown > 0)
                tangleCooldown -= Time.deltaTime;
            //melee cooldown
            if (meleeCooldown > 0)
                meleeCooldown -= Time.deltaTime;
            //kill cooldown
            if (killCoolDown > 0)
                killCoolDown -= Time.deltaTime;
        }
        //Enemy buffing 
        enemyBuff = buffNum > 0;
        //Threat level
        if (threatGauge >= 100 && threatLV <= 2)
        {
            threatLV += (threatLV <= 2) ? 1 : 0;
            threatGauge = (threatLV <= 2) ? 0 : 100;
            GetComponent<AudioSource>().PlayOneShot(upSound);
            Debug.Log("Threat level up!");
        }
        if (threatGauge >= 100 && threatLV == 3)
        {
            threatGauge = 100;

        }
        if (threatGauge < 0 && threatLV > 1)
        {
            threatGauge = (threatLV > 1) ? 50 : 0;
            threatLV -= (threatLV > 1) ? 1 : 0;
            GetComponent<AudioSource>().PlayOneShot(upSound);
            Debug.Log("Threat level down!");
        }

        //When player is colliding with lab pod
        shop();
        setBarSize();
        setAbility();
        
    }
    IEnumerator death()
    {
        GameObject dFx = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
        dFx.GetComponent<ParticleSystem>().Play();
        Destroy(dFx, 1f);
        yield return new WaitForSeconds(1f);
        Cursor.visible = true;
        SceneManager.LoadScene("Death", LoadSceneMode.Additive);
        Time.timeScale = 0f;
    }
    private void setText()
    {
        //health text
        hpText.text = hp.ToString("F0") + "/" + hpMax.ToString();
        //psychic text
        ppText.text = pp.ToString("F0") + "/"+ ppMax.ToString();
        //ammo text
        a1Text.text = ammo1.ToString("F0");
        a2Text.text = ammo2.ToString("F0");
        if (!bossFight)
        {
            bpText.text = bp.ToString();
            activeText.color = (powBlock) ? Color.gray : activeColor;
            passiveText.color = (powBlock) ? Color.gray : passiveColor;
            threatText.text = "LV." + threatLV;
            switch (threatLV)
            {
                case 1:
                    threatText.color = Color.green;
                    threatImg.color = Color.green;
                    break;
                case 2:
                    threatText.color = Color.yellow;
                    threatImg.color = Color.yellow;
                    break;
                case 3:
                    threatText.color = Color.red;
                    threatImg.color = Color.red;
                    break;
            }
        }

    }
    public void Damage(float dam){
        //Player Damage
        if(hp > 0 && damCooldown <= 0){
            if (!pAbilDict["earth"])
            {
                //lose hp
                hp -= dam;
                GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
                hit.GetComponent<ParticleSystem>().Play();
                Destroy(hit, 1f);
                //sound effect
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                //static shock damaging all enemies
                if (pAbilDict["shock"] && !shockDam)
                {
                    shockDam = true;
                    hp += (dam / 2);
                    foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        EnemyFollow enemy = e.GetComponent<EnemyFollow>();
                        GameObject h = Instantiate(hitEffect, e.transform.position, Quaternion.identity) as GameObject;
                        h.GetComponent<ParticleSystem>().Play();
                        enemy.hp -= (enemy.Electro) ? 5 : 10;
                        shockCoolDown = .2f;
                    }
                }
                //cooldown after being hit
                damCooldown = 1.5f;
                //decrease threat gauge
                switch (threatLV)
                {
                    case 1:
                        threatGauge -= 10;
                        break;
                    case 2:
                        threatGauge -= 20;
                        break;
                    case 3:
                        threatGauge -= 40;
                        break;
                }
                    
            }
            if (log.noDamage)
                log.noDamage = false;
        }
        
    }
    private void equippedWeapon()
    {
        //Primary weapon
        switch (weapon1)
        {
            case 1:
                //Set ammo text on
                a1Text.enabled = true;
                //Set ammo text to bullet value
                ammo1 = ammoDict["bullet"];
                //Set text color to ammo value
                ammo1Color1();
                //Set damage based on level
                switch (wep1Level)
                {
                    case 0:
                        damDict["bulletDam"] = 10; //Damage
                        wepDropNum = 0; //Weapon drop value
                        break;
                    case 1:
                        damDict["bulletDam"] = 15;
                        wepDropNum = 5;
                        break;
                    case 2:
                        damDict["bulletDam"] = 10;
                        wepDropNum = 10;
                        break;
                    case 3:
                        damDict["bulletDam"] = 10;
                        wepDropNum = 15;
                        break;
                }
                break;
            case 2:
                a1Text.enabled = true;
                //Set ammo text to bullet value
                ammo1 = ammoDict["shell"];
                ammo1Color2();
                switch (wep1Level)
                {
                    case 0:
                        damDict["shellDam"] = 10;
                        wepDropNum = 1;
                        break;
                    case 1:
                        damDict["shellDam"] = 15;
                        wepDropNum = 6;
                        break;
                    case 2:
                        damDict["shellDam"] = 10;
                        wepDropNum = 11;
                        break;
                    case 3:
                        damDict["shellDam"] = 15;
                        wepDropNum = 16;
                        break;
                }
                break;
            case 3:
                a1Text.enabled = true;
                ammo1 = ammoDict["explosive"];
                ammo1Color3();
                switch (wep1Level)
                {
                    case 0:
                        damDict["explosiveDam"] = 20;
                        wepDropNum = 2;
                        break;
                    case 1:
                        damDict["explosiveDam"] = 20;
                        wepDropNum = 7;
                        break;
                    case 2:
                        damDict["explosiveDam"] = 30;
                        wepDropNum = 12;
                        break;
                    case 3:
                        damDict["explosiveDam"] = 30;
                        wepDropNum = 17;
                        break;
                }
                break;
            case 4:
                a1Text.enabled = true;
                ammo1 = ammoDict["laser"];
                ammo1Color4();
                switch (wep1Level)
                {
                    case 0:
                        damDict["laserDam"] = 5;
                        wepDropNum = 3;
                        break;
                    case 1:
                        damDict["laserDam"] = 10;
                        wepDropNum = 8;
                        break;
                    case 2:
                        damDict["laserDam"] = 20;
                        wepDropNum = 13;
                        break;
                    case 3:
                        damDict["laserDam"] = 40;
                        wepDropNum = 18;
                        break;
                }
                break;
            case 5:
                a1Text.enabled = true;
                ammo1 = 0;
                a1Text.text = "";
                switch (wep1Level)
                {
                    case 0:
                        damDict["meleeDam"] = 10;
                        wepDropNum = 4;
                        break;
                    case 1:
                        damDict["meleeDam"] = 10;
                        wepDropNum = 9;
                        break;
                    case 2:
                        damDict["meleeDam"] = 20;
                        wepDropNum = 14;
                        break;
                    case 3:
                        damDict["meleeDam"] = 15;
                        wepDropNum = 19;
                        break;
                }
                break;
        }
        //Secondary weapon
        switch (weapon2)
        {
            case 1:
                a2Text.enabled = true;
                ammo2 = ammoDict["bullet"];
                ammo2Color1();
                switch (wep2Level)
                {
                    case 0:
                        wepDropNum2 = 0; //Weapon drop value
                        break;
                    case 1:
                        wepDropNum2 = 5;
                        break;
                    case 2:
                        wepDropNum2 = 10;
                        break;
                    case 3:
                        wepDropNum2 = 15;
                        break;
                }
                break;
                
            case 2:
                a2Text.enabled = true;
                ammo2 = ammoDict["shell"];
                ammo2Color2();
                switch (wep2Level)
                {
                    case 0:
                        wepDropNum2 = 1;
                        break;
                    case 1:
                        wepDropNum2 = 6;
                        break;
                    case 2:
                        wepDropNum2 = 11;
                        break;
                    case 3:
                        wepDropNum2 = 16;
                        break;
                }
                break;
            case 3:
                a2Text.enabled = true;
                ammo2 = ammoDict["explosive"];
                ammo2Color3();
                switch (wep2Level)
                {
                    case 0:
                        wepDropNum2 = 2;
                        break;
                    case 1:
                        wepDropNum2 = 7;
                        break;
                    case 2:
                        wepDropNum2 = 12;
                        break;
                    case 3:
                        wepDropNum2 = 17;
                        break;
                }
                break;
            case 4:
                a2Text.enabled = true;
                ammo2 = ammoDict["laser"];
                ammo2Color4();
                switch (wep2Level)
                {
                    case 0:
                        wepDropNum2 = 3;
                        break;
                    case 1:
                        wepDropNum2 = 8;
                        break;
                    case 2:
                        wepDropNum2 = 13;
                        break;
                    case 3:
                        wepDropNum2 = 18;
                        break;
                }
                break;
            case 5:
                a2Text.enabled = true;
                ammo2 = 0;
                a2Text.text = "";
                switch (wep2Level)
                {
                    case 0:
                        wepDropNum2 = 4;
                        break;
                    case 1:
                        wepDropNum2 = 9;
                        break;
                    case 2:
                        wepDropNum2 = 14;
                        break;
                    case 3:
                        wepDropNum2 = 19;
                        break;
                }
                break;
        }
        //Ammo more than Max
        if (ammoDict["bullet"] > ammoMaxDict["bulletMax"])
            ammoDict["bullet"] = ammoMaxDict["bulletMax"];
        if (ammoDict["shell"] > ammoMaxDict["shellMax"])
            ammoDict["shell"] = ammoMaxDict["shellMax"];
        if (ammoDict["explosive"] > ammoMaxDict["explosiveMax"])
            ammoDict["explosive"] = ammoMaxDict["explosiveMax"];
        if (ammoDict["laser"] > ammoMaxDict["laserMax"])
            ammoDict["laser"] = ammoMaxDict["laserMax"];
        //Laser recharge
        if (laserCooldown >= 0)
            laserCooldown -= Time.deltaTime;
        if (ammoDict["laser"] < ammoMaxDict["laserMax"] && laserCooldown <= 0)
            ammoDict["laser"] += Time.deltaTime * 3;
    }
    //setting for the ammo color and weapon damage
    private void ammo1Color1()
    {
        if (ammoDict["bullet"] == ammoMaxDict["bulletMax"])
            a1Text.color = Color.green;
        if (ammoDict["bullet"] < ammoMaxDict["bulletMax"])
            a1Text.color = Color.yellow;
        if (ammoDict["bullet"] <= ammoMaxDict["bulletMax"] / 4)
            a1Text.color = Color.red;
    }
    private void ammo1Color2()
    {
        if (ammoDict["shell"] == ammoMaxDict["shellMax"])
            a1Text.color = Color.green;
        if (ammoDict["shell"] < ammoMaxDict["shellMax"])
            a1Text.color = Color.yellow;
        if (ammoDict["shell"] <= ammoMaxDict["shellMax"] / 4)
            a1Text.color = Color.red;

    }
    private void ammo1Color3()
    {
        if (ammoDict["explosive"] == ammoMaxDict["explosiveMax"])
            a1Text.color = Color.green;
        if (ammoDict["explosive"] < ammoMaxDict["explosiveMax"])
            a1Text.color = Color.yellow;
        if (ammoDict["explosive"] <= ammoMaxDict["explosiveMax"] / 4)
            a1Text.color = Color.red;
    }
    private void ammo1Color4()
    {
        if (ammoDict["laser"] == ammoMaxDict["laserMax"])
            a1Text.color = Color.green;
        if (ammoDict["laser"] < ammoMaxDict["laserMax"])
            a1Text.color = Color.yellow;
        if (ammoDict["laser"] <= ammoMaxDict["laserMax"] / 4)
            a1Text.color = Color.red;
    }
    private void ammo2Color1()
    {
        if (ammoDict["bullet"] == ammoMaxDict["bulletMax"])
            a2Text.color = Color.green;
        if (ammoDict["bullet"] < ammoMaxDict["bulletMax"])
            a2Text.color = Color.yellow;
        if (ammoDict["bullet"] <= ammoMaxDict["bulletMax"] / 4)
            a2Text.color = Color.red;
    }
    private void ammo2Color2()
    {
        if (ammoDict["shell"] == ammoMaxDict["shellMax"])
            a2Text.color = Color.green;
        if (ammoDict["shell"] < ammoMaxDict["shellMax"])
            a2Text.color = Color.yellow;
        if (ammoDict["shell"] <= ammoMaxDict["shellMax"] / 4)
            a2Text.color = Color.red;
    }
    private void ammo2Color3()
    {
        if (ammoDict["explosive"] == ammoMaxDict["explosiveMax"])
            a2Text.color = Color.green;
        if (ammoDict["explosive"] < ammoMaxDict["explosiveMax"])
            a2Text.color = Color.yellow;
        if (ammoDict["explosive"] <= ammoMaxDict["explosiveMax"] / 4)
            a2Text.color = Color.red;
    }
    private void ammo2Color4()
    {
        if (ammoDict["laser"] == ammoMaxDict["laserMax"])
            a2Text.color = Color.green;
        if (ammoDict["laser"] < ammoMaxDict["laserMax"])
            a2Text.color = Color.yellow;
        if (ammoDict["laser"] <= ammoMaxDict["laserMax"] / 4)
            a2Text.color = Color.red;
    }
    private void wepSwap(){
        //Weapon Swapping
        int temp = weapon1;
        int tempLv = wep1Level;
        int tempStack = ammoStack1;
        weapon1 = weapon2;
        wep1Level = wep2Level;
        ammoStack1 = ammoStack2;
        weapon2 = temp;
        wep2Level = tempLv;
        ammoStack2 = tempStack;
        
    }
    private void itemPickup()
    {
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            if (wepPickupDict["box"])
            {
                pickedUp = true;
                itemDrop();
                GetComponent<AudioSource>().PlayOneShot(boxSound);
            }
            if (wepPickupDict["hp"])
            {
                if (hp < hpMax)
                {
                    showEffect = true;
                    pickedUp = true;
                    hp += 25;
                    pickupText.GetComponent<TextMesh>().text = "HP + 25";
                    log.healthUse++;
                    GetComponent<AudioSource>().PlayOneShot(pickupSound);
                }
            }
            if (wepPickupDict["bAmmo"])
            {
                if (ammoDict["bullet"] < ammoMaxDict["bulletMax"])
                {
                    showEffect = true;
                    pickedUp = true;
                    float ammo = Random.Range(8, 18);
                    ammoDict["bullet"] += ammo;
                    pickupText.GetComponent<TextMesh>().text = "Bullets + " + ammo.ToString();
                    GetComponent<AudioSource>().PlayOneShot(pickupSound);
                }

            }
            if (wepPickupDict["shAmmo"])
            {
                if (ammoDict["shell"] < ammoMaxDict["shellMax"])
                {
                    showEffect = true;
                    pickedUp = true;
                    List<int> ammoList = new List<int> { 4, 6, 8, 10};
                    float ammo = ammoList[Random.Range(0, 4)];
                    ammoDict["shell"] += ammo;
                    pickupText.GetComponent<TextMesh>().text = "Shells + " + ammo.ToString();
                    GetComponent<AudioSource>().PlayOneShot(pickupSound);
                }
            }
            if (wepPickupDict["eAmmo"])
            {
                if (ammoDict["explosive"] < ammoMaxDict["explosiveMax"])
                {
                    showEffect = true;
                    pickedUp = true;
                    float ammo = Random.Range(2, 5);
                    ammoDict["explosive"] += ammo;
                    pickupText.GetComponent<TextMesh>().text = "Explosives + " + ammo.ToString();
                    GetComponent<AudioSource>().PlayOneShot(pickupSound);
                }
            }
            if (wepPickupDict["glitch"])
            {
                if(bp >= 50)
                {
                    pickedUp = true;
                    bp -= 50;
                    Instantiate(glitchItems[Random.Range(0, glitchItems.Length)], glitchObj.transform.position, Quaternion.identity);
                    log.shopUse++;
                    Debug.Log("shop: " + log.shopUse);
                    GetComponent<AudioSource>().PlayOneShot(pickupSound);
                }
            }
            if (wepPickupDict["cache"])
            {
                pickedUp = true;
                Instantiate(cacheItems[Random.Range(0, cacheItems.Length)], cacheObj.transform.position, Quaternion.identity);
                log.shopUse++;
                Debug.Log("shop: " + log.shopUse);
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
            }
            if (wepPickupDict["hUP"])
            {
                if (hpMax < 200)
                {
                    showEffect = true;
                    pickedUp = true;
                    hpMax += 10;
                    pickupText.GetComponent<TextMesh>().text = "Max HP + 10";
                    GetComponent<AudioSource>().PlayOneShot(pickupSound);
                }
            }
            if (wepPickupDict["pUP"])
            {
                if (ppMax < 200)
                {
                    showEffect = true;
                    pickedUp = true;
                    ppMax += 10;
                    pickupText.GetComponent<TextMesh>().text = "Max PP + 10";
                    GetComponent<AudioSource>().PlayOneShot(pickupSound);
                }
            }
        }
    }
    private void wepPickup()
    {
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            tempWep = wepDropNum;
            //LV0 Bullet Weapon Pickup
            if (wepPickupDict["b0"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 1 && wep1Level == 0)
                {
                    stackWep = true;
                    if (ammoDict["bullet"] >= ammoMaxDict["bulletMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["bullet"] < ammoMaxDict["bulletMax"])
                    {
                        StartCoroutine(pickedOff());
                        float ammo = Random.Range(6, 13);
                        ammoDict["bullet"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Bullets + " + ammo.ToString();
                    }
                }
                else
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 1;
                    wep1Level = 0;
                }
            }
            //LV0 Shell Weapon Pickup
            if (wepPickupDict["s0"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 2 && wep1Level == 0)
                {
                    stackWep = true;
                    if (ammoDict["shell"] >= ammoMaxDict["shellMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["shell"] < ammoMaxDict["shellMax"])
                    {
                        StartCoroutine(pickedOff());
                        List<int> ammoList = new List<int> { 4, 6, 8, 10 };
                        float ammo = ammoList[Random.Range(0, 4)];
                        ammoDict["shell"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Shells + " + ammo.ToString();
                    }
                }
                else if (weapon1 != 2)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 2;
                    wep1Level = 0;
                }
            }
            //LV0 Expolsive Weapon Pickup
            if (wepPickupDict["e0"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 3 && wep1Level == 0)
                {
                    stackWep = true;
                    if (ammoDict["explosive"] >= ammoMaxDict["explosiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["explosive"] < ammoMaxDict["explosiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        float ammo = Random.Range(2, 5);
                        ammoDict["explosive"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Explosives + " + ammo.ToString();
                    }
                }
                else if (weapon1 != 3)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 3;
                    wep1Level = 0;
                }
            }
            //LV0 Laser Weapon Pickup
            if (wepPickupDict["l0"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 4 && wep1Level == 0)
                {
                    stackWep = true;
                    StartCoroutine(pickedOff());
                    ammoStack1 += 1;
                    pickupText.GetComponent<TextMesh>().text = "Stack +1";
                }
                else if (weapon1 != 4)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 4;
                    wep1Level = 0;
                }
            }
            //LV0 Melee Weapon Pickup
            if (wepPickupDict["m0"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 5 && wep1Level == 0)
                {
                    stackWep = true;
                    StartCoroutine(pickedOff());
                    ammoStack1 += 1;
                    pickupText.GetComponent<TextMesh>().text = "Stack +1";
                }
                else if (weapon1 != 5)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 5;
                    wep1Level = 0;
                }

            }
            //LV1 Bullet Weapon Pickup
            if (wepPickupDict["b1"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 1 && wep1Level == 1)
                {
                    stackWep = true;
                    if (ammoDict["bullet"] >= ammoMaxDict["bulletMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["bullet"] < ammoMaxDict["bulletMax"])
                    {
                        StartCoroutine(pickedOff());
                        float ammo = Random.Range(6, 13);
                        ammoDict["bullet"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Bullets + " + ammo.ToString();
                    }
                }
                else
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 1;
                    wep1Level = 1;
                }
            }
            //LV1 Shell Weapon Pickup
            if (wepPickupDict["s1"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 2 && wep1Level == 1)
                {
                    stackWep = true;
                    if (ammoDict["shell"] >= ammoMaxDict["shellMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["shell"] < ammoMaxDict["shellMax"])
                    {
                        StartCoroutine(pickedOff());
                        List<int> ammoList = new List<int> { 4, 6, 8, 10 };
                        float ammo = ammoList[Random.Range(0, 4)];
                        ammoDict["shell"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Shells + " + ammo.ToString();
                    }
                }
                else if (weapon1 != 2)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 2;
                    wep1Level = 1;
                }
            }
            //LV1 Expolsive Weapon Pickup
            if (wepPickupDict["e1"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 3 && wep1Level == 1)
                {
                    stackWep = true;
                    if (ammoDict["explosive"] >= ammoMaxDict["explosiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["explosive"] < ammoMaxDict["explosiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        float ammo = Random.Range(2, 5);
                        ammoDict["explosive"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Explosives + " + ammo.ToString();
                    }
                }
                else if (weapon1 != 3)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 3;
                    wep1Level = 1;
                }
            }
            //LV1 Laser Weapon Pickup
            if (wepPickupDict["l1"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 4 && wep1Level == 1)
                {
                    stackWep = true;
                    StartCoroutine(pickedOff());
                    ammoStack1 += 1;
                    pickupText.GetComponent<TextMesh>().text = "Stack +1";
                }
                else if (weapon1 != 4)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 4;
                    wep1Level = 1;
                }
            }
            //LV1 Melee Weapon Pickup
            if (wepPickupDict["m1"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 5 && wep1Level == 1)
                {
                    stackWep = true;
                    StartCoroutine(pickedOff());
                    ammoStack1 += 1;
                    pickupText.GetComponent<TextMesh>().text = "Stack +1";
                }
                else if (weapon1 != 5)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 5;
                    wep1Level = 1;
                }

            }
            //LV2 Bullet Weapon Pickup
            if (wepPickupDict["b2"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 1 && wep1Level == 2)
                {
                    stackWep = true;
                    if (ammoDict["bullet"] >= ammoMaxDict["bulletMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["bullet"] < ammoMaxDict["bulletMax"])
                    {
                        StartCoroutine(pickedOff());
                        float ammo = Random.Range(6, 13);
                        ammoDict["bullet"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Bullets + " + ammo.ToString();
                    }
                }
                else
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 1;
                    wep1Level = 2;
                }
            }
            //LV2 Shell Weapon Pickup
            if (wepPickupDict["s2"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 2 && wep1Level == 2)
                {
                    stackWep = true;
                    if (ammoDict["shell"] >= ammoMaxDict["shellMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["shell"] < ammoMaxDict["shellMax"])
                    {
                        StartCoroutine(pickedOff());
                        List<int> ammoList = new List<int> { 4, 6, 8, 10 };
                        float ammo = ammoList[Random.Range(0, 4)];
                        ammoDict["shell"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Shells + " + ammo.ToString();
                    }
                }
                else if (weapon1 != 2)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 2;
                    wep1Level = 2;
                }
            }
            //LV2 Expolsive Weapon Pickup
            if (wepPickupDict["e2"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 3 && wep1Level == 2)
                {
                    stackWep = true;
                    if (ammoDict["explosive"] >= ammoMaxDict["explosiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["explosive"] < ammoMaxDict["explosiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        float ammo = Random.Range(2, 5);
                        ammoDict["explosive"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Explosives + " + ammo.ToString();
                    }
                }
                else if (weapon1 != 3)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 3;
                    wep1Level = 2;
                }
            }
            //LV2 Laser Weapon Pickup
            if (wepPickupDict["l2"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 4 && wep1Level == 2)
                {
                    stackWep = true;
                    StartCoroutine(pickedOff());
                    ammoStack1 += 1;
                    pickupText.GetComponent<TextMesh>().text = "Stack +1";
                }
                else if (weapon1 != 4)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 4;
                    wep1Level = 2;
                }
            }
            //LV2 Melee Weapon Pickup
            if (wepPickupDict["m2"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 5 && wep1Level == 2)
                {
                    stackWep = true;
                    StartCoroutine(pickedOff());
                    ammoStack1 += 1;
                    pickupText.GetComponent<TextMesh>().text = "Stack +1";
                }
                else if (weapon1 != 5)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 5;
                    wep1Level = 2;
                }

            }
            //LV3 Bullet Weapon Pickup
            if (wepPickupDict["b3"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 1 && wep1Level == 3)
                {
                    stackWep = true;
                    if (ammoDict["bullet"] >= ammoMaxDict["bulletMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["bullet"] < ammoMaxDict["bulletMax"])
                    {
                        StartCoroutine(pickedOff());
                        float ammo = Random.Range(6, 13);
                        ammoDict["bullet"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Bullets + " + ammo.ToString();
                    }
                }
                else
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 1;
                    wep1Level = 3;
                }
            }
            //LV3 Shell Weapon Pickup
            if (wepPickupDict["s3"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 2 && wep1Level == 3)
                {
                    stackWep = true;
                    if (ammoDict["shell"] >= ammoMaxDict["shellMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["shell"] < ammoMaxDict["shellMax"])
                    {
                        StartCoroutine(pickedOff());
                        List<int> ammoList = new List<int> { 4, 6, 8, 10 };
                        float ammo = ammoList[Random.Range(0, 4)];
                        ammoDict["shell"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Shells + " + ammo.ToString();
                    }
                }
                else if (weapon1 != 2)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 2;
                    wep1Level = 3;
                }
            }
            //LV3 Expolsive Weapon Pickup
            if (wepPickupDict["e3"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 3 && wep1Level == 3)
                {
                    stackWep = true;
                    if (ammoDict["explosive"] >= ammoMaxDict["explosiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["explosive"] < ammoMaxDict["explosiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        float ammo = Random.Range(2, 5);
                        ammoDict["explosive"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Explosives + " + ammo.ToString();
                    }
                }
                else if (weapon1 != 3)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 3;
                    wep1Level = 3;
                }
            }
            //LV3 Laser Weapon Pickup
            if (wepPickupDict["l3"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 4 && wep1Level == 3)
                {
                    stackWep = true;
                    StartCoroutine(pickedOff());
                    ammoStack1 += 1;
                    pickupText.GetComponent<TextMesh>().text = "Stack +1";
                }
                else if (weapon1 != 4)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 4;
                    wep1Level = 3;
                }
            }
            //LV3 Melee Weapon Pickup
            if (wepPickupDict["m3"] && storeCoolDown <= 0 && !onLab)
            {
                showEffect = true;
                pickedUp = true;
                GetComponent<AudioSource>().PlayOneShot(pickupSound);
                if (weapon1 == 5 && wep1Level == 3)
                {
                    stackWep = true;
                    StartCoroutine(pickedOff());
                    ammoStack1 += 1;
                    pickupText.GetComponent<TextMesh>().text = "Stack +1";
                }
                else if (weapon1 != 5)
                {
                    stackWep = false;
                    StartCoroutine(pickedOff());
                    weapon1 = 5;
                    wep1Level = 3;
                }

            }
        }
    }
    public void dropWeapon()
    {
        Instantiate(wepDrop[wepDropNum], transform.position, Quaternion.identity);
    }
    private void setBarSize()
    {
        hpBar.value = (hp / hpMax);
        ppBar.value = (pp / ppMax);
    }
    private void setAbility()
    {
        switch (actNum)
        {
            case 0:
                Active = "Firebomb";
                break;
            case 1:
                Active = "Freeze Blast";
                break;
            case 2:
                Active = "Bolt Dash";
                break;
            case 3:
                Active = "Tremor";
                break;
            case 4:
                Active = "Confusion";
                break;
        }
        switch (pasNum)
        {
            case 0:
                Passive = "Heatstroke";
                break;
            case 1:
                Passive = "Cold Zone";
                break;
            case 2:
                Passive = "Static Shock";
                break;
            case 3:
                Passive = "Earth Barrier";
                break;
            case 4:
                Passive = "Issuion Decoy";
                break;
        }
    }
    void itemDrop()
    {
        //item drop chance based on threat level
        int itemChance = Random.Range(0, 6);
        if (itemChance <= 4 && PlayerPrefs.GetInt("Threat Level") == 1)
            Instantiate(crateItems[Random.Range(0, crateItems.Length)], crate.transform.position, Quaternion.identity);
        else if (itemChance <= 2 && PlayerPrefs.GetInt("Threat Level") == 2)
            Instantiate(crateItems[Random.Range(0, crateItems.Length)], crate.transform.position, Quaternion.identity);
        else if (itemChance <= 1 && PlayerPrefs.GetInt("Threat Level") == 3)
            Instantiate(crateItems[Random.Range(0, crateItems.Length)], crate.transform.position, Quaternion.identity);
    }

    IEnumerator pickedOff()
    {
        yield return new WaitForSeconds(0.3f);
        if (!stackWep)
            Instantiate(wepDrop[tempWep], transform.position, Quaternion.identity);
    }
    IEnumerator burnDam()
    {
        yield return new WaitForSeconds(0.5f);
        Damage(Random.Range(4, 8));
    }
    IEnumerator exitLevel()
    {
        StartCoroutine(log.addEnemy());
        PlayerPrefs.SetInt("lastActive", actNum);
        PlayerPrefs.SetInt("lastPassive", pasNum);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("EndLevel", LoadSceneMode.Additive);
    }
  
    void shop()
    {
        if (onLab && !inStore && storeCoolDown <= 0)
        {
            if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
                inStore = true;
                Cursor.visible = true;
                
            }
                
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Colleting bio points
        if (other.CompareTag("MP"))
        {
            bp += 1;
            if (!pAbilDict["heat"] || !pAbilDict["cold"] || pAbilDict["shock"] || pAbilDict["earth"] || pAbilDict["decoy"])
                pp += 10;
            Destroy(other.gameObject);
            GetComponent<AudioSource>().PlayOneShot(bpSound);
        }
        //Exiting level
        if (other.CompareTag("Exit"))
        {
            StartCoroutine(exitLevel());
            GetComponent<AudioSource>().PlayOneShot(exitSound);
            hp += 20;
            player.stopMovement = true;
        }
        // Hit by enemy projectile
        if (other.CompareTag("E.Bullet"))
        {
            eBullet = other.gameObject.GetComponent<EnemyProjectile>();
            if (damCooldown <= 0)
                Damage(Random.Range(eBullet.minDamFinal, eBullet.maxDamFinal));
            Destroy(other.gameObject);
        }
        //Hit by burning projectile
        if (other.CompareTag("Burn"))
        {
            eBullet = other.gameObject.GetComponent<EnemyProjectile>();
            if(damCooldown <= 0)
                Damage(Random.Range(eBullet.minDamFinal, eBullet.maxDamFinal));
            //burn damage
            int chance = Random.Range(0, 4);
            if (chance >= 3)
                burnCoolDown = Random.Range(1f, 1.6f);
            Destroy(other.gameObject);
        }
        //Hit by tangle projectile
        if (other.CompareTag("Spore"))
        {
            eBullet = other.gameObject.GetComponent<EnemyProjectile>();
            if (damCooldown <= 0)
                Damage(Random.Range(eBullet.minDamFinal, eBullet.maxDamFinal));
            //tangle player
            int chance = Random.Range(0, 4);
            if (chance >= 3)
                tangleCooldown = 1f;
            Destroy(other.gameObject);
        }
        //Hit by bomb projectile
        if (other.CompareTag("E.Bomb") || other.CompareTag("Bomb"))
            Damage(Random.Range(10, 15));
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        //Heatlh Pickup
        if (other.CompareTag("Health")){
            if(!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Medkit";
            pickupText.SetActive(true);
            wepPickupDict["hp"] = true;
            if (pickedUp)
            {
                showEffect = true;
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //Bullet Pickup    
        if (other.CompareTag("BAmmo"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Bullet Ammo";
            pickupText.SetActive(true);
            wepPickupDict["bAmmo"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //Shell Pickup
        if (other.CompareTag("ShAmmo"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Shell Ammo";
            pickupText.SetActive(true);
            wepPickupDict["shAmmo"] = true;
            if (pickedUp){
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //Explosive Pickup
        if (other.CompareTag("EAmmo"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Explosive Ammo";
            pickupText.SetActive(true);
            wepPickupDict["eAmmo"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //Glitch Pickup
        if (other.CompareTag("Glitch")){
            pickupText.GetComponent<TextMesh>().text = "Random item 50BP";
            pickupText.SetActive(true);
            glitchObj = other.gameObject;
            wepPickupDict["glitch"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //Cache Pickup
        if (other.CompareTag("Cache"))
        {
            pickupText.SetActive(true);
            pickupText.GetComponent<TextMesh>().text = "Open the Cache";
            cacheObj = other.gameObject;
            wepPickupDict["cache"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //open crate
        if (other.CompareTag("Box"))
        {
            pickupText.SetActive(true);
            pickupText.GetComponent<TextMesh>().text = "Open";
            crate = other.gameObject;
            wepPickupDict["box"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //Lab Room(Shop)
        if (other.CompareTag("Shop"))
        {
            pickupText.GetComponent<TextMesh>().text = "Lab Room";
            pickupText.SetActive(true);
            onLab = true;
        }
        //Heatlh Upgrade Pickup
        if (other.CompareTag("H.UP"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "HP Booster";
            pickupText.SetActive(true);
            wepPickupDict["hUP"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //Psychic Upgrade Pickup
        if (other.CompareTag("P.Up"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "PP Booster";
            pickupText.SetActive(true);
            wepPickupDict["pUP"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV0 Bullet Weapon Pickup
        if (other.CompareTag("BWep0"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Pistol";
            pickupText.SetActive(true);
            wepPickupDict["b0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV0 Shell Weapon Pickup
        if (other.CompareTag("SWep0"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Sawed Off";
            pickupText.SetActive(true);
            wepPickupDict["s0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }

        }
        //LV0 Explosive Weapon Pickup
        if (other.CompareTag("EWep0"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Grenade Launcher";
            pickupText.SetActive(true);
            wepPickupDict["e0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
           
        }
        //LV0 Laser Weapon Pickup
        if (other.CompareTag("LWep0"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Plasma Blaster";
            pickupText.SetActive(true);
            wepPickupDict["l0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV0 Melee Weapon Pickup
        if (other.CompareTag("MWep0"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Baton";
            pickupText.SetActive(true);
            wepPickupDict["m0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV1 Bullet Weapon Pickup
        if (other.CompareTag("BWep1"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Revolver";
            pickupText.SetActive(true);
            wepPickupDict["b1"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV1 Shell Weapon Pickup
        if (other.CompareTag("SWep1"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Pump Shotgun";
            pickupText.SetActive(true);
            wepPickupDict["s1"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }

        }
        //LV1 Explosive Weapon Pickup
        if (other.CompareTag("EWep1"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Missle Launcher";
            pickupText.SetActive(true);
            wepPickupDict["e1"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }

        }
        //LV1 Laser Weapon Pickup
        if (other.CompareTag("LWep1"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Laser Repeater";
            pickupText.SetActive(true);
            wepPickupDict["l1"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV1 Melee Weapon Pickup
        if (other.CompareTag("MWep1"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Hand Axe";
            pickupText.SetActive(true);
            wepPickupDict["m1"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV2 Bullet Weapon Pickup
        if (other.CompareTag("BWep2"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Magnum";
            pickupText.SetActive(true);
            wepPickupDict["b2"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV2 Shell Weapon Pickup
        if (other.CompareTag("SWep2"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Riot Shotgun";
            pickupText.SetActive(true);
            wepPickupDict["s2"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }

        }
        //LV2 Explosive Weapon Pickup
        if (other.CompareTag("EWep2"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Mine Launcher";
            pickupText.SetActive(true);
            wepPickupDict["e2"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }

        }
        //LV2 Laser Weapon Pickup
        if (other.CompareTag("LWep2"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Beam Rifle";
            pickupText.SetActive(true);
            wepPickupDict["l2"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV2 Melee Weapon Pickup
        if (other.CompareTag("MWep2"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Sledgehammer";
            pickupText.SetActive(true);
            wepPickupDict["m2"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV3 Bullet Weapon Pickup
        if (other.CompareTag("BWep3"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Machnie Gun";
            pickupText.SetActive(true);
            wepPickupDict["b3"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV3 Shell Weapon Pickup
        if (other.CompareTag("SWep3"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Quad Barrel";
            pickupText.SetActive(true);
            wepPickupDict["s3"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }

        }
        //LV3 Explosive Weapon Pickup
        if (other.CompareTag("EWep3"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Heat Seeker";
            pickupText.SetActive(true);
            wepPickupDict["e3"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }

        }
        //LV3 Laser Weapon Pickup
        if (other.CompareTag("LWep3"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Rail Gun";
            pickupText.SetActive(true);
            wepPickupDict["l3"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
        //LV3 Melee Weapon Pickup
        if (other.CompareTag("MWep3"))
        {
            if (!showEffect)
                pickupText.GetComponent<TextMesh>().text = "Katana";
            pickupText.SetActive(true);
            wepPickupDict["m3"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
                pickedUp = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
         if (other.CompareTag("Health"))
        {
            StartCoroutine(textOff());
            wepPickupDict["hp"] = false;
        }
        if (other.CompareTag("BAmmo"))
        {
            StartCoroutine(textOff());
            wepPickupDict["bAmmo"] = false;
        }
        if (other.CompareTag("ShAmmo"))
        {
            StartCoroutine(textOff());
            wepPickupDict["shAmmo"] = false;
        }
        if (other.CompareTag("EAmmo"))
        {
            StartCoroutine(textOff());
            wepPickupDict["eAmmo"] = false;
        }
        if (other.CompareTag("Glitch"))
        {
            StartCoroutine(textOff());
            wepPickupDict["glitch"] = false;
        }
        if (other.CompareTag("Box"))
        {
            StartCoroutine(textOff());
            wepPickupDict["box"] = false;
        }
        if (other.CompareTag("H.UP"))
        {
            StartCoroutine(textOff());
            wepPickupDict["hUP"] = false;
        }
        if (other.CompareTag("P.Up"))
        {
            StartCoroutine(textOff());
            wepPickupDict["pUP"] = false;
        }
        if (other.CompareTag("Cache"))
        {
            StartCoroutine(textOff());
            wepPickupDict["cache"] = false;
        }
        if (other.CompareTag("Shop"))
        {
            onLab = false;
            StartCoroutine(textOff());
        }
        if (other.CompareTag("BWep0"))
        {
            StartCoroutine(textOff());
            wepPickupDict["b0"] = false;
        }
        if (other.CompareTag("SWep0"))
        {
            StartCoroutine(textOff());
            wepPickupDict["s0"] = false;
        }
        if (other.CompareTag("EWep0"))
        {
            StartCoroutine(textOff());
            wepPickupDict["e0"] = false;
        }
        if (other.CompareTag("LWep0"))
        {
            StartCoroutine(textOff());
            wepPickupDict["l0"] = false;
        }
        if (other.CompareTag("MWep0"))
        {
            StartCoroutine(textOff());
            wepPickupDict["m0"] = false;
        }
        if (other.CompareTag("BWep1"))
        {
            StartCoroutine(textOff());
            wepPickupDict["b1"] = false;
        }
        if (other.CompareTag("SWep1"))
        {
            StartCoroutine(textOff());
            wepPickupDict["s1"] = false;
        }
        if (other.CompareTag("EWep1"))
        {
            StartCoroutine(textOff());
            wepPickupDict["e1"] = false;
        }
        if (other.CompareTag("LWep1"))
        {
            StartCoroutine(textOff());
            wepPickupDict["l1"] = false;
        }
        if (other.CompareTag("MWep1"))
        {
            StartCoroutine(textOff());
            wepPickupDict["m1"] = false;
        }
        if (other.CompareTag("BWep2"))
        {
            StartCoroutine(textOff());
            wepPickupDict["b2"] = false;
        }
        if (other.CompareTag("SWep2"))
        {
            StartCoroutine(textOff());
            wepPickupDict["s2"] = false;
        }
        if (other.CompareTag("EWep2"))
        {
            StartCoroutine(textOff());
            wepPickupDict["e2"] = false;
        }
        if (other.CompareTag("LWep2"))
        {
            StartCoroutine(textOff());
            wepPickupDict["l2"] = false;
        }
        if (other.CompareTag("MWep2"))
        {
            StartCoroutine(textOff());
            wepPickupDict["m2"] = false;
        }
        if (other.CompareTag("BWep3"))
        {
            StartCoroutine(textOff());
            wepPickupDict["b3"] = false;
        }
        if (other.CompareTag("SWep3"))
        {
            StartCoroutine(textOff());
            wepPickupDict["s3"] = false;
        }
        if (other.CompareTag("EWep3"))
        {
            StartCoroutine(textOff());
            wepPickupDict["e3"] = false;
        }
        if (other.CompareTag("LWep3"))
        {
            StartCoroutine(textOff());
            wepPickupDict["l3"] = false;
        }
        if (other.CompareTag("MWep3"))
        {
            StartCoroutine(textOff());
            wepPickupDict["m3"] = false;
        }
    }
    IEnumerator textOff()
    {
        yield return new WaitForSeconds(0.4f);
        pickupText.SetActive(false);
        showEffect = false;
    }
   private void controlInputs()
    {
        //Weapon Swapping
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Joystick1Button3))
            wepSwap();
        //Set passive on
        if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.Joystick1Button8)){
            //Passive heat
            if (Passive == "Heatstroke"){
                if (passiveCooldown <= 0 && activeCooldown <=0 && pp > 40){
                    if (!pAbilDict["heat"]){
                        pp -= 20;
                        pAbilDict["heat"] = true;
                    }
                }
                else if (pAbilDict["heat"])
                    pAbilDict["heat"] = false;
            }
            //Passive cold
            if (Passive == "Cold Zone"){
                if (passiveCooldown <= 0 && activeCooldown <= 0 && pp > 40){
                    if (!pAbilDict["cold"]){
                        pp -= 20;
                        pAbilDict["cold"] = true;
                    }
                }
                else if (pAbilDict["cold"])
                    pAbilDict["cold"] = false;
            }
            //Passive shock
            if (Passive == "Static Shock")
            {
                if (passiveCooldown <= 0 && activeCooldown <= 0 && pp > 40){
                    if (!pAbilDict["shock"]){
                        pp -= 20;
                        pAbilDict["shock"] = true;
                    }
                }
                else if (pAbilDict["shock"])
                    pAbilDict["shock"] = false;
            }
            //Passive earth
            if (Passive == "Earth Barrier"){
                if (passiveCooldown <= 0 && activeCooldown <= 0 && pp > 40)
                {
                    if (!pAbilDict["earth"])
                    {
                        pp -= 20;
                        pAbilDict["earth"] = true;
                    }
                }
                else if (pAbilDict["earth"])
                    pAbilDict["earth"] = false;
            }
            //Passive decoy
            if (Passive == "Issuion Decoy"){
                if (passiveCooldown <= 0 && activeCooldown <= 0 && pp > 40)
                {
                    if (!pAbilDict["decoy"])
                    {
                        pp -= 20;
                        decoy.SetActive(true);
                        decoy.transform.position = transform.position;
                        pAbilDict["decoy"] = true;
                    }
                }
                else if (pAbilDict["decoy"])
                {
                    decoy.SetActive(false);
                    pAbilDict["decoy"] = false;
                }
                    

            }
        }
    }

}
