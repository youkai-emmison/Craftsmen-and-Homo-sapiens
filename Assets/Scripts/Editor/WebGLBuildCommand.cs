// WebGLBuildCommand prepares Unity WebGL build and deploy folders for submission.
// Key variables:
// - webGLBuildPath: Unity WebGL build output directory.
// - deployFolderPath: Static-site folder copied from a completed WebGL build.

using System.IO;
using UnityEditor;
using UnityEngine;

public static class WebGLBuildCommand
{
    public const string webGLBuildPath = "Build/WebGL";
    public const string deployFolderPath = "Submission/WebGLSite";

    public static void BuildWebGL()
    {
        string[] enabledScenes = GetEnabledScenePaths();

        if (enabledScenes.Length == 0)
        {
            Debug.LogError("WebGL build stopped: no enabled scenes found in Build Settings. Add Assets/Scenes/SampleScene.unity before building.");
            return;
        }

        Directory.CreateDirectory(webGLBuildPath);

        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = enabledScenes,
            locationPathName = webGLBuildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildOptions);
        Debug.Log($"WebGL build requested. Output path: {webGLBuildPath}");
    }

    public static void PrepareDeployFolder()
    {
        if (!Directory.Exists(webGLBuildPath))
        {
            Debug.LogError($"Deploy folder preparation stopped: {webGLBuildPath} does not exist. Build WebGL first.");
            return;
        }

        if (Directory.Exists(deployFolderPath))
        {
            Directory.Delete(deployFolderPath, true);
        }

        CopyDirectory(webGLBuildPath, deployFolderPath);
        File.WriteAllText(
            Path.Combine(deployFolderPath, "README_DEPLOY.txt"),
            "Static Unity WebGL deploy folder for hackathon submission. Upload this folder content to Render Static Site, Cloudflare Pages, or GitHub Pages.\n");

        Debug.Log($"Deploy folder prepared: {deployFolderPath}");
    }

    private static string[] GetEnabledScenePaths()
    {
        var scenes = EditorBuildSettings.scenes;
        var enabledScenePaths = new System.Collections.Generic.List<string>();

        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.enabled)
            {
                enabledScenePaths.Add(scene.path);
            }
        }

        return enabledScenePaths.ToArray();
    }

    private static void CopyDirectory(string sourcePath, string destinationPath)
    {
        Directory.CreateDirectory(destinationPath);

        foreach (string filePath in Directory.GetFiles(sourcePath))
        {
            string fileName = Path.GetFileName(filePath);
            File.Copy(filePath, Path.Combine(destinationPath, fileName), true);
        }

        foreach (string directoryPath in Directory.GetDirectories(sourcePath))
        {
            string directoryName = Path.GetFileName(directoryPath);
            CopyDirectory(directoryPath, Path.Combine(destinationPath, directoryName));
        }
    }
}
