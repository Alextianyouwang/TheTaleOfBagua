
using System.IO;
using UnityEditor;
using UnityEngine;

public static class EditorUtil
{
    public static void DrawSeparator()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

    }

    public static T GetObject<T>(string objectNameWithExtension) where T : Object
    {
        string[] paths = Directory.GetFiles(Application.dataPath, objectNameWithExtension, SearchOption.AllDirectories);
        if (paths.Length == 0)
        {
            Debug.Log("Couldn't find '" + objectNameWithExtension + "', please add it manually");
            return null;
        }

        string assetPath = paths[0].Replace("\\", "/").Replace(Application.dataPath, "");
        string assetFullPath = "Assets" + assetPath;

        T loadedObject = AssetDatabase.LoadAssetAtPath<T>(assetFullPath);
        if (loadedObject == null)
        {
            Debug.Log("Failed to load asset at path: " + assetFullPath);
            return null;
        }

        return loadedObject;
    }
}
