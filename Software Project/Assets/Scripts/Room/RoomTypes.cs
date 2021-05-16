using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTypes : MonoBehaviour
{
    int roomChance;
    public bool enemyOn = false, start = false, exit = false, shop = false, time = false, wJam = false, pBlocked = false,
        glitch = false, hazard = false, cache = false, entered = false, noEnemy = false, enemyBuff = false, boss;
    private RoomCount count;
    private Countdown timeCountdown;
    private RoomTemplates templates;
    public GameObject sIcon, gIcon, cIcon, eBox, IBox, box, eArea, exitPortal, floor, hArea;
    public GameObject[] items;
    public GameObject[] newEnemies;
    public int enemySpawnCount, enemyCount, itemCount;
    public Color normalColor, coldColor, hotColor, shockColor, shockDamColor, earthColor, decoyColor;
    public List<GameObject> enemies;
    public List<Transform> eSpawnPoints;
    public List<Transform> iSpawnPoints;
    PlayerStat player;
    Log log;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        if (!boss)
        {
            if (start)
                noEnemy = true;
            count = GameObject.Find("Global").GetComponent<RoomCount>();
            timeCountdown = GameObject.Find("Global").GetComponent<Countdown>();
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            log = GameObject.Find("Global").GetComponent<Log>();
            roomChance = Random.Range(1, 11);
            //always spawn a lab room in level
            shopSpawn();
            //spawn room when threat level is 1
            if (PlayerPrefs.GetInt("Threat Level") >= 1)
            {
                timeSpawn();
                glitchSpawn();
            }
            //spawn room when threat level is 2
            if (PlayerPrefs.GetInt("Threat Level") >= 2)
            {
                wepJamSpawn();
                powBlockSpawn();
            }
            //spawn room when threat level is 2
            if (PlayerPrefs.GetInt("Threat Level") >= 3)
            {
                hazardSpawn();
                cacheSpawn();
            }
            //Adding new enemies
            StartCoroutine(addEnemy());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //room types wont overlap with necessary rooms with no enemies
        if (exit)
        {
            shop = false;
            time = false;
            wJam = false;
            pBlocked = false;
            glitch = false;
            hazard = false;
            cache = false;
            noEnemy = true;
            Destroy(eBox);
            Destroy(IBox);
            enemySpawnCount = 0;
            enemyCount = 0;
        }
        if (start)
        {
            shop = false;
            time = false;
            wJam = false;
            pBlocked = false;
            glitch = false;
            hazard = false;
            cache = false;
            Destroy(eBox);
            Destroy(IBox);
            enemySpawnCount = 0;
            enemyCount = 0;
        }
        if (shop)
        {
            noEnemy = true;
            time = false;
            wJam = false;
            pBlocked = false;
            glitch = false;
            hazard = false;
            cache = false;
            Destroy(eBox);
            enemySpawnCount = 0;
            enemyCount = 0;
            Destroy(IBox);
        }
        if (glitch)
        {
            noEnemy = true;
            time = false;
            wJam = false;
            pBlocked = false;
            shop = false;
            hazard = false;
            cache = false;
            Destroy(eBox);
            enemySpawnCount = 0;
            enemyCount = 0;
            Destroy(IBox);
        }
        if (cache)
        {
            noEnemy = true;
            time = false;
            wJam = false;
            pBlocked = false;
            shop = false;
            hazard = false;
            glitch = false;
            Destroy(eBox);
            enemySpawnCount = 0;
            enemyCount = 0;
            Destroy(IBox);
        }
        enemyOn = (enemyCount > 0) ? true : false;
        //Passive ability floor effect
        if(PlayerPrefs.GetInt("hOff") == 1)
        {
            if (player.pAbilDict["heat"])
                floor.GetComponent<SpriteRenderer>().color = hotColor;
            else
                floor.GetComponent<SpriteRenderer>().color = normalColor;
        }
        else if(PlayerPrefs.GetInt("cOff") == 1)
        {
            if (player.pAbilDict["cold"])
                floor.GetComponent<SpriteRenderer>().color = coldColor;
            else
                floor.GetComponent<SpriteRenderer>().color = normalColor;
        }
        else if(PlayerPrefs.GetInt("sOff") == 1)
        {
            if (player.pAbilDict["shock"] && !player.shockDam)
                floor.GetComponent<SpriteRenderer>().color = shockColor;
            else if (player.shockDam)
                floor.GetComponent<SpriteRenderer>().color = shockDamColor;
            else
                floor.GetComponent<SpriteRenderer>().color = normalColor;
        }
        else if(PlayerPrefs.GetInt("eOff") == 1)
        {
            if (player.pAbilDict["earth"])
                floor.GetComponent<SpriteRenderer>().color = earthColor;
            else
                floor.GetComponent<SpriteRenderer>().color = normalColor;
        }
        else if (PlayerPrefs.GetInt("dOff") == 1)
        {
             if (player.pAbilDict["decoy"])
                floor.GetComponent<SpriteRenderer>().color = decoyColor;
            else
                floor.GetComponent<SpriteRenderer>().color = normalColor;
        }
        
        //room clear cheat
        if (Input.GetKey(KeyCode.F1))
        {
            enemyCount = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Start")
        {
            start = true;
            Debug.Log("start");
        }
        //when the player enters a special room 
        if (other.name == "Player")
        {
            if (time && !entered)
                timeRoom();
            if (wJam && !entered)
                StartCoroutine(wJamRoom());
            if (pBlocked && !entered)
                StartCoroutine(powBlockRoom());
            if (hazard && !entered)
                StartCoroutine(hazardRoom());
            if (exit)
                Instantiate(exitPortal, transform.position, Quaternion.identity);
            if (glitch)
                Instantiate(gIcon, transform.position, Quaternion.identity);
            if (cache)
                Instantiate(cIcon, transform.position, Quaternion.identity);
            if (!entered && !noEnemy && !start)
            {
                StartCoroutine(eSpawn());
                itemSpawn();
            }
            entered = true;

        }
        //minimap icons
        if (other.CompareTag("Boss") && !boss)
            exit = true;
        if (other.CompareTag("Shop"))
        {
            shop = true;
            other.transform.parent = gameObject.transform;
        }
        if (other.CompareTag("Health"))
            other.transform.parent = gameObject.transform;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Shop") && exit)
        {
            Destroy(other.gameObject);
        }
        //turn items into children of the room
        if (other.CompareTag("BAmmo") || other.CompareTag("ShAmmo") || other.CompareTag("Health") || other.CompareTag("Glitch") || other.CompareTag("BWep0") ||
            other.CompareTag("BWep1") || other.CompareTag("BWep2") || other.CompareTag("BWep3") || other.CompareTag("SWep0") || other.CompareTag("SWep1") || 
            other.CompareTag("SWep2") || other.CompareTag("SWep3") || other.CompareTag("EWep0") || other.CompareTag("EWep1") || other.CompareTag("EWep2") ||
            other.CompareTag("EWep3") || other.CompareTag("LWep0") || other.CompareTag("LWep1") || other.CompareTag("LWep2") || other.CompareTag("LWep3") ||
            other.CompareTag("MWep0") || other.CompareTag("MWep1") || other.CompareTag("MWep2") || other.CompareTag("MWep3") || other.CompareTag("Cache"))
            other.transform.parent = gameObject.transform;

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            player.wepJam = false;
            player.powBlock = false;
        }
        //Lower enemy count when a enemy dies
        //if (other.CompareTag("Enemy"))
        //    enemyCount--;
        //Lower item count when a item is taken
        if (other.CompareTag("Box"))
            itemCount--;

    }
    IEnumerator addEnemy()
    {
        // Adding new enemies at the end of the level
        if (log.add003 && !log.del003)
        {
            enemies.Add(newEnemies[0]);
            yield return new WaitForSeconds(.5f);
            log.del003 = true;
        }
        if (log.add004 && !log.del004)
        {
            enemies.Add(newEnemies[1]);
            yield return new WaitForSeconds(.5f);
            log.del004 = true;
        }
        if (log.add005 && !log.del005)
        {
            enemies.Add(newEnemies[2]);
            yield return new WaitForSeconds(.5f);
            log.del005 = true;
        }
        if (log.add006 && !log.del006)
        {
            enemies.Add(newEnemies[3]);
            yield return new WaitForSeconds(.5f);
            log.del006 = true;
        }
        if (log.add007 && !log.del007)
        {
            enemies.Add(newEnemies[4]);
            yield return new WaitForSeconds(.5f);
            log.del007 = true;
        }
        if (log.add008 && !log.del008)
        {
            enemies.Add(newEnemies[5]);
            yield return new WaitForSeconds(.5f);
            log.del008 = true;
        }
        if (log.add009 && !log.del009)
        {
            enemies.Add(newEnemies[6]);
            yield return new WaitForSeconds(.5f);
            log.del009 = true;
        }
        if (log.add010 && !log.del010)
        {
            enemies.Add(newEnemies[7]);
            yield return new WaitForSeconds(.5f);
            log.del010 = true;
        }
        if (log.add011 && !log.del011)
        {
            enemies.Add(newEnemies[8]);
            yield return new WaitForSeconds(.5f);
            log.del011 = true;
        }
        if (log.add012 && !log.del012)
        {
            enemies.Add(newEnemies[9]);
            yield return new WaitForSeconds(.5f);
            log.del012 = true;
        }
    }
    void shopSpawn()
    {
        if (count.shopCount > 0)
        {
            //random chance of time room
            if (templates.rooms.Count >= Random.Range(8, templates.rooms.Count))
            {
                shop = true;
                Instantiate(sIcon, transform.position, Quaternion.identity);
                count.shopCount--;
                Debug.Log("shop");
            }
        }
    }
    void timeSpawn()
    {
        if (count.timeCount > 0)
        {
            //random chance of time room
            if (roomChance <= 2 && !start)
            {
                time = true;
                count.timeCount--;
                Debug.Log("time");
            }
        }
    }
    void wepJamSpawn()
    {
        if (count.wepJamCount > 0)
        {
            //random chance of time room
            if (roomChance == 3 && !start)
            {
                wJam = true;
                count.wepJamCount--;
                Debug.Log("jam");
            }
        }
    }
    void powBlockSpawn()
    {
        if (count.powBlockCount > 0)
        {
            //random chance of time room
            if (roomChance == 4 && !start)
            {
                pBlocked = true;
                count.powBlockCount--;
                Debug.Log("blocked");
            }
        }
    }
    void glitchSpawn()
    {
        if (count.glitchCount > 0)
        {
            //random chance of glitch room
            if (roomChance == 5 && !start)
            {
                glitch = true;
                count.glitchCount--;
                Debug.Log("glitch");
            }
        }
    }
    void hazardSpawn()
    {
        if (count.hazardCount > 0)
        {
            //random chance of hazard room
            if (roomChance == 6 && !start)
            {
                hazard = true;
                count.hazardCount--;
                Debug.Log("hazard");
            }
        }
    }
    void cacheSpawn()
    {
        if (count.cacheCount > 0)
        {
            //random chance of glitch room
            if (roomChance == 7 && !start)
            {
                cache = true;
                count.cacheCount--;
                Debug.Log("cache");
            }
        }
    }
    void timeRoom()
    {
        StartCoroutine(eSpawn());
        List<int> randomTime = new List<int> { 15, 25, 35 };
        timeCountdown.timerIsRunning = true;
        timeCountdown.timeRemaining = randomTime[Random.Range(0, 3)];
        timeCountdown.timeRemaining = 15;
        if (timeCountdown.timerIsRunning && timeCountdown.timeRemaining > 0)
            StartCoroutine(waveSpawn());
    }
    IEnumerator eSpawn()
    {
        enemySpawnCount = Random.Range(3, 7);
        enemyCount = enemySpawnCount;
        int postionNum = 0;
        Vector3 position = eSpawnPoints[postionNum].position;
        while (enemySpawnCount > 0)
        {
            var area = Instantiate(eArea, position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
            Instantiate(enemies[Random.Range(0, enemies.Count)], position, Quaternion.identity);
            Destroy(area);
            postionNum++;
            position = eSpawnPoints[postionNum].position;
            enemySpawnCount--;
        }

    }
    IEnumerator waveSpawn()
    {
        float spawnTime = 12.0f;
        while (timeCountdown.timeRemaining > 0 && enemyCount < 7)
        {
            yield return new WaitForSeconds(spawnTime);
            if (timeCountdown.timerIsRunning)
                StartCoroutine(eSpawn());
            eSpawnShuffle();
            iSpawnShuffle();
            if (itemCount <= 1)
                itemSpawn();

        }
    }
    IEnumerator wJamRoom()
    {
        Debug.Log("J");
        while (wJam)
        {
            player.jamImage.SetActive(true);
            player.wepJam = true;
            yield return new WaitForSeconds(3.0f);
            player.wepJam = false;
            player.jamImage.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            if (enemyCount <= 0)
            {
                wJam = false;
                player.jamImage.SetActive(false);
            }
        }


    }
    IEnumerator powBlockRoom()
    {
        Debug.Log("P");
        while (pBlocked)
        {
            player.blockImage.SetActive(true);
            player.powBlock = true;
            yield return new WaitForSeconds(3.0f);
            player.powBlock = false;
            player.blockImage.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            if (enemyCount <= 0)
            {
                pBlocked = false;
                player.blockImage.SetActive(false);
            }

        }

    }
    IEnumerator hazardRoom()
    {
        Debug.Log("H");
        while (hazard)
        {
            player.hazardImage.SetActive(true);
            yield return new WaitForSeconds(4.0f);
            Instantiate(hArea, player.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            if (enemyCount <= 0)
            {
                hazard = false;
                player.hazardImage.SetActive(false);
            }

        }

    }
    public void itemSpawn()
    {
        if(!boss)
            itemCount = Random.Range(2, 7);
        if (boss)
            itemCount = Random.Range(7, 11);
        int postionNum = 0;
        Vector3 position = iSpawnPoints[postionNum].position;
        while (itemCount > 0)
        {
            int rndNum = Random.Range(0, items.Length);
            GameObject b = Instantiate(items[rndNum], position, Quaternion.identity);
            b.transform.parent = this.transform;

            postionNum++;
            position = iSpawnPoints[postionNum].position;
            itemCount--;
        }
    }
    void eSpawnShuffle()
    {
        for (int i = 0; i < eSpawnPoints.Count; i++)
        {
            Transform temp = eSpawnPoints[i];
            int randomIndex = Random.Range(i, eSpawnPoints.Count);
            eSpawnPoints[i] = eSpawnPoints[randomIndex];
            eSpawnPoints[randomIndex] = temp;
        }
    }
    void iSpawnShuffle()
    {
        for (int i = 0; i < iSpawnPoints.Count; i++)
        {
            Transform temp = iSpawnPoints[i];
            int randomIndex = Random.Range(i, eSpawnPoints.Count);
            iSpawnPoints[i] = eSpawnPoints[randomIndex];
            iSpawnPoints[randomIndex] = temp;
        }
    }
}
