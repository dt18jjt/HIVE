using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardScript : MonoBehaviour
{
    bool active = false;
    public Color inactiveColor, activeColor;
    PlayerStat stat;
    // Start is called before the first frame update
    void Start()
    {
        stat = GameObject.Find("Player").GetComponent<PlayerStat>();
        gameObject.GetComponent<SpriteRenderer>().color = inactiveColor;
        StartCoroutine(activeOn());
    }
    IEnumerator activeOn()
    {
        yield return new WaitForSeconds(2.0f);
        active = true;
        gameObject.GetComponent<SpriteRenderer>().color = activeColor;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Player" && active)
        {
            stat.Damage(30);
            Destroy(gameObject);
        }
    }
}
