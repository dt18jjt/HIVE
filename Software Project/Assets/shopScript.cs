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
    PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        stat = GameObject.Find("Player").GetComponent<PlayerStat>();
        //Adding prices
        {
            priceDict.Add("Medkit + 20HP", 25);
            priceDict.Add("Bullet Ammo + 8", 15);
            priceDict.Add("Bullet Ammo + 16", 30);
            priceDict.Add("Shell Ammo + 4", 15);
            priceDict.Add("Shell Ammo + 8", 30);
            priceDict.Add("Expolsive Ammo + 2", 15);
            priceDict.Add("Expolsive Ammo + 4", 30);
            priceDict.Add("HP Max +10", 50); 
            priceDict.Add("PP Max + 10", 50);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //bioText.text = PlayerPrefs.GetInt("BP").ToString();
        bioText.text = stat.bp.ToString();
        for (int i = 0; i < buttons.Count; i++)
        {
            var random = priceDict.Keys.ElementAt((int)Random.Range(0, priceDict.Count - 1));
            buttons[i].gameObject.GetComponent<buyScript>().price = priceDict[random];
            buttons[i].gameObject.GetComponent<buyScript>().buyText.text = random;
            buttons[i].gameObject.GetComponent<buyScript>().priceText.text = 
            buttons[i].gameObject.GetComponent<buyScript>().price.ToString();
            priceDict.Remove(random);

        }
    }
    public void exit()
    {
        SceneManager.UnloadSceneAsync("shop");
        stat.inStore = false;
        stat.storeCoolDown = 0.1f;
    }
}
