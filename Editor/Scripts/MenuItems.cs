using UnityEditor;
using UnityEngine;

public static partial class MenuItems
{
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