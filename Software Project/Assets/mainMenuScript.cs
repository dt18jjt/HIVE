using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //quit game
    public void quit()
    {
        Application.Quit();
    }
    // go to first level
    public void startGame()
    {
        SceneManager.LoadScene("1");
    }
    //go to tutorial
    public void tutorial()
    {

    }
}
