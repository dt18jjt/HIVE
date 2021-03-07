using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buyScript : MonoBehaviour
{
    public Text buyText, priceText;
    public int price, effect;
    public bool bought;
    PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        stat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void buy()
    {
        if (stat.bp >= price && !bought)
        {
            //PlayerPrefs.SetInt("BP", PlayerPrefs.GetInt("BP")-price);
            //Debug.Log(PlayerPrefs.GetInt("BP"));
            stat.bp -= price;
            createBuyList();
            bought = true;
            priceText.text = "0";
            priceText.color = Color.red;
        }
        else if (stat.bp < price)
            StartCoroutine(invaild());
    }
    IEnumerator invaild()
    {
        //int temp = price;
        priceText.text = "INVAILD";
        yield return new WaitForSeconds(0.5f);
        priceText.text = price.ToString();
    }
    delegate void buyitemMethod();
    void createBuyList()
    {
        List<buyitemMethod> buyItem = new List<buyitemMethod>();
        //Add all item functions
        buyItem.Add(sMedkit);
        buyItem.Add(lMedkit);
        buyItem.Add(bAmmo8);
        buyItem.Add(bAmmo16);
        buyItem.Add(sAmmo4);
        buyItem.Add(sAmmo8);
        buyItem.Add(eAmmo2);
        buyItem.Add(eAmmo4);
        buyItem.Add(HPMax);
        buyItem.Add(PPMax);
        //call a method
        buyItem[effect]();
    }
    void sMedkit()
    {
        stat.hp += 20;
    }
    void lMedkit()
    {
        stat.hp += 40;
    }
    void bAmmo8()
    {
        stat.ammoDict["bullet"] += 8;
    }
    void bAmmo16()
    {
        stat.ammoDict["bullet"] += 8;
    }
    void sAmmo4()
    {
        stat.ammoDict["shell"] += 4;
    }
    void sAmmo8()
    {
        stat.ammoDict["shell"] += 8;
    }
    void eAmmo2()
    {
        stat.ammoDict["expolsive"] += 2;

    }
    void eAmmo4()
    {
        stat.ammoDict["expolsive"] += 4;
    }
    void HPMax()
    {
        stat.hpMax += 10;
        //stat.hp += 10;
    }
    void PPMax()
    {
        stat.ppMax += 10;
        //stat.pp += 10;
    }
}
