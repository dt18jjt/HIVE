using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	private bool spawnedExit;
	public bool selection, bossFight;
	public GameObject exit;
	public GameObject boss;
	public GameObject bossText;
	public
	VisibleRoom visible;
    private void Start()
    {
		load.SetActive(true);
		//show the ability choice screen for normal levels
		if (!bossFight)
        {
			selection = true;
			SceneManager.LoadScene("Ability", LoadSceneMode.Additive);
		}
        //set boss inactive at start
        if (bossFight)
        {
			boss.gameObject.SetActive(false);
			bossText.gameObject.SetActive(true);
		}
		
	}

    void Update(){
		if (!selection)
		{
			if (waitTime <= 0 && !spawnedExit)
			{
				for (int i = 0; i < rooms.Count; i++)
				{
					if (i == rooms.Count - 1)
					{
						load.SetActive(false);
						if (!bossFight)
                        {
							Instantiate(exit, rooms[i].transform.position, Quaternion.identity);
							spawnedExit = true;
						}
						if(bossFight)
							bossText.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				waitTime -= Time.deltaTime;
			}
		}
		//set boss active in boss fight after wait time
		if (bossFight && waitTime <= 0)
			StartCoroutine(bossOn());
		
	}
	IEnumerator bossOn()
    {
		yield return new WaitForSeconds(1f);
		boss.SetActive(true);
	}
}
