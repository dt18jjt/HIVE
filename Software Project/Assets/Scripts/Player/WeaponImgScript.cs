using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponImgScript : MonoBehaviour
{
    public Sprite[] weapons;
    Image Image;
    public bool primary;
    PlayerStat player;
    public Color normalColor;
    // Start is called before the first frame update
    void Start()
    {
        Image = GetComponent<Image>();
        player = GameObject.Find("Player").GetComponent<PlayerStat>();

    }

    // Update is called once per frame
    void Update()
    {
        //sets image for primary weapon
        if (primary)
        {
            Image.sprite = weapons[player.weapon1 - 1];
            if (player.ammo1 <= 0)
                Image.color = Color.red;
        }
        //sets image for secondary weapon
        else
        {
            Image.sprite = weapons[player.weapon2 - 1];
            if (player.ammo2 <= 0 && !primary)
                Image.color = Color.red;
        }
        if (player.wepJam)
            Image.color = Color.red;
        else
            Image.color = normalColor;
        
    }
}
