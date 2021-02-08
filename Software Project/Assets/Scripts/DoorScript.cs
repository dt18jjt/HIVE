using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject newPos;
    public bool locked = false;
    bool blocked;
    SpriteRenderer door;
    RoomTypes room;
    Countdown timeCountdown;
    // Start is called before the first frame update
    void Start()
    {
        door = GetComponentInChildren<SpriteRenderer>();
        room = GetComponentInParent<RoomTypes>();
        timeCountdown = GameObject.Find("Global").GetComponent<Countdown>();
        

    }

    // Update is called once per frame
    void Update(){
        if (room.time && timeCountdown.timeRemaining > 0)
            locked = true;
        else if (room.enemyOn)
            locked = true;
        else if (blocked)
            Destroy(gameObject);
        else
            locked = false;
        if (!locked)
            door.color = Color.green;
        else
            door.color = Color.red;

    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.name == "Player" && !locked)
            other.transform.position = newPos.transform.position;
        if (other.tag == "Blocked")
            blocked = true;
    }
    private void OnTriggerStay2D(Collider2D other){
        
    }
}
