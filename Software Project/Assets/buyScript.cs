using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buyScript : MonoBehaviour
{
    public Text buyText;
    public Text priceText;
    public int price;
    PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void buy()
    {
        if (stat.bp >= price)
            stat.bp -= price;
    }
    IEnumerator invaild()
    {
        //int temp = price;
        priceText.text = "INVAILD";
        yield return new WaitForSeconds(0.5f);
        priceText.text = price.ToString();
    }
}
