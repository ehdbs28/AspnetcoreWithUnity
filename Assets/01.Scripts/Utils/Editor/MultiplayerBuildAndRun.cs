using UnityEditor;
using UnityEngine;

public class MultiplayerBuildAndRun
{
    [MenuItem("Tools/Run Multiplayer/Mac/1 Players")]
    static void PerformMacBuild1()
    {
        PerformMacBuild(1);
    }

    [MenuItem("Tools/Run Multiplayer/Mac/2 Players")]
    static void PerformMacBuild2()
    {
        PerformMacBuild(2);
    }

    [MenuItem("Tools/Run Multiplayer/Mac/3 Players")]
    static void PerformMacBuild3()
    {
        PerformMacBuild(3);
    }

    [MenuItem("Tools/Run Multiplayer/Mac/4 Players")]
    static void PerformMacBuild4()
    {
        PerformMacBuild(4);
    }

    static void PerformMacBuild(int playerCount)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Standalone,
            BuildTarget.StandaloneWindows
        );

        for (int i = 1; i <= playerCount; i++)
        {
            BuildPipeline.BuildPlayer(GetScenePaths(),
                "Builds/" + GetProjectName() + i.ToString() + "/" + GetProjectName() + i.ToString() + ".app",
                BuildTarget.StandaloneOSX, BuildOptions.AutoRunPlayer
            );
        }
    }
    
    static string GetProjectName()
    {
        var s = Application.dataPath.Split('/');
        return s[^2];
    }

    static string[] GetScenePaths()
    {
        var scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
    }
}