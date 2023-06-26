using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MenuTools : MonoBehaviour
{
    [MenuItem("Tools/PlayGame _F5")]
    private static void CustorkKeys_F5()
    {
        if (Application.isPlaying) {
            EditorApplication.ExitPlaymode();
        } else {
            var scenes = EditorBuildSettings.scenes;
            if (scenes.Length > 0) {
                var path = scenes[0].path;
                var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                if (!scene.path.Equals(path)) {
                    EditorSceneManager.SaveScene(scene);
                    EditorSceneManager.OpenScene(path);
                }
                EditorApplication.EnterPlaymode();
            }
        }
    }

    [MenuItem("Tools/Open Folder/PersistentDataPath", false, 1000)]
    public static void OpenFolder_PersistentDataPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    [MenuItem("Tools/Open Folder/StreamingAssetsPath", false, 1000)]
    public static void OpenFolder_StreamingAssetsPath()
    {
        EditorUtility.RevealInFinder(Application.streamingAssetsPath);
    }
}