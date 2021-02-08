using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour {

	public GameObject[] bottomRooms;
	public GameObject[] topRooms;
	public GameObject[] leftRooms;
	public GameObject[] rightRooms;
	public GameObject[] startRooms;
	public GameObject[] bSplitRoom;
	public GameObject[] tSplitRoom;
	public GameObject[] lSplitRoom;
	public GameObject[] rSplitRoom;
	public GameObject TRoom;
	public GameObject BRoom;
	public GameObject RRoom;
	public GameObject LRoom;
	public GameObject closedRoom;
	public GameObject load;
	public List<GameObject> rooms;

	public float waitTime;
	private bool spawnedBoss;
	public GameObject boss;
	VisibleRoom visible;
    private void Start()
    {
		load.SetActive(true);
	}

    void Update(){

		if(waitTime <= 0 && !spawnedBoss){
			for (int i = 0; i < rooms.Count; i++) {
				if(i == rooms.Count-1){
					Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
					spawnedBoss = true;
					load.SetActive(false);
				}
			}
		} else {
			waitTime -= Time.deltaTime;
		}

	}
}
