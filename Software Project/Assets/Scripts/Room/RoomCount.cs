using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCount : MonoBehaviour
{
    public int timeCount;
    public int shopCount;
    public int keyCount;
    public int wepJamCount;
    public int powBlockCount;
    public int glitchCount;
    public int sDeathCount;
    // Start is called before the first frame update
    void Start()
    {
        timeCount = Random.Range(0, 3);
        wepJamCount = Random.Range(0, 3);
        powBlockCount = Random.Range(0, 3);
        glitchCount = Random.Range(0, 3);
        sDeathCount = Random.Range(0, 2);
        shopCount = 1;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
