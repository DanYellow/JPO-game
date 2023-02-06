// using System;
// using UnityEditor;
// using UnityEditor.Build;
// using UnityEditor.Build.Reporting;
// using UnityEngine;

public class BuildVersionProcessor //IPreprocessBuildWithReport
{
    // public int callbackOrder => 0;
    // private const string initialVersion = "0.0";
    
    // public void OnPreprocessBuild(BuildReport report) {
    //     // string currentVersion = FindCurrentVersion();
    //     // UpdateVersion(currentVersion);
    // }

    // private void UpdateVersion(string currentVersion)
    // {
    //     if(float.TryParse(currentVersion, out float versionNumber)) {
    //         float nextVersion = versionNumber + 0.01f;
    //         string date = DateTime.Now.ToString("d");

    //         PlayerSettings.bundleVersion = string.Format("Version [{0}] - {1}", nextVersion, date);
    //     }
    // }

    // private string FindCurrentVersion()
    // {
    //     string[] currentVersion = PlayerSettings.bundleVersion.Split('[', ']');

    //     return currentVersion.Length == 1 ? initialVersion : currentVersion[1];
    // }
}
