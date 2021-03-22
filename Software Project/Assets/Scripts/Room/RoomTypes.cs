using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTypes : MonoBehaviour
{
    int roomChance;
    public bool enemyOn = false, start = false, boss = false, shop = false, time = false, wJam = false, pBlocked = false,
        glitch = false, entered = false, noEnemy = false,  enemyBuff = false;
    private RoomCount count;
    private Countdown timeCountdown;
    private RoomTemplates templates;
    public GameObject sIcon, gIcon, eBox, IBox, box, eArea, exit, floor;
    public GameObject[] enemies;
    public GameObject[] items;
    public int enemySpawnCount, enemyCount, itemCount;
    public Color normalColor, coldColor, hotColor, shockColor, shockDamColor, earthColor, decoyColor;
    public List<Transform> eSpawnPoints;
    public List<Transform> iSpawnPoints;
    PlayerStat player;
    // Start is called before the first frame update
    void Start()
    {
        if (start)
            noEnemy = true;
        player = GameObject.Find("Player").GetComponent<PlayerStat>();
        count = GameObject.Find("Global").GetComponent<RoomCount>();
        timeCountdown = GameObject.Find("Global").GetComponent<Countdown>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        roomChance = Random.Range(1, 11);
        timeSpawn();
        shopSpawn();
        wepJamSpawn();
        powBlockSpawn();
        glitchSpawn();
        //sDeathSpawn();
        
    }

    // Update is called once per frame
    void Update(){
        if (boss)
        {
            shop = false;
            time = false;
            wJam = false;
            pBlocked = false;
            glitch = false;
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
            Destroy(eBox);
            enemySpawnCount = 0;
            enemyCount = 0;
            Destroy(IBox);
        }
        enemyOn = (enemyCount > 0) ? true : false;
        if (player.pAbilDict["heat"])
            floor.GetComponent<SpriteRenderer>().color = hotColor;
        else if (player.pAbilDict["cold"])
            floor.GetComponent<SpriteRenderer>().color = coldColor;
        else if (player.pAbilDict["shock"] && !player.shockDam)
            floor.GetComponent<SpriteRenderer>().color = shockColor;
        else if (player.shockDam)
            floor.GetComponent<SpriteRenderer>().color = shockDamColor;
        else if (player.pAbilDict["earth"])
            floor.GetComponent<SpriteRenderer>().color = earthColor;
        else if (player.pAbilDict["decoy"])
            floor.GetComponent<SpriteRenderer>().color = decoyColor;
        else
            floor.GetComponent<SpriteRenderer>().color = normalColor;
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
        if (other.name == "Player")
        {
            if (time && !entered)
                timeRoom();
            if (wJam && !entered)
                wJamRoom();
            if (pBlocked && !entered)
                player.powBlock = true;
            if (boss)
                Instantiate(exit, transform.position, Quaternion.identity);
            if (glitch)
                Instantiate(gIcon, transform.position, Quaternion.identity);
            if (!entered && !noEnemy)
            {
                StartCoroutine(eSpawn());
                itemSpawn();
            }
            entered = true;

        }
        if (other.CompareTag("Boss"))
            boss = true;
        if (other.CompareTag("Shop")){
            shop = true;
            other.transform.parent = gameObject.transform;
        }
        if (other.CompareTag("Health"))
            other.transform.parent = gameObject.transform;
        //if (other.CompareTag("Enemy")){
        //    if (sDeath)
        //    {
        //        EnemyFollow enemy = other.gameObject.GetComponent<EnemyFollow>();
        //        enemy.suddenDeath = true;
        //    }
        //}
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Shop") && boss)
        {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("BAmmo") || other.CompareTag("ShAmmo") || other.CompareTag("Health"))
            other.transform.parent = gameObject.transform;

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            player.wepJam = false;
            player.powBlock = false;
            player.suddenDeath = false;
        }
        if (other.CompareTag("Enemy"))
            enemyCount--;
        if (other.CompareTag("Box"))
            itemCount--;

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
            //random chance of time room
            if (roomChance == 5 && !start)
            {
                glitch = true;
                count.glitchCount--;
                Debug.Log("glitch");
            }
        }
    }
    void timeRoom()
    {
        StartCoroutine(eSpawn());
        List<int> randomTime = new List<int> { 15, 30, 45 };
        timeCountdown.timerIsRunning = true;
        //timeCountdown.timeRemaining = randomTime[Random.Range(0, 3)];
        timeCountdown.timeRemaining = 15;
        if (timeCountdown.timerIsRunning && timeCountdown.timeRemaining > 0 )
            StartCoroutine(waveSpawn());
    }
    IEnumerator eSpawn()
    {
        enemySpawnCount = Random.Range(3, 7);
        enemyCount = enemySpawnCount;
        int postionNum = 0;
        Vector3 position = eSpawnPoints[postionNum].position;
        while (enemySpawnCount > 0){
            var area = Instantiate(eArea, position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
            Instantiate(enemies[Random.Range(0, enemies.Length)], position, Quaternion.identity) ;
            Destroy(area);
            postionNum++;
            position = eSpawnPoints[postionNum].position;
            enemySpawnCount--;
        }

    }
    IEnumerator waveSpawn()
    {
        float spawnTime = 12.0f;
        while (timeCountdown.timeRemaining > 0 && enemyCount < 7 )
        {
            yield return new WaitForSeconds(spawnTime);
            if(timeCountdown.timerIsRunning)
                StartCoroutine(eSpawn());
            eSpawnShuffle();
            iSpawnShuffle();
            if (itemCount <= 1)
                itemSpawn();

        }
    }
    IEnumerator wJamRoom()
    {
        while (wJam)
        {
            player.wepJam = true;
            yield return new WaitForSeconds(3.0f);
            player.wepJam = false;
        }
            
    }
    IEnumerator powBlockRoom()
    {
        while (pBlocked)
        {
            player.powBlock = true;
            yield return new WaitForSeconds(3.0f);
            player.powBlock = false;
            yield return new WaitForSeconds(3.0f);
        }

    }
    void itemSpawn()
    {
        itemCount = Random.Range(2, 7);
        int postionNum = 0;
        Vector3 position = iSpawnPoints[postionNum].position;
        while (itemCount > 0){
            GameObject b = Instantiate(items[Random.Range(0,items.Length)], position, Quaternion.identity);
            b.transform.parent = this.transform;
            postionNum++;
            position = iSpawnPoints[postionNum].position;
            itemCount--;
        }
    }
    void eSpawnShuffle()
    {
        for (int i = 0; i < eSpawnPoints.Count; i++){
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
