using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	public float waitTime, bossCountdown = 5f, pauseCooldown;
	private bool spawnedExit;
	public bool selection, bossFight, bossDeath, paused;
	public GameObject exit;
	public GameObject boss;
	public GameObject bossText;
	AudioSource music;
	VisibleRoom visible;
	public alphaBossScript alpha;
	PlayerStat player;
	private void Start()
    {
		Time.timeScale = 1f;
		player = GameObject.Find("Player").GetComponent<PlayerStat>();
		load.SetActive(true);
		//show the ability choice screen for normal levels
		if (!bossFight)
        {
			music = GameObject.Find("Global").GetComponent<AudioSource>();
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
							music.Play();
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
			if(waitTime <= 0)
            {
				//Pausing
				if (player.hp > 0)
                {
					Time.timeScale = (paused) ? 0f : 1f;
				}
			
				if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Joystick1Button7)){
					if (!paused){
						Cursor.visible = true;
						paused = true;
						SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
						pauseCooldown = 0.1f;
					}
					else{
						Cursor.visible = false;
						paused = false;
						SceneManager.UnloadSceneAsync("Pause");
						
					}
				}
			}
			if (pauseCooldown > 0 && !paused)
				pauseCooldown -= Time.deltaTime;
			
		}
		//set boss active in boss fight after wait time
		if (bossFight && waitTime <= 0)
			StartCoroutine(bossOn());
		if (bossDeath)
        {
			StartCoroutine(bossEnd());
			Text bText = GameObject.Find("bossText").GetComponent<Text>();
			bText.text = "Boss Defeated..." + bossCountdown.ToString("F0");
			bossCountdown -= Time.deltaTime;
			Debug.Log(bossCountdown);
		}
			
		
	}
	IEnumerator bossOn()
    {
		yield return new WaitForSeconds(1f);
		if(!bossDeath)
			boss.SetActive(true);
	}
	public IEnumerator bossEnd()
	{
		bossText.SetActive(true);
		yield return new WaitForSeconds(5f);
		SceneManager.LoadScene("3");
	}
}
