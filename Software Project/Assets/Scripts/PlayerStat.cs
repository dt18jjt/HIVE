﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStat : MonoBehaviour
{
    public int hp = 100, hpMax = 100, ppMax = 100, bp = 0;
    public float pp = 100;
    //Ammo display
    public float ammo1 , ammo2;
    //Weapon values
    public int weapon1, weapon2, wep1Level = 0, wep2Level = 0 , ammoStack1 = 0, ammoStack2 = 0;
    //cooldowns
    public float damCooldown, pulseCooldown, meleeCooldown, laserCooldown = 0f, activeCooldown = 0f, passiveCooldown = 0f,
        shockCoolDown = 0f, passiveTimer = 1f, burnCoolDown, tangleCooldown, storeCoolDown = 0f;
    public string Active, Passive;
    //Room conditions
    public bool wepJam = false, powBlock = false, suddenDeath = false, shockDam = false, pickedUp = false, stackWep = false,
        inStore = false, enemyBuff = false;
    public int tempWep;
    public Dictionary<string, bool> pAbilDict = new Dictionary<string, bool>(); // Passive abilities Dictionary
    public Dictionary<string, bool> wepPickupDict = new Dictionary<string, bool>(); // Passive abilities Dictionary
    public Dictionary<string, int> damDict = new Dictionary<string, int>(); // Damage Dictionary
    public Dictionary<string, int> wepLevelDict = new Dictionary<string, int>(); // Damage Dictionary
    public Dictionary<string, float> ammoDict = new Dictionary<string, float>(); // Ammo Dictionary
    public Dictionary<string, int> ammoMaxDict = new Dictionary<string, int>(); // Max Ammo Dictionary
    public Text hpText, ppText, mpText, a1Text, a2Text;
    public Text activeText;
    public Text passiveText;
    public Color activeColor;
    public Color passiveColor;
    public GameObject hitEffect; //hit particle
    public GameObject pulse, jamImage, pickupText, decoy;
    public GameObject[] glitchItems;
    public GameObject[] wepDrop;
    public List<Transform> cEmenies;
    // Start is called before the first frame update
    void Start(){
        //Set text at start
        pickupText.SetActive(false);
        //Setting Damage Values
        {
            damDict.Add("pulseDam", 5);
            damDict.Add("bulletDam", 10);
            damDict.Add("shellDam", 10);
            damDict.Add("expolsiveDam", 20);
            damDict.Add("fireDam", 10);
            damDict.Add("freezeDam", 10);
            damDict.Add("boltDam", 10);
            damDict.Add("confuseDam", 5);
            damDict.Add("tremorDam", 5);
            damDict.Add("laserDam", 5);
            damDict.Add("meleeDam", 10);
        }
        //Setting Ammo Values
        {
            ammoDict.Add("bullet", 24);
            ammoDict.Add("shell", 10);
            ammoDict.Add("expolsive", 4);
            ammoDict.Add("laser", 24);
        }
        //Setting Max Ammo Values
        {
            ammoMaxDict.Add("bulletMax", 32);
            ammoMaxDict.Add("shellMax", 16);
            ammoMaxDict.Add("expolsiveMax", 8);
            ammoMaxDict.Add("laserMax", 24);
        }
        //Setting Weapon Level values 
        {
            wepLevelDict.Add("bullet", 0);
            wepLevelDict.Add("shell", 0);
            wepLevelDict.Add("expolsive", 0);
            wepLevelDict.Add("laser", 0);
            wepLevelDict.Add("melee", 0);
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
        }
        decoy.SetActive(false);
        
    }

    // Update is called once per frame
    void Update(){
        Vector2 pickupPos = new Vector2(transform.position.x, transform.position.y + 15);
        pickupText.transform.position = pickupPos;
        //keeping value player preferences
        PlayerPrefs.SetInt("BP", bp);
        PlayerPrefs.SetInt("HP", hp);
        PlayerPrefs.SetInt("HPMax", hpMax);
        PlayerPrefs.SetInt("PPMax", ppMax);
        setText();
        //Player Health set to zero and max
        if (hp <= 0){
            hp = 0;
            //Destroy(gameObject, 1.5f);
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
        if(pAbilDict["heat"] || pAbilDict["cold"] || pAbilDict["shock"] || pAbilDict["earth"] || pAbilDict["decoy"])
        {
            passiveCooldown = 1;
            passiveTimer -= Time.deltaTime;
            if(passiveTimer <= 0){
                pp -= 20;
                passiveTimer = 1f;
            }
        }
        //Psyhic recharge
        if (pp < ppMax && activeCooldown > 0)
            activeCooldown -= Time.deltaTime;
        if (pp < ppMax && passiveCooldown > 0){
            if(!pAbilDict["heat"] || !pAbilDict["cold"] || pAbilDict["shock"] || pAbilDict["earth"] || pAbilDict["decoy"])
                passiveCooldown -= Time.deltaTime;
        }
        if (pp < ppMax && activeCooldown <= 0 && passiveCooldown <= 0){
            activeCooldown = 0;
            passiveCooldown = 0;
            pp += Time.deltaTime * 10f;

        }
        //set ammo to weapon equipped
        equippedAmmo();
        controlInputs();
        //Damage CoolDown
        GetComponent<SpriteRenderer>().color = (damCooldown > 0) ? Color.yellow : Color.white;
        if (damCooldown > 0)
            damCooldown -= Time.deltaTime;
        //Pulse CoolDown
        if (pulseCooldown > 0)
            pulseCooldown -= Time.deltaTime;
        //Weapon Jam
        jamImage.SetActive((wepJam) ? true : false);
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
        if (burnCoolDown > 0)
        {
            burnCoolDown -= Time.deltaTime;
            StartCoroutine(burnDam());
        }
        if (tangleCooldown > 0)
            tangleCooldown -= Time.deltaTime;
    }
    private void setText()
    {
        hpText.text = "HP:" + hp.ToString() + "/" + hpMax.ToString();
        ppText.text = "PP:" + pp.ToString("F0") + "/"+ ppMax.ToString();
        mpText.text = bp.ToString();
        a1Text.text = ammo1.ToString("F0");
        a2Text.text = ammo2.ToString("F0");
        activeText.text = Active;
        passiveText.text = Passive;
        activeText.color = (powBlock) ? Color.gray : activeColor;
        passiveText.color= (powBlock) ? Color.gray : passiveColor;

    }
    public void Damage(int dam){
        //Player Damage
        if(hp > 0 && damCooldown <= 0){
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            hit.GetComponent<ParticleSystem>().Play();
            Destroy(hit, 1f);
            if (pAbilDict["shock"] && !shockDam)
                shockDam = true;
            if (!pAbilDict["earth"])
            {
                hp -= dam;
                damCooldown = 1.5f;
            }
        }
        
    }
    private void equippedAmmo()
    {
        //Primary weapon
        if (weapon1 == 1)
        {
            a1Text.enabled = true;
            ammo1 = ammoDict["bullet"];
            if (ammoDict["bullet"] == ammoMaxDict["bulletMax"])
                a1Text.color = Color.green;
            if (ammoDict["bullet"] < ammoMaxDict["bulletMax"])
                a1Text.color = Color.yellow;
            if (ammoDict["bullet"] <= ammoMaxDict["bulletMax"] / 4)
                a1Text.color = Color.red;
        }
        if (weapon1 == 2)
        {
            a1Text.enabled = true;
            ammo1 = ammoDict["shell"];
            if (ammoDict["shell"] == ammoMaxDict["shellMax"])
                a1Text.color = Color.green;
            if (ammoDict["shell"] < ammoMaxDict["shellMax"])
                a1Text.color = Color.yellow;
            if (ammoDict["shell"] <= ammoMaxDict["shellMax"] / 4)
                a1Text.color = Color.red;
        }
        if (weapon1 == 3)
        {
            a1Text.enabled = true;
            ammo1 = ammoDict["expolsive"];
            if (ammoDict["expolsive"] == ammoMaxDict["expolsiveMax"])
                a1Text.color = Color.green;
            if (ammoDict["expolsive"] < ammoMaxDict["expolsiveMax"])
                a1Text.color = Color.yellow;
            if (ammoDict["expolsive"] <= ammoMaxDict["expolsiveMax"] / 4)
                a1Text.color = Color.red;
        }
        if (weapon1 == 4)
        {
            a1Text.enabled = true;
            ammo1 = ammoDict["laser"];
            if (ammoDict["laser"] == ammoMaxDict["laserMax"])
                a1Text.color = Color.green;
            if (ammoDict["laser"] < ammoMaxDict["laserMax"])
                a1Text.color = Color.yellow;
            if (ammoDict["laser"] <= ammoMaxDict["laserMax"] / 4)
                a1Text.color = Color.red;
        }
        if (weapon1 == 5)
            a1Text.enabled = false;
        //Secondary weapon
        if (weapon2 == 1)
        {
            a2Text.enabled = true;
            ammo2 = ammoDict["bullet"];
            if (ammoDict["bullet"] == ammoMaxDict["bulletMax"])
                a2Text.color = Color.green;
            if (ammoDict["bullet"] < ammoMaxDict["bulletMax"])
                a2Text.color = Color.yellow;
            if (ammoDict["bullet"] <= ammoMaxDict["bulletMax"] / 4)
                a2Text.color = Color.red;
        }
        if (weapon2 == 2)
        {
            a2Text.enabled = true;
            ammo2 = ammoDict["shell"];
            if (ammoDict["shell"] == ammoMaxDict["shellMax"])
                a2Text.color = Color.green;
            if (ammoDict["shell"] < ammoMaxDict["shellMax"])
                a2Text.color = Color.yellow;
            if (ammoDict["shell"] <= ammoMaxDict["shellMax"] / 4)
                a2Text.color = Color.red;
        }
        if (weapon2 == 3)
        {
            a2Text.enabled = true;
            ammo2 = ammoDict["expolsive"];
            if (ammoDict["expolsive"] == ammoMaxDict["expolsiveMax"])
                a2Text.color = Color.green;
            if (ammoDict["expolsive"] < ammoMaxDict["expolsiveMax"])
                a2Text.color = Color.yellow;
            if (ammoDict["expolsive"] <= ammoMaxDict["expolsiveMax"] / 4)
                a2Text.color = Color.red;
        }
        if (weapon2 == 4)
        {
            a2Text.enabled = true;
            ammo2 = ammoDict["laser"];
            if (ammoDict["laser"] == ammoMaxDict["laserMax"])
                a2Text.color = Color.green;
            if (ammoDict["laser"] < ammoMaxDict["laserMax"])
                a2Text.color = Color.yellow;
            if (ammoDict["laser"] <= ammoMaxDict["laserMax"] / 4)
                a2Text.color = Color.red;
        }
        if (weapon2 == 5)
            a2Text.enabled = false;
        //Ammo more than Max
        if (ammoDict["bullet"] > ammoMaxDict["bulletMax"])
            ammoDict["bullet"] = ammoMaxDict["bulletMax"];
        if (ammoDict["shell"] > ammoMaxDict["shellMax"])
            ammoDict["shell"] = ammoMaxDict["shellMax"];
        if (ammoDict["expolsive"] > ammoMaxDict["expolsiveMax"])
            ammoDict["expolsive"] = ammoMaxDict["expolsiveMax"];
        if (ammoDict["laser"] > ammoMaxDict["laserMax"])
            ammoDict["laser"] = ammoMaxDict["laserMax"];
        //Laser recharge
        if (laserCooldown >= 0)
            laserCooldown -= Time.deltaTime;
        if (ammoDict["laser"] < ammoMaxDict["laserMax"] && laserCooldown <= 0)
            ammoDict["laser"] += Time.deltaTime;
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
    private void wepPickup()
    {
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            tempWep = weapon1;
            if (wepPickupDict["b0"])
        {
                pickedUp = true;
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
                        wep1Level = 0;
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
            if (wepPickupDict["s0"])
            {
                pickedUp = true;
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
                        float ammo = Random.Range(4, 11);
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
            if (wepPickupDict["e0"])
            {
                pickedUp = true;
                if (weapon1 == 3 && wep1Level == 0)
                {
                    stackWep = true;
                    if (ammoDict["expolsive"] >= ammoMaxDict["expolsiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        ammoStack1 += 1;
                        pickupText.GetComponent<TextMesh>().text = "Stack +1";
                    }
                    else if (ammoDict["expolsive"] < ammoMaxDict["expolsiveMax"])
                    {
                        StartCoroutine(pickedOff());
                        float ammo = Random.Range(2, 5);
                        ammoDict["expolsive"] += ammo;
                        pickupText.GetComponent<TextMesh>().text = "Expolsives + " + ammo.ToString();
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
            if (wepPickupDict["l0"])
            {

                pickedUp = true;
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
            if (wepPickupDict["m0"])
            {

                pickedUp = true;
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
        }
    }
    IEnumerator pickedOff()
    {
        
        yield return new WaitForSeconds(0.1f);
        pickedUp = false;
        if (!stackWep)
            Instantiate(wepDrop[tempWep - 1], transform.position, Quaternion.identity);
       
    }
    IEnumerator burnDam()
    {
        yield return new WaitForSeconds(0.5f);
        Damage(Random.Range(4, 8));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MP"))
        {
            bp += 1;
            if (!pAbilDict["heat"] || !pAbilDict["cold"] || pAbilDict["shock"] || pAbilDict["earth"] || pAbilDict["decoy"])
                pp += 10;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Exit"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (other.CompareTag("Burn"))
            burnCoolDown = Random.Range(1f, 1.6f);
        if (other.CompareTag("Spore"))
        {
            int chance = Random.Range(0, 4);
            if (chance >= 3)
            {
                tangleCooldown = 1f;
                Debug.Log("tangled");
            }
                
        }
    }
    private IEnumerator pulseAction()
    {
        pulse.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        pulse.SetActive(false);
        pulseCooldown = 1f;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        //Heatlh Pickup
        if (other.CompareTag("Health")){
            pickupText.GetComponent<TextMesh>().text = "Medkit";
            pickupText.SetActive(true);
            if (Input.GetKey(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                if (hp < hpMax)
                {
                    hp += 25;
                    Destroy(other.gameObject);
                    pickupText.GetComponent<TextMesh>().text = "HP + 25";
                }
            }
        }
        //Bullet Pickup    
        if (other.CompareTag("BAmmo"))
        {
            pickupText.GetComponent<TextMesh>().text = "Bullet Ammo";
            pickupText.SetActive(true);
            if (Input.GetKey(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                if (ammoDict["bullet"] < ammoMaxDict["bulletMax"]){
                    float ammo = Random.Range(6, 13);
                    ammoDict["bullet"] += ammo;
                    pickupText.GetComponent<TextMesh>().text = "Bullets + " + ammo.ToString();
                    Destroy(other.gameObject);
                }
            }
        }
        //Shell Pickup
        if (other.CompareTag("ShAmmo"))
        {
            pickupText.GetComponent<TextMesh>().text = "Shell Ammo";
            pickupText.SetActive(true);
            if (Input.GetKey(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0)){
                if (ammoDict["shell"] < ammoMaxDict["shellMax"])
                {
                    float ammo = Random.Range(4, 11);
                    ammoDict["shell"] += ammo;
                    pickupText.GetComponent<TextMesh>().text = "Shells + " + ammo.ToString();
                    Destroy(other.gameObject);
                }
            }
        }
        //Expolsive Pickup
        if (other.CompareTag("EAmmo"))
        {
            pickupText.GetComponent<TextMesh>().text = "Expolsive Ammo";
            pickupText.SetActive(true);
            if (Input.GetKey(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                if (ammoDict["expolsive"] < ammoMaxDict["expolsiveMax"])
                {
                    float ammo = Random.Range(2, 5);
                    ammoDict["expolsive"] += ammo;
                    pickupText.GetComponent<TextMesh>().text = "Expolsives + " + ammo.ToString();
                    Destroy(other.gameObject);
                }
            }
        }
        //Glitch Pickup
        if (other.CompareTag("Glitch")){
            if (Input.GetKeyUp(KeyCode.E) && bp >= 50|| Input.GetKeyUp(KeyCode.Joystick1Button0) && bp >= 50)
            {
                bp -= 50;
                Instantiate(glitchItems[Random.Range(0, glitchItems.Length)], other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }
        //LV0 Bullet Weapon Pickup
        if (other.CompareTag("BWep0"))
        {
            pickupText.GetComponent<TextMesh>().text = "Pistol";
            pickupText.SetActive(true);
            wepPickupDict["b0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
            }
            
        }
        //LV0 Shell Weapon Pickup
        if (other.CompareTag("SWep0"))
        {
            pickupText.GetComponent<TextMesh>().text = "Sawed Off";
            pickupText.SetActive(true);
            wepPickupDict["s0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
            }

        }
        //LV0 Explosive Weapon Pickup
        if (other.CompareTag("EWep0"))
        {
            pickupText.GetComponent<TextMesh>().text = "Grenade Launcher";
            pickupText.SetActive(true);
            wepPickupDict["e0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
            }
           
        }
        //LV0 Laser Weapon Pickup
        if (other.CompareTag("LWep0"))
        {
            pickupText.GetComponent<TextMesh>().text = "Plasma Blaster";
            pickupText.SetActive(true);
            wepPickupDict["l0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
            }
        }
        //LV0 Melee Weapon Pickup
        if (other.CompareTag("MWep0"))
        {
            pickupText.GetComponent<TextMesh>().text = "Baton";
            pickupText.SetActive(true);
            wepPickupDict["m0"] = true;
            if (pickedUp)
            {
                Destroy(other.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
         if (other.CompareTag("Health"))
            StartCoroutine(textOff());
        if (other.CompareTag("BAmmo"))
            StartCoroutine(textOff());
        if (other.CompareTag("ShAmmo"))
            StartCoroutine(textOff());
        if (other.CompareTag("EAmmo"))
            StartCoroutine(textOff());
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
    }
    IEnumerator textOff()
    {
        yield return new WaitForSeconds(0.4f);
        pickupText.SetActive(false);
    }
   private void controlInputs()
    {
        //Weapon Swapping
        if (Input.GetKeyUp(KeyCode.Q) && ammo2 > 0 || Input.GetKeyUp(KeyCode.Joystick1Button3) && ammo2 > 0)
            wepSwap();
        //Psychic pulse
        if (Input.GetKeyUp(KeyCode.F) && pulseCooldown <= 0 || Input.GetKeyUp(KeyCode.Joystick1Button9) && pulseCooldown <= 0)
            StartCoroutine(pulseAction());
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
