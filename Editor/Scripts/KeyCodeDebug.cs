using UnityEngine;

public class KeyCodeDebug : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    private static void OnInit()
    {
        var go = GameObject.Find("DEBUG") ?? new GameObject("DEBUG", typeof(KeyCodeDebug));
        DontDestroyOnLoad(go);
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
        if (Input.GetKeyUp(KeyCode.Space)) {
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
        }
        if (Input.GetKeyUp(KeyCode.S)) {
        }
        if (Input.GetKeyUp(KeyCode.D)) {
        }
    }
}