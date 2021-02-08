using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : MonoBehaviour
{
    private RoomTemplates templates;
    public int rand;
    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        rand = Random.Range(0, templates.startRooms.Length);
        Instantiate(templates.startRooms[rand], transform.position, templates.startRooms[rand].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
