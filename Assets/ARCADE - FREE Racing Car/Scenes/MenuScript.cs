using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuScript : MonoBehaviour
{

    public Button playButton;
    public Button quitButton;

   

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
  
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
