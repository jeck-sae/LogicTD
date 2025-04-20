using System.Collections;
using TMPro;
using UnityEngine;

namespace TowerDefense
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] float cursorSpeed;

        string title;
        private void Start()
        {
            Time.timeScale = 1;
            StartCoroutine(TitleCursorEffect());
        }


        IEnumerator TitleCursorEffect()
        {
            title = text.text;
            while (true)
            {
                text.text = title + "<color=#00000000>_</color>";
                yield return Helpers.GetWait(cursorSpeed);
                text.text = title + "_";
                yield return Helpers.GetWait(cursorSpeed);
            }
        }
    }
}
