using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] string sceneName = "SelectableLevel";
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
