// SubmissionScreenshotCapture saves current Game View frames for poster and PPT use.
// Key variables:
// - ScreenshotFolderPath: repository folder where screenshots are saved.
// - OrderedScreenshotNames: suggested capture sequence for the demo video route.

using System.IO;
using UnityEditor;
using UnityEngine;

public static class SubmissionScreenshotCapture
{
    private const string ScreenshotFolderPath = "Submission/screenshots";

    private static readonly string[] OrderedScreenshotNames =
    {
        "01_title_or_spawn.png",
        "02_intro_memory_log.png",
        "03_early_room_combat.png",
        "04_level_up_or_growth.png",
        "05_mid_room_enemy.png",
        "06_boss_room.png",
        "07_victory_screen.png"
    };

    [MenuItem("Tools/Hackathon/Capture Submission Screenshots")]
    public static void CaptureSubmissionScreenshot()
    {
        string screenshotDirectory = GetScreenshotDirectory();
        Directory.CreateDirectory(screenshotDirectory);

        string screenshotPath = GetNextScreenshotPath(screenshotDirectory);
        ScreenCapture.CaptureScreenshot(screenshotPath, 1);

        Debug.Log($"Submission screenshot requested: {screenshotPath}. If the file does not appear immediately, wait a moment or leave Play Mode.");
    }

    [MenuItem("Tools/Hackathon/Open Screenshot Folder")]
    public static void OpenScreenshotFolder()
    {
        string screenshotDirectory = GetScreenshotDirectory();
        Directory.CreateDirectory(screenshotDirectory);
        EditorUtility.RevealInFinder(screenshotDirectory);
    }

    private static string GetScreenshotDirectory()
    {
        string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        return Path.Combine(projectRoot, ScreenshotFolderPath);
    }

    private static string GetNextScreenshotPath(string screenshotDirectory)
    {
        foreach (string screenshotName in OrderedScreenshotNames)
        {
            string candidatePath = Path.Combine(screenshotDirectory, screenshotName);

            if (!File.Exists(candidatePath))
            {
                return candidatePath;
            }
        }

        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return Path.Combine(screenshotDirectory, $"submission_screenshot_{timestamp}.png");
    }
}
