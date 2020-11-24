using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

public static class SceneSwitch
{
    private const string MAIN_SCENE = "Assets/Scenes/main.unity";
    private const string SPLASH_SCENE = "Assets/Scenes/splash.unity";
    private const string LOBBY_SCENE = "Assets/Scenes/lobby.unity";
    private const string ROYALE_SCENE = "Assets/Scenes/RoyaleScene.unity";
    private const string ROOM_SCENE = "Assets/Scenes/room.unity";

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

    [MenuItem("ChangeScene/To lobby")]
    public static void ToLobbyScene()
    {
        OpenScene(LOBBY_SCENE);
    }

    [MenuItem("ChangeScene/To royale")]
    public static void ToRoyaleScene()
    {
        OpenScene(ROYALE_SCENE);
    }

    [MenuItem("ChangeScene/To room")]
    public static void ToRoomScene()
    {
        OpenScene(ROOM_SCENE);
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
