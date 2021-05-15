using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{
    RoomTemplates room;
    // Start is called before the first frame update
    void Start()
    {
        room = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void resume()
    {
        room.paused = false;
        SceneManager.UnloadSceneAsync("Pause");
    }
    public void quit()
    {
        SceneManager.LoadScene("Main");
        PlayerPrefs.DeleteAll();
    }
}
