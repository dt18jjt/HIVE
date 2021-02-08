using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStat : MonoBehaviour
{
    public int hp = 100;
    public int hpMax = 100;
    public float pp = 100;
    public int ppMax = 100;
    public int bp = 0;
    //Ammo display
    public float ammo1;
    public float ammo2;
    //Weapon display
    public int weapon1;
    public int weapon2;
    public float damCooldown;
    public float pulseCooldown;
    public float laserCooldown = 0f;
    public float activeCooldown = 0f;
    public float passiveCooldown = 0f;
    public float shockCoolDown = 0f;
    public float passiveTimer = 1f;
    public string Active;
    public string Passive;
    //Passive abilities
    public bool heat = false;
    public bool cold = false;
    public bool shock = false;
    //Room conditions
    public bool wepJam = false;
    public bool powBlock = false;
    public bool suddenDeath = false;
    public bool shockDam = false;
    public Dictionary<string, int> damDict = new Dictionary<string, int>(); // Damage Dictionary
    public Dictionary<string, int> wepLevelDict = new Dictionary<string, int>(); // Damage Dictionary
    public Dictionary<string, float> ammoDict = new Dictionary<string, float>(); // Ammo Dictionary
    public Dictionary<string, int> ammoMaxDict = new Dictionary<string, int>(); // Max Ammo Dictionary
    public Text hpText;
    public Text ppText;
    public Text mpText;
    public Text a1Text;
    public Text a2Text;
    public Text activeText;
    public Text passiveText;
    public Color activeColor;
    public Color passiveColor;
    public GameObject hitEffect; //hit particle
    public GameObject pulse;
    public GameObject jamImage;
    public GameObject sDeathImage;
    public GameObject[] glitchItems;
    // Start is called before the first frame update
    void Start(){
        //Setting Damage Values
        damDict.Add("pulseDam", 5);
        damDict.Add("bulletDam", 10);
        damDict.Add("shellDam", 10);
        damDict.Add("expolsiveDam", 20);
        damDict.Add("fireDam", 10);
        damDict.Add("freezeDam", 10);
        damDict.Add("boltDam", 10);
        damDict.Add("laserDam", 5);
        damDict.Add("meleeDam", 10);
        //Setting Ammo Values
        ammoDict.Add("bullet", 32);
        ammoDict.Add("shell", 16);
        ammoDict.Add("expolsive", 8);
        ammoDict.Add("laser", 24);
        //Setting Max Ammo Values
        ammoMaxDict.Add("bulletMax", 32);
        ammoMaxDict.Add("shellMax", 16);
        ammoMaxDict.Add("expolsiveMax", 8);
        ammoMaxDict.Add("laserMax", 24);
        //Setting Weapon Level values 
        wepLevelDict.Add("bullet", 0);
        wepLevelDict.Add("shell", 0);
        wepLevelDict.Add("expolsive", 0);
        wepLevelDict.Add("laser", 0);
        wepLevelDict.Add("melee", 0);
    }

    // Update is called once per frame
    void Update(){
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
            heat = false;
            cold = false;
            shock = false;
        }
        if (pp > ppMax)
            pp = ppMax;
        //Passive decrease
        if(heat || cold || shock){
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
            if(!heat)
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
        if (damCooldown > 0)
        {
            damCooldown -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
            GetComponent<SpriteRenderer>().color = Color.white;
        //Pulse CoolDown
        if (pulseCooldown > 0)
            pulseCooldown -= Time.deltaTime;
        //Weapon Jam
        if (wepJam)
            jamImage.SetActive(true);
        else
            jamImage.SetActive(false);
        //Sudden Death
        if (suddenDeath)
            sDeathImage.SetActive(true);
        else
            sDeathImage.SetActive(false);
        //shockCooldown
        if (shockCoolDown > 0)
            shockCoolDown -= Time.deltaTime;
        if (shockCoolDown <= 0)
            shockDam = false;
    }
    private void setText()
    {
        hpText.text = "HP:" + hp.ToString() + "/100";
        ppText.text = "PP:" + pp.ToString("F0") + "/100";
        mpText.text = bp.ToString();
        a1Text.text = ammo1.ToString("F0");
        a2Text.text = ammo2.ToString("F0");
        activeText.text = Active;
        passiveText.text = Passive;
        if (powBlock){
            activeText.color = Color.gray;
            passiveText.color = Color.gray;
        }
        else{
            activeText.color = activeColor;
            passiveText.color = passiveColor;
        }
    }
    public void Damage(int dam){
        //Player Damage
        if(hp > 0 && damCooldown <= 0){
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            hit.GetComponent<ParticleSystem>().Play();
            Destroy(hit, 1f);
            if (shock && !shockDam)
                shockDam = true;
            if (!suddenDeath){
                hp -= dam;
                damCooldown = 1.5f;
            }
            else
                hp = 0;
        }
        
    }
    public void equippedAmmo()
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
    public void wepSwap(){
        //Weapon Swapping
        int temp = weapon1;
        weapon1 = weapon2;
        weapon2 = temp;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MP"))
        {
            bp += 1;
            if (!heat || !cold)
                pp += 10;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Exit"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator pulseAction()
    {
        pulse.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        pulse.SetActive(false);
        pulseCooldown = 1f;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Health"))
            if (hp < hpMax){
                if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0)){
                    hp += 25;
                    Destroy(other.gameObject);
                }
            }
        if (other.CompareTag("BAmmo"))
            if (ammoDict["bullet"] < ammoMaxDict["bulletMax"]){
                if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0)){
                    ammoDict["bullet"] += Random.Range(10, 21);
                    Destroy(other.gameObject);
                }
            }
        if (other.CompareTag("ShAmmo"))
            if (ammoDict["shell"] < ammoMaxDict["shellMax"]){
                //Debug.Log("Picked up");
                if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0)){
                    ammoDict["shell"] += Random.Range(6, 13);
                    Destroy(other.gameObject);
                }
            }
        if (other.CompareTag("Glitch")){
            if (Input.GetKeyUp(KeyCode.E) && bp >= 50|| Input.GetKeyUp(KeyCode.Joystick1Button0) && bp >= 50)
            {
                bp -= 50;
                Instantiate(glitchItems[Random.Range(0, glitchItems.Length)], other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }
    }
   private void controlInputs()
    {
        //Active switch cheat
        if (Input.GetKeyUp(KeyCode.Alpha1))
            Active = "Firebomb";
        if (Input.GetKeyUp(KeyCode.Alpha2))
            Active = "Freeze Blast";
        if (Input.GetKeyUp(KeyCode.Alpha3))
            Active = "Bolt Dash";
        if (Input.GetKeyUp(KeyCode.Alpha7))
            Passive = "Heatstroke";
        if (Input.GetKeyUp(KeyCode.Alpha8))
            Passive = "Cold Zone";
        if (Input.GetKeyUp(KeyCode.Alpha9))
            Passive = "Static Shock";
        //Ammo cheat
        if (Input.GetKeyUp(KeyCode.F2)){
            hp = hpMax;
            ammoDict["bullet"] = ammoMaxDict["bulletMax"];
            ammoDict["shell"] = ammoMaxDict["shellMax"];
            ammoDict["expolsive"] = ammoDict["expolsiveMax"];
        }
        //Weapon Swapping
        if (Input.GetKeyUp(KeyCode.Q) && ammo2 > 0 || Input.GetKeyUp(KeyCode.Joystick1Button3) && ammo2 > 0)
            wepSwap();
        //Psychic pulse
        if (Input.GetKeyUp(KeyCode.F) && pulseCooldown <= 0 || Input.GetKeyUp(KeyCode.Joystick1Button9) && pulseCooldown <= 0)
            StartCoroutine(pulseAction());
        //Passive heat
        if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.Joystick1Button8))
        {
            if(Passive == "Heatstroke"){
                if (passiveCooldown <= 0 && activeCooldown <=0 && pp > 40){
                    if (!heat){
                        pp -= 20;
                        heat = true;
                    }
                }
                else if (heat)
                    heat = false;
            }
            if(Passive == "Cold Zone"){
                if (passiveCooldown <= 0 && activeCooldown <= 0 && pp > 40){
                    if (!cold){
                        pp -= 20;
                        cold = true;
                    }
                }
                else if (cold)
                    cold = false;
            }
            if (Passive == "Static Shock")
            {
                if (passiveCooldown <= 0 && activeCooldown <= 0 && pp > 40){
                    if (!shock){
                        pp -= 20;
                        shock = true;
                    }
                }
                else if (shock)
                    shock = false;
            }
        }
    }

}
