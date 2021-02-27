using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buyScript : MonoBehaviour
{
    public Text buyText;
    public Text priceText;
    public int price;
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
}
