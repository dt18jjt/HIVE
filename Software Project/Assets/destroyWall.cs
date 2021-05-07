using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Door") || collision.collider.CompareTag("Blocked"))
            Destroy(gameObject);
    }
}
