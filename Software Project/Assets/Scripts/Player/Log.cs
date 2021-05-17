using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Log : MonoBehaviour
{
    //Player behaviours
    public int quickKill = 0, shopUse = 0, stackUse = 0, healthUse = 0, slots;
    public Dictionary<string, int> playerAction = new Dictionary<string, int>(); //Dictionary of player actions
    public List<string> highestActions; //highest actions are put into a list
    public bool noDamage = true;
    bool high1, high2;
    public string highString, highString2;
    //adding enemy spawn values
    public bool add003, add004, add005, add006, add007, add008, add009, add010, add011, add012;
    //remove enemies stopping them from being spawned again
    public bool del003, del004, del005, del006, del007, del008, del009, del010, del011, del012;
    private void Start()
    {
        playerAction.Add("pyroHit", 0);
        playerAction.Add("cryoHit", 0);
        playerAction.Add("geoHit", 0);
        playerAction.Add("electroHit", 0);
        playerAction.Add("hypnoHit", 0);
        playerAction.Add("bulletHit", 0);
        playerAction.Add("shellHit", 0);
        playerAction.Add("explosiveHit", 0);
        playerAction.Add("laserHit", 0);
        playerAction.Add("meleeHit", 0);
        //StartCoroutine(addEnemy());
    }
    private void Update()
    {
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
        //set the add enemies boolean if values is in highest actions list
        if(!add003)
            add003 = (highestActions.Contains("pyroHit")) ? true : false;
        if (!add004)
            add004 = (highestActions.Contains("cryoHit")) ? true : false;
        if (!add005)
            add005 = (highestActions.Contains("electroHit")) ? true : false;
        if (!add006)
            add006 = (highestActions.Contains("geoHit")) ? true : false;
        if (!add007)
            add007 = (highestActions.Contains("hypnoHit")) ? true : false;
        if (!add008)
            add008 = (highestActions.Contains("meleeHit")) ? true : false;
        if (!add009)
            add009 = (highestActions.Contains("bulletHit")) ? true : false;
        if (!add010)
            add010 = (highestActions.Contains("shellHit")) ? true : false;
        if (!add011)
            add011 = (highestActions.Contains("explosiveHit")) ? true : false;
        if (!add012)
            add012 = (highestActions.Contains("laserHit")) ? true : false;
        //remove keys for enemies that are already added
        if (del003)
            playerAction.Remove("pyroHit");
        if (del004)
            playerAction.Remove("cryoHit");
        if (del005)
            playerAction.Remove("geoHit");
        if (del006)
            playerAction.Remove("electroHit");
        if (del007)
            playerAction.Remove("hypnoHit");
        if (del008)
            playerAction.Remove("meleeHit");
        if (del009)
            playerAction.Remove("bulletHit");
        if (del010)
            playerAction.Remove("shellHit");
        if (del011)
            playerAction.Remove("explosiveHit");
        if (del012)
            playerAction.Remove("laserHit");
    }
    public IEnumerator addEnemy()
    {
        foreach (KeyValuePair<string, int> pAction in playerAction)
        {
            if (pAction.Value == playerAction.Values.Max() && !high1)
            {
                highString = pAction.Key;
                high1 = true;
            }
        }
        highestActions.Add(highString);
        playerAction.Remove(highString);
        yield return new WaitForSeconds(.2f);
        foreach (KeyValuePair<string, int> pAction in playerAction)
        {
            if (pAction.Value == playerAction.Values.Max() && !high2)
            {
                highString2 = pAction.Key;
                high2 = true;
            }
        }
        highestActions.Add(highString2);
        playerAction.Remove(highString2);
    }
}
