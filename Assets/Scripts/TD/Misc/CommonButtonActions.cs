using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace TowerDefense
{
    public class CommonButtonActions : MonoBehaviour
    {
        public void StartGame(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
    
}
