using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
      public string SceneName = "SampleScene";
      public TextMeshProUGUI highscoreUI;

      private void Start()
      {
            highscoreUI.text = "Highest wave survived : " + SaveLoadManager.instance.LoadHighScore();
            Cursor.lockState = CursorLockMode.None;
      }

      public void StartGame()
      {
            SceneManager.LoadScene(SceneName);
      }

      public void Quit()
      {
            print("Closing Game");
            Application.Quit();
      }
}
