using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   [SerializeField] private SceneField MainLevel;

   private void Start()
   {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
   }

   public void StartGame()
   {
      SceneManager.LoadScene(MainLevel.SceneName);
   }

   public void QuitGame()
   {
      Application.Quit();
   }
   
}
