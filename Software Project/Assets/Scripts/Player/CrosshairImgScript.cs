using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairImgScript : MonoBehaviour
{
    public Sprite[] crosshairs;
    SpriteRenderer Image;
    public bool primary;
    PlayerStat player;
    // Start is called before the first frame update
    void Start()
    {
        Image = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<PlayerStat>();

    }

    // Update is called once per frame
    void Update()
    {
        Image.sprite = crosshairs[player.weapon1 - 1];
    }
}
