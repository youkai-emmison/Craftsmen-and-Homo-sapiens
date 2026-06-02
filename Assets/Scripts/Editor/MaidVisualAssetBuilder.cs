// Script purpose: Builds the generated maid sprite sheet into Unity sprites, clips, and an Animator Controller.
// Key variables:
// - SpriteSheetPath: Generated transparent PNG used for the Player visual.
// - FrameSize: Fixed cell size for the 4x4 sprite sheet.
// - ControllerPath: Visual-only Animator Controller assigned to MaidVisual.
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public static class MaidVisualAssetBuilder
{
    private const string SpriteSheetPath = "Assets/Art/Generated/Characters/maid_heroine_spritesheet.png";
    private const string AnimationFolder = "Assets/Art/Generated/Characters/Animations";
    private const string ControllerPath = "Assets/Art/Generated/Characters/MaidPlayerAnimator.controller";
    private const int FrameSize = 64;
    private const int FrameColumns = 4;
    private const int FrameRows = 4;
    private const int PixelsPerUnit = 64;

    private static readonly string[] AnimationNames =
    {
        "Idle",
        "Walk",
        "Attack",
        "Hurt"
    };

    [MenuItem("Tools/Player Visual/Build Maid Sprite Assets")]
    public static void BuildMaidSpriteAssets()
    {
        if (!File.Exists(SpriteSheetPath))
        {
            Debug.LogError($"MaidVisualAssetBuilder: Sprite sheet missing at {SpriteSheetPath}.");
            return;
        }

        EnsureFolder(AnimationFolder);
        ConfigureSpriteSheetImporter();
        Sprite[] sprites = LoadSlicedSprites();

        if (sprites.Length < FrameColumns * FrameRows)
        {
            Debug.LogError("MaidVisualAssetBuilder: Sprite sheet did not import into 16 frames.");
            return;
        }

        AnimationClip idleClip = CreateClip("Maid_Idle", sprites, 0, true);
        AnimationClip walkClip = CreateClip("Maid_Walk", sprites, 1, true);
        AnimationClip attackClip = CreateClip("Maid_Attack", sprites, 2, false);
        AnimationClip hurtClip = CreateClip("Maid_Hurt", sprites, 3, false);
        CreateAnimatorController(idleClip, walkClip, attackClip, hurtClip);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("MaidVisualAssetBuilder: Maid sprite sheet, clips, and Animator Controller are ready.");
    }

    private static void ConfigureSpriteSheetImporter()
    {
        TextureImporter importer = AssetImporter.GetAtPath(SpriteSheetPath) as TextureImporter;

        if (importer == null)
        {
            throw new InvalidOperationException("MaidVisualAssetBuilder: TextureImporter was not found.");
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = PixelsPerUnit;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;
#pragma warning disable 0618
        // Unity 2022 still supports this metadata API, and it keeps this generated sheet setup compact.
        importer.spritesheet = CreateSpriteMetadata();
#pragma warning restore 0618
        importer.SaveAndReimport();
    }

    private static SpriteMetaData[] CreateSpriteMetadata()
    {
        SpriteMetaData[] spriteMetadata = new SpriteMetaData[FrameColumns * FrameRows];
        int spriteIndex = 0;

        for (int rowIndex = 0; rowIndex < FrameRows; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < FrameColumns; columnIndex++)
            {
                spriteMetadata[spriteIndex] = new SpriteMetaData
                {
                    name = $"maid_{AnimationNames[rowIndex].ToLowerInvariant()}_{columnIndex + 1}",
                    rect = new Rect(columnIndex * FrameSize, (FrameRows - rowIndex - 1) * FrameSize, FrameSize, FrameSize),
                    alignment = (int)SpriteAlignment.Custom,
                    pivot = new Vector2(0.5f, 0.08f)
                };

                spriteIndex++;
            }
        }

        return spriteMetadata;
    }

    private static Sprite[] LoadSlicedSprites()
    {
        return AssetDatabase
            .LoadAllAssetsAtPath(SpriteSheetPath)
            .OfType<Sprite>()
            .OrderBy(sprite => sprite.name)
            .ToArray();
    }

    private static AnimationClip CreateClip(string clipName, Sprite[] sprites, int rowIndex, bool loopTime)
    {
        string clipPath = $"{AnimationFolder}/{clipName}.anim";
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);

        if (clip == null)
        {
            clip = new AnimationClip();
            AssetDatabase.CreateAsset(clip, clipPath);
        }

        clip.frameRate = 8f;

        EditorCurveBinding spriteBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = string.Empty,
            propertyName = "m_Sprite"
        };

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[FrameColumns];
        int startIndex = rowIndex * FrameColumns;

        for (int frameIndex = 0; frameIndex < FrameColumns; frameIndex++)
        {
            keyframes[frameIndex] = new ObjectReferenceKeyframe
            {
                time = frameIndex / clip.frameRate,
                value = sprites[startIndex + frameIndex]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, keyframes);
        AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
        clipSettings.loopTime = loopTime;
        AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
        EditorUtility.SetDirty(clip);
        return clip;
    }

    private static void CreateAnimatorController(AnimationClip idleClip, AnimationClip walkClip, AnimationClip attackClip, AnimationClip hurtClip)
    {
        if (File.Exists(ControllerPath))
        {
            AssetDatabase.DeleteAsset(ControllerPath);
        }

        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
        controller.AddParameter("IsMoving", AnimatorControllerParameterType.Bool);
        controller.AddParameter("Attack", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Hurt", AnimatorControllerParameterType.Trigger);

        AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
        AnimatorState idleState = CreateState(stateMachine, "Idle", idleClip, new Vector3(250f, 80f, 0f));
        AnimatorState walkState = CreateState(stateMachine, "Walk", walkClip, new Vector3(250f, 200f, 0f));
        AnimatorState attackState = CreateState(stateMachine, "Attack", attackClip, new Vector3(520f, 80f, 0f));
        AnimatorState hurtState = CreateState(stateMachine, "Hurt", hurtClip, new Vector3(520f, 200f, 0f));
        stateMachine.defaultState = idleState;

        AnimatorStateTransition idleToWalk = idleState.AddTransition(walkState);
        idleToWalk.hasExitTime = false;
        idleToWalk.AddCondition(AnimatorConditionMode.If, 0f, "IsMoving");

        AnimatorStateTransition walkToIdle = walkState.AddTransition(idleState);
        walkToIdle.hasExitTime = false;
        walkToIdle.AddCondition(AnimatorConditionMode.IfNot, 0f, "IsMoving");

        AnimatorStateTransition attackTransition = stateMachine.AddAnyStateTransition(attackState);
        attackTransition.hasExitTime = false;
        attackTransition.AddCondition(AnimatorConditionMode.If, 0f, "Attack");

        AnimatorStateTransition hurtTransition = stateMachine.AddAnyStateTransition(hurtState);
        hurtTransition.hasExitTime = false;
        hurtTransition.AddCondition(AnimatorConditionMode.If, 0f, "Hurt");

        AnimatorStateTransition attackToIdle = attackState.AddTransition(idleState);
        attackToIdle.hasExitTime = true;
        attackToIdle.exitTime = 0.9f;
        attackToIdle.duration = 0f;

        AnimatorStateTransition hurtToIdle = hurtState.AddTransition(idleState);
        hurtToIdle.hasExitTime = true;
        hurtToIdle.exitTime = 0.9f;
        hurtToIdle.duration = 0f;
        EditorUtility.SetDirty(controller);
    }

    private static AnimatorState CreateState(AnimatorStateMachine stateMachine, string stateName, Motion motion, Vector3 position)
    {
        AnimatorState state = stateMachine.AddState(stateName, position);
        state.motion = motion;
        return state;
    }

    private static void EnsureFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            return;
        }

        string parentFolder = Path.GetDirectoryName(folderPath)?.Replace("\\", "/");
        string folderName = Path.GetFileName(folderPath);

        if (string.IsNullOrEmpty(parentFolder) || string.IsNullOrEmpty(folderName))
        {
            throw new InvalidOperationException($"MaidVisualAssetBuilder: Invalid folder path '{folderPath}'.");
        }

        EnsureFolder(parentFolder);
        AssetDatabase.CreateFolder(parentFolder, folderName);
    }
}
