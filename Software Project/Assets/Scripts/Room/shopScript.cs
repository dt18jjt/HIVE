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
    public Sprite[] shopImages;
    public Text buyText, priceText;
    public int price, effect, i;
    public AudioClip buySound, invaildSound;
    // Start is called before the first frame update
    void Start()
    {
        stat = GameObject.Find("Player").GetComponent<PlayerStat>();
        //Adding prices
        {
            priceDict.Add("Health Pack + 20", 25);
            priceDict.Add("Health Pack + 50", 40);
            priceDict.Add("Bullet Ammo + 12", 15);
            priceDict.Add("Bullet Ammo + 24", 30);
            priceDict.Add("Shell Ammo + 6", 15);
            priceDict.Add("Shell Ammo + 12", 30);
            priceDict.Add("Explosive Ammo + 3", 15);
            priceDict.Add("Explosive Ammo + 5", 30);
            priceDict.Add("Health Booster", 50);
            priceDict.Add("Psychic Booster", 50);
            priceDict.Add("Ammo Stack + 1", 40);
            priceDict.Add("Revolver LV.1", 40);
            priceDict.Add("Pump Shotgun LV.1", 40);
            priceDict.Add("Missile Launcher LV.1", 40);
            priceDict.Add("Laser Repeater LV.1", 40);
            priceDict.Add("Hand Axe LV.1", 40);
            priceDict.Add("Magnum LV.2", 60);
            priceDict.Add("Riot Shotgun LV.2", 60);
            priceDict.Add("Mine Launcher LV.2", 60);
            priceDict.Add("Beam Rifle LV.2", 60);
            priceDict.Add("Sledgehammer LV.2", 60);
            priceDict.Add("Machine gun LV.3", 80);
            priceDict.Add("Super barrel LV.3", 80);
            priceDict.Add("Heat Seeker LV.3", 80);
            priceDict.Add("Rail Gun LV.3", 80);
            priceDict.Add("Katana LV.3", 80);
        }
        //Adding effects
        {
            effectDict.Add("Health Pack + 20", 0);
            effectDict.Add("Health Pack + 50", 1);
            effectDict.Add("Bullet Ammo + 12", 2);
            effectDict.Add("Bullet Ammo + 24", 3);
            effectDict.Add("Shell Ammo + 6", 4);
            effectDict.Add("Shell Ammo + 12", 5);
            effectDict.Add("Explosive Ammo + 3", 6);
            effectDict.Add("Explosive Ammo + 5", 7);
            effectDict.Add("Health Booster", 8);
            effectDict.Add("Psychic Booster", 9);
            effectDict.Add("Ammo Stack + 1", 10);
            effectDict.Add("Revolver LV.1", 11);
            effectDict.Add("Pump Shotgun LV.1", 12);
            effectDict.Add("Missile Launcher LV.1", 13);
            effectDict.Add("Laser Repeater LV.1", 14);
            effectDict.Add("Hand Axe LV.1", 15);
            effectDict.Add("Magnum LV.2", 16);
            effectDict.Add("Riot Shotgun LV.2", 17);
            effectDict.Add("Mine Launcher LV.2", 18);
            effectDict.Add("Beam Rifle LV.2", 19);
            effectDict.Add("Sledgehammer LV.2", 20);
            effectDict.Add("Machine gun LV.3", 21);
            effectDict.Add("Super barrel LV.3", 22);
            effectDict.Add("Heat Seeker LV.3", 23);
            effectDict.Add("Rail Gun LV.3", 24);
            effectDict.Add("Katana LV.3", 25);
        }
        if (!stat.storeFound)
        {
            //setting each buy item for the first time
            for (i = 0; i < buttons.Count; i++)
            {
                var random = priceDict.Keys.ElementAt((int)Random.Range(0, priceDict.Count));
                buttons[i].gameObject.GetComponent<buyScript>().price = priceDict[random];
                buttons[i].gameObject.GetComponent<buyScript>().buyText.text = random;
                buttons[i].gameObject.GetComponent<buyScript>().priceText.text =
                buttons[i].gameObject.GetComponent<buyScript>().price.ToString();
                buttons[i].gameObject.GetComponent<buyScript>().effect = effectDict[random];
                PlayerPrefs.SetString("Value" + i.ToString(), random);
                priceDict.Remove(random);
                
            }

        }
        if (stat.storeFound)
        {
            //setting each buy item for repeated times
            for (i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.GetComponent<buyScript>().price = priceDict[PlayerPrefs.GetString("Value" + i.ToString())];
                buttons[i].gameObject.GetComponent<buyScript>().buyText.text = PlayerPrefs.GetString("Value" + i.ToString());
                buttons[i].gameObject.GetComponent<buyScript>().priceText.text =
                buttons[i].gameObject.GetComponent<buyScript>().price.ToString();
                buttons[i].gameObject.GetComponent<buyScript>().effect = effectDict[PlayerPrefs.GetString("Value" + i.ToString())];
                priceDict.Remove(PlayerPrefs.GetString("Value" + i.ToString()));

            }
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
        stat.storeCoolDown = 0.2f;
        Cursor.visible = false;
        stat.storeFound = true;
    }
}
