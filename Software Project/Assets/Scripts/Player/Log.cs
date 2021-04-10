using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    //Player behaviours
    public int pyroHit = 0, cryoHit = 0, geoHit = 0, electroHit = 0, hypnoHit = 0, bulletHit = 0, shellHit = 0, explosiveHit = 0, laserHit = 0, meleeHit = 0, 
        quickKill = 0, shopUse = 0, stackUse = 0, healthUse = 0;
    public bool noDamage = true;
    //enemy add slots
    public int slotsLeft = 2;
    //adding enemy spawn values
    public bool add003, add004, add005, add006, add007, add008, add009, add010, add011, add012;
    //remove enemies stopping them from being spawned again
    public bool del003, del004, del005, del006, del007, del008, del009, del010, del011, del012;
    private void Update()
    {
        if(slotsLeft > 0)
        {
            if (pyroHit >= 20 && !add003){
                add003 = true;
                slotsLeft--;
                Debug.Log("003 added!");
            }
            if (cryoHit >= 20 && !add004){
                add004 = true;
                slotsLeft--;
                Debug.Log("004 added!");
            }
            if (geoHit >= 20 && !add005){
                add005 = true;
                slotsLeft--;
                Debug.Log("005 added!");
            }
            if (electroHit >= 20 && !add006){
                add006 = true;
                slotsLeft--;
                Debug.Log("006 added!");
            }
            if (hypnoHit >= 20 && !add007){
                add007 = true;
                slotsLeft--;
                Debug.Log("007 added!");
            }
            if (meleeHit >= 20 && !add008){
                add008 = true;
                slotsLeft--;
                Debug.Log("008 added!");
            }
            if (bulletHit >= 20 && !add009){
                add009 = true;
                slotsLeft--;
                Debug.Log("009 added!");
            }
            if (shellHit >= 20 && !add010){
                add010 = true;
                slotsLeft--;
                Debug.Log("010 added!");
            }
            if (explosiveHit >= 20 && !add011){
                add011 = true;
                slotsLeft--;
                Debug.Log("011 added!");
            }
            if (laserHit >= 20 && !add012){
                add012 = true;
                slotsLeft--;
                Debug.Log("012 added!");
            }
        }
        //setting enemies added
        PlayerPrefs.SetInt("003", (add003 ? 1 : 0));
        PlayerPrefs.SetInt("004", (add004 ? 1 : 0));
        PlayerPrefs.SetInt("005", (add005 ? 1 : 0));
        PlayerPrefs.SetInt("006", (add006 ? 1 : 0));
        PlayerPrefs.SetInt("007", (add007 ? 1 : 0));
        PlayerPrefs.SetInt("008", (add008 ? 1 : 0));
        PlayerPrefs.SetInt("009", (add009 ? 1 : 0));
        PlayerPrefs.SetInt("010", (add010 ? 1 : 0));
        PlayerPrefs.SetInt("011", (add011 ? 1 : 0));
        PlayerPrefs.SetInt("012", (add012 ? 1 : 0));
        //stop enemies being set again
        PlayerPrefs.SetInt("del003", (del003 ? 1 : 0));
        PlayerPrefs.SetInt("del004", (del004 ? 1 : 0));
        PlayerPrefs.SetInt("del005", (del005 ? 1 : 0));
        PlayerPrefs.SetInt("del006", (del006 ? 1 : 0));
        PlayerPrefs.SetInt("del007", (del007 ? 1 : 0));
        PlayerPrefs.SetInt("del008", (del008 ? 1 : 0));
        PlayerPrefs.SetInt("del009", (del009 ? 1 : 0));
        PlayerPrefs.SetInt("del010", (del010 ? 1 : 0));
        PlayerPrefs.SetInt("del011", (del011 ? 1 : 0));
        PlayerPrefs.SetInt("del012", (del012 ? 1 : 0));

    }
}
