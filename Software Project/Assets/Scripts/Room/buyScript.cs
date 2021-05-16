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
    Log log;
    public Image item;
    shopScript shop;
    GameObject shopObj;
    // Start is called before the first frame update
    void Start()
    {
        stat = GameObject.Find("Player").GetComponent<PlayerStat>();
        log = GameObject.Find("Global").GetComponent<Log>();
        shop = GameObject.Find("shopSystem").GetComponent<shopScript>();
        shopObj = GameObject.Find("shopSystem");

    }

    // Update is called once per frame
    void Update()
    {
        item.sprite = shop.shopImages[effect];
        item.SetNativeSize();
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
            log.shopUse++;
            Debug.Log("shop: " + log.shopUse);
            shopObj.GetComponent<AudioSource>().PlayOneShot(shop.buySound);
        }
        else if (stat.bp < price)
            StartCoroutine(invaild());
    }
    IEnumerator invaild()
    {
        //int temp = price;
        shopObj.GetComponent<AudioSource>().PlayOneShot(shop.invaildSound);
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
        buyItem.Add(bAmmoS);
        buyItem.Add(bAmmoL);
        buyItem.Add(sAmmoS);
        buyItem.Add(sAmmoL);
        buyItem.Add(eAmmoS);
        buyItem.Add(eAmmoL);
        buyItem.Add(HPMax);
        buyItem.Add(PPMax);
        buyItem.Add(aStack);
        buyItem.Add(Lv1B);
        buyItem.Add(Lv1S);
        buyItem.Add(Lv1E);
        buyItem.Add(Lv1L);
        buyItem.Add(Lv1M);
        buyItem.Add(Lv2B);
        buyItem.Add(Lv2S);
        buyItem.Add(Lv2E);
        buyItem.Add(Lv2L);
        buyItem.Add(Lv2M);
        buyItem.Add(Lv3B);
        buyItem.Add(Lv3S);
        buyItem.Add(Lv3E);
        buyItem.Add(Lv3L);
        buyItem.Add(Lv3M);
        //call a method
        buyItem[effect]();
    }
    //Buy item functions
    void sMedkit()
    {
        stat.hp += 20;
    }
    void lMedkit()
    {
        stat.hp += 40;
    }
    void bAmmoS()
    {
        stat.ammoDict["bullet"] += 8;
    }
    void bAmmoL()
    {
        stat.ammoDict["bullet"] += 8;
    }
    void sAmmoS()
    {
        stat.ammoDict["shell"] += 4;
    }
    void sAmmoL()
    {
        stat.ammoDict["shell"] += 8;
    }
    void eAmmoS()
    {
        stat.ammoDict["explosive"] += 2;

    }
    void eAmmoL()
    {
        stat.ammoDict["explosive"] += 4;
    }
    void HPMax()
    {
        stat.hpMax += 10;
        stat.hp += 10;
    }
    void PPMax()
    {
        stat.ppMax += 10;
    }
    void aStack()
    {
        stat.ammoStack1++;
        stat.ammoStack2++;
    }
    void Lv1B()
    {
        //stat.dropWeapon();
        stat.weapon1 = 1;
        stat.wep1Level = 1;
    }
    void Lv1S()
    {
        //stat.dropWeapon();
        stat.weapon1 = 2;
        stat.wep1Level = 1;
    }
    void Lv1E()
    {
        //stat.dropWeapon();
        stat.weapon1 = 3;
        stat.wep1Level = 1;
    }
    void Lv1L()
    {
        //stat.dropWeapon();
        stat.weapon1 = 4;
        stat.wep1Level = 1;
    }
    void Lv1M()
    {
        //stat.dropWeapon();
        stat.weapon1 = 5;
        stat.wep1Level = 1;
    }
    void Lv2B()
    {
        stat.weapon1 = 1;
        stat.wep1Level = 2;
    }
    void Lv2S()
    {
        //stat.dropWeapon();
        stat.weapon1 = 2;
        stat.wep1Level = 2;
    }
    void Lv2E()
    {
        //stat.dropWeapon();
        stat.weapon1 = 3;
        stat.wep1Level = 2;
    }
    void Lv2L()
    {
        //stat.dropWeapon();
        stat.weapon1 = 4;
        stat.wep1Level = 2;
    }
    void Lv2M()
    {
        //stat.dropWeapon();
        stat.weapon1 = 5;
        stat.wep1Level = 2;
    }
    void Lv3B()
    {
        //stat.dropWeapon();
        stat.weapon1 = 1;
        stat.wep1Level = 3;
    }
    void Lv3S()
    {
        //stat.dropWeapon();
        stat.weapon1 = 2;
        stat.wep1Level = 3;
    }
    void Lv3E()
    {
        //stat.dropWeapon();
        stat.weapon1 = 3;
        stat.wep1Level = 3;
    }
    void Lv3L()
    {
        //stat.dropWeapon();
        stat.weapon1 = 4;
        stat.wep1Level = 3;
    }
    void Lv3M()
    {
        //stat.dropWeapon();
        stat.weapon1 = 5;
        stat.wep1Level = 3;
    }
}
