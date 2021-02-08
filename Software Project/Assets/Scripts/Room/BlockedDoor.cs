using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedDoor : MonoBehaviour
{
    public bool blocked = false;
    // Start is called before the first frame update
    private void Update(){
        if (blocked)
            transform.GetChild(0).gameObject.SetActive(true);
        else if(!blocked)
            transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D other){
        if (other.tag == "Wall")
        {
            blocked = true;
        }
        if (other.tag == "Closed")
        {
            blocked = true;
        }
      

    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Wall"){
            blocked = false;
        }
        if (other.tag == "Closed"){
            blocked = false;
        }
    }
}
