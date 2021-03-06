using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChange : MonoBehaviour
{
    bool Nearby = false, Traced = false, playerOn = false, bRoom, sRoom;
    public Color offColor;
    [SerializeField]
    public GameObject trackBox;
    // Start is called before the first frame update
    void Start()
    {
        trackBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Show nearby rooms
        if (Nearby)
            GetComponent<SpriteRenderer>().enabled = true;
        else
            GetComponent<SpriteRenderer>().enabled = false;
        if (!Traced && !bRoom)
            offColor = Color.blue;
        else if(bRoom)
            offColor = Color.red;
        else if (sRoom)
            offColor = Color.yellow;
        else
            offColor = Color.gray;
        //When the player is in a room
        if (playerOn)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            trackBox.SetActive(true);
        }
        if(!playerOn)
        {
            GetComponent<SpriteRenderer>().color = offColor;
            trackBox.SetActive(false);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Icon")
        {
            Nearby = true;
        }
        if (other.tag == "Boss" && !Traced)
        {
            bRoom = true;
            Destroy(other.gameObject);

        }
        if (other.tag == "Shop")
        {
            sRoom = true;
            //Destroy(other.gameObject);

        }
        if (other.tag == "Room")
        {
            
        }
        if (other.name == "Player")
        {
            Traced = true;

        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            playerOn = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            playerOn = false;
        }

    }
}
