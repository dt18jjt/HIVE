using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class endLevelScript : MonoBehaviour
{
    public Text compText, newEmText1, newEmText2;
    public Image newImg, newImg2;
    public Sprite[] newSprites;
    PlayerStat player;
    Log log;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        log = GameObject.Find("Global").GetComponent<Log>();
        switch (player.gameLevel)
        {
            case 0:
                compText.text = "ZONE 0 COMPLETE";
                break;
            case 1:
                compText.text = "ZONE 1 COMPLETE";
                break;
            case 2:
                compText.text = "ZONE 2 COMPLETE";
                break;
        }
        switch (log.highString)
        {
            case "pyroHit":
                newEmText1.text = "SUBJECT 003 ('SCORCH') ADDED";
                newImg.sprite = newSprites[0];
                break;
            case "cryoHit":
                newEmText1.text = "SUBJECT 004 ('CHILL') ADDED";
                newImg.sprite = newSprites[1];
                break;
            case "geoHit":
                newEmText1.text = "SUBJECT 005 ('VINE') ADDED";
                newImg.sprite = newSprites[2];
                break;
            case "electroHit":
                newEmText1.text = "SUBJECT 006 ('SPARK') ADDED";
                newImg.sprite = newSprites[3];
                break;
            case "hypnoHit":
                newEmText1.text = "SUBJECT 007 ('WOKEN') ADDED";
                newImg.sprite = newSprites[4];
                break;
            case "meleeHit":
                newEmText1.text = "SUBJECT 008 ('PHANTOM') ADDED";
                newImg.sprite = newSprites[5];
                break;
            case "bulletHit":
                newEmText1.text = "SUBJECT 009 ('VOID') ADDED";
                newImg.sprite = newSprites[6];
                break;
            case "shellHit":
                newEmText1.text = "SUBJECT 010 ('FUSE') ADDED";
                newImg.sprite = newSprites[7];
                break;
            case "explosiveHit":
                newEmText1.text = "SUBJECT 011 ('SPEEDY') ADDED";
                newImg.sprite = newSprites[8];
                break;
            case "laserHit":
                newEmText1.text = "SUBJECT 012 ('RAGE') ADDED";
                newImg.sprite = newSprites[9];
                break;
        }
        switch (log.highString2)
        {
            case "pyroHit":
                newEmText2.text = "SUBJECT 003 ('SCORCH') ADDED";
                newImg2.sprite = newSprites[0];
                break;
            case "cryoHit":
                newEmText2.text = "SUBJECT 004 ('CHILL') ADDED";
                newImg2.sprite = newSprites[1];
                break;
            case "geoHit":
                newEmText2.text = "SUBJECT 005 ('VINE') ADDED";
                newImg2.sprite = newSprites[2];
                break;
            case "electroHit":
                newEmText2.text = "SUBJECT 006 ('SPARK') ADDED";
                newImg2.sprite = newSprites[3];
                break;
            case "hypnoHit":
                newEmText2.text = "SUBJECT 007 ('WOKEN') ADDED";
                newImg2.sprite = newSprites[4];
                break;
            case "meleeHit":
                newEmText2.text = "SUBJECT 008 ('PHANTOM') ADDED";
                newImg2.sprite = newSprites[5];
                break;
            case "bulletHit":
                newEmText2.text = "SUBJECT 009 ('VOID') ADDED";
                newImg2.sprite = newSprites[6];
                break;
            case "shellHit":
                newEmText2.text = "SUBJECT 010 ('FUSE') ADDED";
                newImg2.sprite = newSprites[7];
                break;
            case "explosiveHit":
                newEmText2.text = "SUBJECT 011 ('SPEEDY') ADDED";
                newImg2.sprite = newSprites[8];
                break;
            case "laserHit":
                newEmText2.text = "SUBJECT 012 ('RAGE') ADDED";
                newImg2.sprite = newSprites[9];
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //continue to next level
        if (Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Space))
        {
            switch (player.gameLevel)
            {
                //go to second level
                case 0:
                    SceneManager.LoadScene("2");
                    break;
                //go to random boss fight
                case 1:
                    int rnd = Random.Range(0, 2);
                    if(rnd == 0)
                        SceneManager.LoadScene("Boss(Alpha)");
                    if (rnd == 1)
                        SceneManager.LoadScene("Boss(Sigma)");
                    break;
                //go to main menu after final level
                case 2:
                    SceneManager.LoadScene("Main");
                    break;
            }
        }
    }
}
