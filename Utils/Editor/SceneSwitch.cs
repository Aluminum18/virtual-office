using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

public static class SceneSwitch
{
    private const string MAIN_SCENE = "Assets/Scenes/main.unity";
    private const string SPLASH_SCENE = "Assets/Scenes/splash.unity";

    [MenuItem("ChangeScene/To main")]
    public static void ToMainScene()
    {
        OpenScene(MAIN_SCENE);
    }

    [MenuItem("ChangeScene/To splash")]
    public static void ToSplashScene()
    {
        OpenScene(SPLASH_SCENE);
    }

    private static void OpenScene(string scenePath)
    {
        if (EditorApplication.isPlaying)
        {
            Debug.Log("Stop app to switch scene!");
            return;
        }

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(scenePath);
    }

}
