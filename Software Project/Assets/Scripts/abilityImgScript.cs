using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class abilityImgScript : MonoBehaviour
{
    public bool active;
    PlayerStat player;
    public Sprite[] icon;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            GetComponent<Image>().sprite = icon[player.actNum];
        }
        if (!active)
        {
            GetComponent<Image>().sprite = icon[player.pasNum];
        }
    }
}
