using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutateObjScript : MonoBehaviour
{
    GameObject player;
    public GameObject MP;
    public float speed = 8.0f;
    bool follow = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(follow)
            MP.transform.position = Vector3.MoveTowards(MP.transform.position, player.transform.position, speed);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
            follow = true;
    }
}
