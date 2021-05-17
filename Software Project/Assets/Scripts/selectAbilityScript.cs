using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class selectAbilityScript : MonoBehaviour
{
    public bool active, a1Off = false, a2Off = false, a3Off = false, a4Off = false, a5Off = false, p1Off = false, p2Off = false, 
        p3Off = false, p4Off = false, p5Off = false;
    public Text abilityText, explainText, headText, contText;
    public int abilityNum;
    public Image icon;
    public Sprite[] abilityIcons;
    public Image B1, B2, B3, B4, B5;
    public float cooldown = 1f;
    PlayerStat stat;
    PlayerMovement player;
    RoomTemplates templates;
    // Start is called before the first frame update
    void Start()
    {
       stat = GameObject.Find("Player").GetComponent<PlayerStat>();
       player = GameObject.Find("Player").GetComponent<PlayerMovement>();
       templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
       Cursor.visible = true;
        contText.text = "";
        if (stat.gameLevel <= 0)
        {
            PlayerPrefs.SetInt("a1", 0);
            PlayerPrefs.SetInt("a2", 0);
            PlayerPrefs.SetInt("a3", 0);
            PlayerPrefs.SetInt("a4", 0);
            PlayerPrefs.SetInt("a5", 0);
            PlayerPrefs.SetInt("p1", 0);
            PlayerPrefs.SetInt("p2", 0);
            PlayerPrefs.SetInt("p3", 0);
            PlayerPrefs.SetInt("p4", 0);
            PlayerPrefs.SetInt("p5", 0);
            //effect on rooms
            PlayerPrefs.SetInt("hOff", 1);
            PlayerPrefs.SetInt("cOff", 1);
            PlayerPrefs.SetInt("sOff", 1);
            PlayerPrefs.SetInt("eOff", 1);
            PlayerPrefs.SetInt("dOff", 1);
        }
       if (stat.gameLevel > 0)
        {
            //values for next level
            a1Off = (PlayerPrefs.GetInt("a1") != 0);
            a2Off = (PlayerPrefs.GetInt("a2") != 0);
            a3Off = (PlayerPrefs.GetInt("a3") != 0);
            a4Off = (PlayerPrefs.GetInt("a4") != 0);
            a5Off = (PlayerPrefs.GetInt("a5") != 0);
            p1Off = (PlayerPrefs.GetInt("p1") != 0);
            p2Off = (PlayerPrefs.GetInt("p2") != 0);
            p3Off = (PlayerPrefs.GetInt("p3") != 0);
            p4Off = (PlayerPrefs.GetInt("p4") != 0);
            p5Off = (PlayerPrefs.GetInt("p5") != 0);
            //effect on rooms
            PlayerPrefs.SetInt("hOff", (p1Off ? 1 : 0));
            PlayerPrefs.SetInt("cOff", (p2Off ? 1 : 0));
            PlayerPrefs.SetInt("sOff", (p3Off ? 1 : 0));
            PlayerPrefs.SetInt("eOff", (p4Off ? 1 : 0));
            PlayerPrefs.SetInt("dOff", (p5Off ? 1 : 0));
            switch (PlayerPrefs.GetInt("lastActive"))
            {
                case 0:
                    a1Off = true;
                    break;
                case 1:
                    a2Off = true;
                    break;
                case 2:
                    a3Off = true;
                    break;
                case 3:
                    a4Off = true;
                    break;
                case 4:
                    a5Off = true;
                    break;
            }
            switch (PlayerPrefs.GetInt("lastPassive"))
            {
                case 0:
                    p1Off = true;
                    break;
                case 1:
                    p2Off = true;
                    break;
                case 2:
                    p3Off = true;
                    break;
                case 3:
                    p4Off = true;
                    break;
                case 4:
                    p5Off = true;
                    break;
            }
        }
        //First active
        if (a1Off)
            abilityNum = 1;
        else if (a1Off && a2Off)
            abilityNum = 2;
        else if (a1Off && a2Off && a3Off)
            abilityNum = 3;
        else if (a1Off && a2Off && a3Off && a4Off)
            abilityNum = 4;
        else
            abilityNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //save ability number for next level
        if (stat.gameLevel > 0)
        {
            PlayerPrefs.SetInt("a1", (a1Off ? 1 : 0));
            PlayerPrefs.SetInt("a2", (a2Off ? 1 : 0));
            PlayerPrefs.SetInt("a3", (a3Off ? 1 : 0));
            PlayerPrefs.SetInt("a4", (a4Off ? 1 : 0));
            PlayerPrefs.SetInt("a5", (a5Off ? 1 : 0));
            PlayerPrefs.SetInt("p1", (p1Off ? 1 : 0));
            PlayerPrefs.SetInt("p2", (p2Off ? 1 : 0));
            PlayerPrefs.SetInt("p3", (p3Off ? 1 : 0));
            PlayerPrefs.SetInt("p4", (p4Off ? 1 : 0));
            PlayerPrefs.SetInt("p5", (p5Off ? 1 : 0));
        }
        headText.text = (active) ? "CHOOSE A ACTIVE ABlLITY" : "CHOOSE A PASSIVE ABlLITY";
        icon.sprite = abilityIcons[abilityNum];
        //Cooldown between choices
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
        //countine text
        contText.text = (cooldown <= 0) ? "PRESS START/SPACE TO Continue": ""; 
        //select ability
        if (active)
        {
            
            //Button greyed out
            B1.color = (a1Off) ? Color.black : Color.white;
            B2.color = (a2Off) ? Color.black : Color.white;
            B3.color = (a3Off) ? Color.black : Color.white;
            B4.color = (a4Off) ? Color.black : Color.white;
            B5.color = (a5Off) ? Color.black : Color.white;
            //Go onto passive selection
            if(cooldown <= 0)
            {
                if (Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Space))
                {
                    cooldown = 1f;
                    active = false;
                    abilityText.enabled = false;
                    explainText.enabled = false;
                    StartCoroutine(passiveSwitch());
                }
            }
            stat.actNum = abilityNum;
            //Change text
            switch (abilityNum)
            {
                case 0:
                    abilityText.text = "FIREBOMB";
                    explainText.text = "Covers the player in a wall of fire. Enemies caught in the flame area take damage.";
                    break;
                case 1:
                    abilityText.text = "FREEZE BLAST";
                    explainText.text = "When hit the enemy takes damage and freezes them in place for a few seconds.";
                    break;
                case 2:
                    abilityText.text = "BOLT DASH";
                    explainText.text = "An electric dash move, when touched enemies take damage ";
                    break;
                case 3:
                    abilityText.text = "TREMOR";
                    explainText.text = "Creates a tremor dealing damage pushing away enemies from the player.";
                    break;
                case 4:
                    abilityText.text = "CONFUSION";
                    explainText.text = "Make an enemy attack another enemy, if none are left they inflict damage onto themselves.";
                    break;
            }
        }
        if (!active)
        {
            
            //Button greyed out
            B1.color = (p1Off) ? Color.black : Color.white;
            B2.color = (p2Off) ? Color.black : Color.white;
            B3.color = (p3Off) ? Color.black : Color.white;
            B4.color = (p4Off) ? Color.black : Color.white;
            B5.color = (p5Off) ? Color.black : Color.white;
            //Go onto level
            if (cooldown <= 0)
            {
                if (Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Space))
                {
                    SceneManager.UnloadSceneAsync("Ability");
                    templates.selection = false;
                    Cursor.visible = false;
                }
            }
            stat.pasNum = abilityNum;
            switch (abilityNum)
            {
                case 0:
                    abilityText.text = "HEAT STROKE";
                    explainText.text = "Raises the temperature of the room resulting in all enemies in the room receiving burn damage";
                    break;
                case 1:
                    abilityText.text = "COLD ZONE";
                    explainText.text = "Decreasing the temperature of the room making the enemies move slower";
                    break;
                case 2:
                    abilityText.text = "STATIC SHOCK";
                    explainText.text = "When hit all enemies take damage ";
                    break;
                case 3:
                    abilityText.text = "EARTH BARRIER";
                    explainText.text = "raises a shield of rocks stopping enemy attacks";
                    break;
                case 4:
                    abilityText.text = "ILLUSION DECOY";
                    explainText.text = "Spawn a decoy that enemies will attack for a period";
                    break;
            }
        }

    }
    IEnumerator passiveSwitch()
    {
        yield return new WaitForSeconds(0.1f);
        //First passive
        if (!active)
        {
            if (p1Off)
                abilityNum = 1;
            else if (p1Off && p2Off)
                abilityNum = 2;
            else if (p1Off && p2Off && p3Off)
                abilityNum = 3;
            else if (p1Off && p2Off && p3Off && p4Off)
                abilityNum = 4;
            else
                abilityNum = 0;
        }
        yield return new WaitForSeconds(0.3f);
        abilityText.enabled = true;
        explainText.enabled = true;
    }
    public void chooseFire()
    {
        if (active && !a1Off)
        {
            abilityNum = 0;
        }
    }
    public void chooseHeat()
    {
        if (!active && !p1Off)
        {
            abilityNum = 0;
        }
    }
    public void chooseFreeze()
    {
        if (active && !a2Off)
        {
            abilityNum = 1;
        }
    }
    public void chooseCold()
    {
        if (!active && !p2Off)
        {
            abilityNum = 1;
        }
    }
    public void chooseBolt()
    {
        if (active && !a3Off)
        {
            abilityNum = 2;
        }
    }
    public void chooseShock()
    {
        if (!active && !p3Off)
        {
            abilityNum = 2;
        }
    }
    public void chooseTremor()
    {
        if (active && !a4Off)
        {
            abilityNum = 3;
        }
    } 
    public void chooseEarth()
    {
        if (!active && !p4Off)
        {
            abilityNum = 3;
        }
    }
    public void chooseConfuse()
    {
        if (active && !a5Off)
        {
            abilityNum = 4;
        }
    }
    public void chooseDecoy()
    {
        if (!active && !p5Off)
        {
            abilityNum = 4;
        }
    }
}

