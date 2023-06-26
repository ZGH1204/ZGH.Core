using UnityEngine;
using ZGH.Core;

public class KeyCodeTimeScale : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInit()
    {
        new GameObject("Editor-KeyCodeTimeScale", typeof(KeyCodeTimeScale)).DontDestroy();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            Time.timeScale = 0.2f;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            Time.timeScale = 5f;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            Time.timeScale = 0f;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            Time.timeScale = 1f;
        }
    }
}