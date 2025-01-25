using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   [SerializeField] private SceneField MainLevel;

   public void StartGame()
   {
      SceneManager.LoadScene(MainLevel.SceneName);
   }

   public void QuitGame()
   {
      Application.Quit();
   }
   
}
