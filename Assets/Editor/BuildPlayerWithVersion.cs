using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.IO;

// https://stackoverflow.com/questions/75630802/create-new-folder-for-the-project-to-get-build-into-throught-script-c-sharp
public class BuildPlayerWithVersion : MonoBehaviour
{
    [MenuItem("Build/Build With Version")]
    public static void MyBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions(); 
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        // BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(buildPlayerOptions);

        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "Builds", "");
        // Get your build version here
        string version = System.DateTime.Now.ToString("y-MM-dd-'h'-HH-mm-ss");
        // string path = Path.Join("Builds", "");
        // print(buildPlayerOptions.locationPathName);
        print(version);

        buildPlayerOptions.locationPathName = path;
        BuildPipeline.BuildPlayer(buildPlayerOptions);

        // Directory.CreateDirectory(path);      
        
        // BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        // BuildSummary summary = report.summary;

        // if (summary.result == BuildResult.Succeeded)
        // {
        //     Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        // }

        // if (summary.result == BuildResult.Failed)
        // {
        //     Debug.Log("Build failed");
        // }
    }
}