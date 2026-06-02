// Script purpose: Plays a small looping sprite-frame animation for demo NPC visuals.
// Key Inspector variables:
// - targetRenderer: SpriteRenderer that displays the current NPC frame.
// - frames: Ordered animation frames from first to last.
// - framesPerSecond: Playback speed for the idle loop.
using UnityEngine;

public class SimpleNpcSpriteAnimator : MonoBehaviour
{
    // Renderer that receives each frame.
    public SpriteRenderer targetRenderer;

    // Frames should come from one consistent sprite sheet to avoid size jumps.
    public Sprite[] frames;

    // Small idle loops should feel alive but not distracting.
    public float framesPerSecond = 6f;

    private int currentFrameIndex;
    private float nextFrameTime;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            Debug.LogError("SimpleNpcSpriteAnimator: Target Renderer is not assigned.", this);
            return;
        }

        if (frames == null || frames.Length == 0)
        {
            Debug.LogError("SimpleNpcSpriteAnimator: Frames are not assigned.", this);
            return;
        }

        targetRenderer.sprite = frames[0];
    }

    private void Update()
    {
        if (targetRenderer == null || frames == null || frames.Length == 0)
        {
            return;
        }

        if (Time.time < nextFrameTime)
        {
            return;
        }

        AdvanceFrame();
    }

    private void AdvanceFrame()
    {
        currentFrameIndex = (currentFrameIndex + 1) % frames.Length;
        targetRenderer.sprite = frames[currentFrameIndex];
        nextFrameTime = Time.time + 1f / framesPerSecond;
    }
}
