// HackathonBuildMenu exposes submission build commands in the Unity editor menu.
// Key variables:
// - BuildWebGLMenuPath: menu item used to create the WebGL build.
// - PrepareDeployMenuPath: menu item used to copy the build into a static deploy folder.

using UnityEditor;

public static class HackathonBuildMenu
{
    private const string BuildWebGLMenuPath = "Tools/Hackathon/Build WebGL";
    private const string PrepareDeployMenuPath = "Tools/Hackathon/Prepare Deploy Folder";

    [MenuItem(BuildWebGLMenuPath)]
    public static void BuildWebGLFromMenu()
    {
        WebGLBuildCommand.BuildWebGL();
    }

    [MenuItem(PrepareDeployMenuPath)]
    public static void PrepareDeployFolderFromMenu()
    {
        WebGLBuildCommand.PrepareDeployFolder();
    }
}
