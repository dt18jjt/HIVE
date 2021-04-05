using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaTrigger : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
            Destroy(gameObject);
    }
}
