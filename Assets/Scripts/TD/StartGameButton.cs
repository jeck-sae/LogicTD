using UnityEngine;
using UnityEngine.SceneManagement;


namespace TowerDefense
{
    public class StartGameButton : MonoBehaviour
    {
        [SerializeField] string sceneName = "SelectableLevel";
        public void StartGame()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    
}
