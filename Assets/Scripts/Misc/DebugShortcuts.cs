using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugShortcuts : MonoBehaviour
{
    float timeScaleBeforePausing;

    private void Update()
    {
        //toggle fullscreen and save
        if (Input.GetKeyDown(KeyCode.F11))
        {
            bool toggle = !Screen.fullScreen;
            Screen.fullScreen = toggle;

            SaveManager.Instance.data.fullscreen = toggle;
            SaveManager.SaveState();
        }

        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene("LoadableLevel");
            
        if (Input.GetKeyDown(KeyCode.F2))
            SceneManager.LoadScene("LevelEditor");

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            //lower volume and save
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                AudioListener.volume = Mathf.Clamp01(AudioListener.volume - .1f);

                SaveManager.Instance.data.volume = AudioListener.volume;
                SaveManager.SaveState();
            }

            //increase volume and save
            if (Input.GetKeyDown(KeyCode.Period))
            {
                AudioListener.volume = Mathf.Clamp01(AudioListener.volume + .1f);

                SaveManager.Instance.data.volume = AudioListener.volume;
                SaveManager.SaveState();
            }


            if (Input.GetKeyDown(KeyCode.G))
            {
                var g = FindAnyObjectByType<LevelBGIconSpawner>(FindObjectsInactive.Include);
                if(g) g.gameObject.SetActive(!g.isActiveAndEnabled);
            }

            //+1000 coins
            if (Input.GetKeyDown(KeyCode.M))
            {
                GameStats.Instance?.ModifyCoins(1000);
                Debug.Log("+1000 coins");
            }

            //Kill enemies
            if (Input.GetKeyDown(KeyCode.K))
            {
                GameManager.Instance?.enemies.ForEach(x => x.Damage(10000));
                Debug.Log("Killed all enemies");
            }

            //Load main scene
            if (Input.GetKeyDown(KeyCode.W))
            {
                SceneManager.LoadScene(0);
                Debug.Log("Loaded main scene");
            }

            //Speed up
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Time.timeScale *= 2;
                Debug.Log($"TimeScale: {Time.timeScale}");
            }
            
            //Slow down
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Time.timeScale /= 2;
                Debug.Log($"TimeScale: {Time.timeScale}");
            }

            //Reset time scale
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Time.timeScale = 1;
                Debug.Log($"TimeScale: {Time.timeScale}");
            }

            //Toggle pause
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (Time.timeScale > 0f)
                {
                    timeScaleBeforePausing = Time.timeScale;
                    Time.timeScale = 0;
                    Debug.Log("Paused");
                }
                else
                {
                    Time.timeScale = timeScaleBeforePausing;
                    Debug.Log("Resumed");
                }
            }
        }
    }
}
