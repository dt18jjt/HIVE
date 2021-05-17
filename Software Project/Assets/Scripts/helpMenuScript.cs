using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class helpMenuScript : MonoBehaviour
{
    public GameObject Main, General, Controls, Weapons, Abilities, Rooms, Enemies, mainText;
    int helpNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Help menu change to differnet section
        switch (helpNum)
        {
            case 0:
                Main.SetActive(true);
                General.SetActive(false);
                Controls.SetActive(false);
                Weapons.SetActive(false);
                Abilities.SetActive(false);
                Rooms.SetActive(false);
                Enemies.SetActive(false);
                mainText.SetActive(false);
                break;
            case 1:
                Main.SetActive(false);
                General.SetActive(true);
                Controls.SetActive(false);
                Weapons.SetActive(false);
                Abilities.SetActive(false);
                Rooms.SetActive(false);
                Enemies.SetActive(false);
                mainText.SetActive(true);
                break;
            case 2:
                Main.SetActive(false);
                General.SetActive(false);
                Controls.SetActive(true);
                Weapons.SetActive(false);
                Abilities.SetActive(false);
                Rooms.SetActive(false);
                Enemies.SetActive(false);
                mainText.SetActive(true);
                break;
            case 3:
                Main.SetActive(false);
                General.SetActive(false);
                Controls.SetActive(false);
                Weapons.SetActive(true);
                Abilities.SetActive(false);
                Rooms.SetActive(false);
                Enemies.SetActive(false);
                mainText.SetActive(true);
                break;
            case 4:
                Main.SetActive(false);
                General.SetActive(false);
                Controls.SetActive(false);
                Weapons.SetActive(false);
                Abilities.SetActive(true);
                Rooms.SetActive(false);
                Enemies.SetActive(false);
                mainText.SetActive(true);
                break;
            case 5:
                Main.SetActive(false);
                General.SetActive(false);
                Controls.SetActive(false);
                Weapons.SetActive(false);
                Abilities.SetActive(false);
                Rooms.SetActive(true);
                Enemies.SetActive(false);
                mainText.SetActive(true);
                break;
            case 6:
                Main.SetActive(false);
                General.SetActive(false);
                Controls.SetActive(false);
                Weapons.SetActive(false);
                Abilities.SetActive(false);
                Rooms.SetActive(false);
                Enemies.SetActive(true);
                mainText.SetActive(true);
                break;
        }  
        //back to help menu
        if(helpNum > 0)
        {
            if (Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Space))
            {
                helpNum = 0;
            }
        }
    }
    public void backToMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void general()
    {
        helpNum = 1;
    }
    public void control()
    {
        helpNum = 2;
    }
    public void weapons()
    {
        helpNum = 3;
    }
    public void abilities()
    {
        helpNum = 4;
    }
    public void rooms()
    {
        helpNum = 5;
    }
    public void enemies()
    {
        helpNum = 6;
    }
}
