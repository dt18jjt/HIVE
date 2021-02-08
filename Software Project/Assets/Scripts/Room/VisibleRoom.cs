using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleRoom : MonoBehaviour
{
    SpriteRenderer[] sprites;
    float speed = 1000.0f;
    GameObject playerCam;
    private Vector3 newPos;
    [SerializeField]
    public GameObject icon;
    bool playerOn = false;
    // Start is called before the first frame update
    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        playerCam = GameObject.Find("Main Camera");
        newPos = new Vector3(transform.position.x, transform.position.y, -10);
        Instantiate(icon, transform.position, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOn)
        {
            sprites = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites)
                sprite.enabled = true;
        }
        if (!playerOn)
        {
            sprites = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites)
                sprite.enabled = false;
        }      
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        //Make walls visible
        if(other.name == "Player")
        {
            playerOn = true;
            float step = speed * Time.deltaTime;
            playerCam.transform.position = Vector3.MoveTowards(playerCam.transform.position, newPos, step);

        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //Make walls invisible
        if (other.name == "Player")
        {
            playerOn = false;

        }
    }
}
