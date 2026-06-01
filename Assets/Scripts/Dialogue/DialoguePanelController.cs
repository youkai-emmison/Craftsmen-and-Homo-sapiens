// Script purpose: Shows and advances the bottom dialogue box.
// Key Inspector variables:
// - canvasGroup: Controls panel visibility and input blocking.
// - speakerNameText / contentText / continueHintText: Text targets for the active line.
using TMPro;
using UnityEngine;

public class DialoguePanelController : MonoBehaviour
{
    // Global read-only flag used by light input scripts.
    public static bool IsAnyDialogueOpen { get; private set; }

    // Fades and blocks the whole dialogue panel.
    public CanvasGroup canvasGroup;

    // Text showing the current speaker.
    public TextMeshProUGUI speakerNameText;

    // Text showing the current dialogue content.
    public TextMeshProUGUI contentText;

    // Text showing the continue prompt.
    public TextMeshProUGUI continueHintText;

    // Prompt text shown while a dialogue is active.
    public string continueHint = "E / Space / Enter  Continue     Esc  Close";

    private DialogueSequence activeSequence;
    private int currentLineIndex;

    public bool IsOpen => activeSequence != null;

    private void Awake()
    {
        HidePanel();
    }

    public void BeginDialogue(DialogueSequence sequence)
    {
        if (sequence == null || sequence.lines == null || sequence.lines.Length == 0)
        {
            Debug.LogWarning("DialoguePanelController: Dialogue sequence is empty.", this);
            return;
        }

        activeSequence = sequence;
        currentLineIndex = 0;
        IsAnyDialogueOpen = true;
        ShowPanel();
        ShowCurrentLine();
    }

    public void Advance()
    {
        if (!IsOpen)
        {
            return;
        }

        currentLineIndex++;
        if (currentLineIndex >= activeSequence.lines.Length)
        {
            Close();
            return;
        }

        ShowCurrentLine();
    }

    public void Close()
    {
        activeSequence = null;
        IsAnyDialogueOpen = false;
        ClearText();
        HidePanel();
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = activeSequence.lines[currentLineIndex];

        if (speakerNameText != null)
            speakerNameText.text = line.speakerName;

        if (contentText != null)
            contentText.text = line.content;

        if (continueHintText != null)
            continueHintText.text = continueHint;
    }

    private void ShowPanel()
    {
        if (canvasGroup == null)
            return;

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void HidePanel()
    {
        if (canvasGroup == null)
            return;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void ClearText()
    {
        if (speakerNameText != null)
            speakerNameText.text = "";

        if (contentText != null)
            contentText.text = "";

        if (continueHintText != null)
            continueHintText.text = "";
    }
}
