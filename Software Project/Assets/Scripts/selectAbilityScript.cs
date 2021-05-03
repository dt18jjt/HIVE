using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectAbilityScript : MonoBehaviour
{
    public bool active;
    public Text abilityText, explainText, headText;
    public int abilityNum;
    public Image icon;
    public Sprite[] abilityIcons;
    PlayerStat player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        headText.text = (active) ? "CHOOSE A ACTIVE ABlLITY" : "CHOOSE A PASSIVE ABlLITY";
        icon.sprite = abilityIcons[abilityNum];
        if (active)
        {
            //Go onto passive selection
            if (Input.GetKey(KeyCode.Joystick1Button7))
                active = false;
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
            //Go onto passive selection
            if (Input.GetKey(KeyCode.Joystick1Button7))
                active = false;
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
                    explainText.text = "The player raises a shield of rocks to protect themselves.";
                    break;
                case 3:
                    abilityText.text = "EARTH BARRIER";
                    explainText.text = "When hit by an enemy they also take damage";
                    break;
                case 4:
                    abilityText.text = "ILLUSION DECOY";
                    explainText.text = "Spawn a decoy that enemies will attack for a period.";
                    break;
            }
        }

    }
    public void chooseFire()
    {
        if (active)
        {
            abilityNum = 0;
        }
    }
    public void chooseHeat()
    {
        if (!active)
        {
            abilityNum = 0;
        }
    }
    public void chooseFreeze()
    {
        if (active)
        {
            abilityNum = 1;
        }
    }
    public void chooseCold()
    {
        if (!active)
        {
            abilityNum = 1;
        }
    }
    public void chooseBolt()
    {
        if (active)
        {
            abilityNum = 2;
        }
    }
    public void chooseShock()
    {
        if (!active)
        {
            abilityNum = 2;
        }
    }
    public void chooseTremor()
    {
        if (active)
        {
            abilityNum = 3;
        }
    } 
    public void chooseEarth()
    {
        if (!active)
        {
            abilityNum = 3;
        }
    }
    public void chooseConfuse()
    {
        if (active)
        {
            abilityNum = 4;
        }
    }
    public void chooseDecoy()
    {
        if (!active)
        {
            abilityNum = 4;
        }
    }
}
