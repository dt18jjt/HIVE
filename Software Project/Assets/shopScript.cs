using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class shopScript : MonoBehaviour
{
    public Text bioText;
    public List<GameObject> buttons;
    Dictionary<string, int> priceDict = new Dictionary<string, int>(); // price Dictionary
    Dictionary<string, int> effectDict = new Dictionary<string, int>(); //effects dictionary
    PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        stat = GameObject.Find("Player").GetComponent<PlayerStat>();
        //Adding prices
        {
            priceDict.Add("Medkit + 20HP", 25);
            priceDict.Add("Medkit + 40HP", 40);
            priceDict.Add("Bullet Ammo + 8", 15);
            priceDict.Add("Bullet Ammo + 16", 30);
            priceDict.Add("Shell Ammo + 4", 15);
            priceDict.Add("Shell Ammo + 8", 30);
            priceDict.Add("Expolsive Ammo + 2", 15);
            priceDict.Add("Expolsive Ammo + 4", 30);
            priceDict.Add("HP Max +10", 50); 
            priceDict.Add("PP Max + 10", 50);
        }
        //Adding effects
        {
            effectDict.Add("Medkit + 20HP", 0);
            effectDict.Add("Medkit + 40HP", 1);
            effectDict.Add("Bullet Ammo + 8", 2);
            effectDict.Add("Bullet Ammo + 16", 3);
            effectDict.Add("Shell Ammo + 4", 4);
            effectDict.Add("Shell Ammo + 8", 5);
            effectDict.Add("Expolsive Ammo + 2", 6);
            effectDict.Add("Expolsive Ammo + 4", 7);
            effectDict.Add("HP Max +10", 8);
            effectDict.Add("PP Max + 10", 9);
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            var random = priceDict.Keys.ElementAt((int)Random.Range(0, priceDict.Count - 1));
            buttons[i].gameObject.GetComponent<buyScript>().price = priceDict[random];
            buttons[i].gameObject.GetComponent<buyScript>().buyText.text = random;
            buttons[i].gameObject.GetComponent<buyScript>().priceText.text =
            buttons[i].gameObject.GetComponent<buyScript>().price.ToString();
            buttons[i].gameObject.GetComponent<buyScript>().effect = effectDict[random];
            priceDict.Remove(random);

        }
    }

    // Update is called once per frame
    void Update()
    {
        //bioText.text = PlayerPrefs.GetInt("BP").ToString();
        bioText.text = stat.bp.ToString();
        
    }
    public void exit()
    {
        SceneManager.UnloadSceneAsync("shop");
        stat.inStore = false;
        stat.storeCoolDown = 0.1f;
    }
}
