using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

	public int openingDirection;
	// 1 --> need bottom door
	// 2 --> need top door
	// 3 --> need left door
	// 4 --> need right door


	private RoomTemplates templates;
	private int rand;
	public bool spawned = false;
	public bool onGrid = false;
	public bool split;
	public float waitTime = 4f;

	void Start(){
		Destroy(gameObject, waitTime);
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		Invoke("Spawn", 0.1f);
	}


	void Spawn(){
		if(spawned == false && onGrid == true){
			if (openingDirection == 1)
			{
				//Spawn a split room
				if(split)
					Instantiate(templates.bSplitRoom[rand], transform.position, templates.bSplitRoom[rand].transform.rotation);
                else
                {
					// Need to spawn a room with a BOTTOM door.
					rand = Random.Range(0, templates.bottomRooms.Length);
					Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
				}
			}
			else if (openingDirection == 2)
			{
				//Spawn a split room
				if (split)
					Instantiate(templates.tSplitRoom[rand], transform.position, templates.tSplitRoom[rand].transform.rotation);
                else
                {
					// Need to spawn a room with a TOP door.
					rand = Random.Range(0, templates.topRooms.Length);
					Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
				}
			}
			else if (openingDirection == 3)
			{
				//Spawn a split room
				if (split)
					Instantiate(templates.lSplitRoom[rand], transform.position, templates.lSplitRoom[rand].transform.rotation);
				else
				{
					// Need to spawn a room with a LEFT door.
					rand = Random.Range(0, templates.leftRooms.Length);
					Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
				}
			}
			else if (openingDirection == 4)
			{
				if(split)
					Instantiate(templates.rSplitRoom[rand], transform.position, templates.rSplitRoom[rand].transform.rotation);
				else
                {
					// Need to spawn a room with a RIGHT door.
					rand = Random.Range(0, templates.rightRooms.Length);
					Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
				}				
			}
			spawned = true;
		}
		else if (spawned == false && onGrid == false)
		{
			if (openingDirection == 1)
				// Need to spawn a room with a BOTTOM door.
				Instantiate(templates.BRoom, transform.position, templates.BRoom.transform.rotation);
			else if (openingDirection == 2)
				// Need to spawn a room with a TOP door.
				Instantiate(templates.TRoom, transform.position, templates.TRoom.transform.rotation);
			else if (openingDirection == 3)
				// Need to spawn a room with a LEFT door.
				Instantiate(templates.LRoom, transform.position, templates.LRoom.transform.rotation);
			else if (openingDirection == 4)
				// Need to spawn a room with a RIGHT door.
				Instantiate(templates.RRoom, transform.position, templates.RRoom.transform.rotation);
			spawned = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		if (other.CompareTag("SpawnPoint")){
			if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false){
				Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
				Destroy(gameObject);
			} 
			spawned = true;
		}
	}
    void OnTriggerStay2D(Collider2D other)
    {
       if (other.tag == ("Grid"))
        {
			onGrid = true;
        }
    }
}
