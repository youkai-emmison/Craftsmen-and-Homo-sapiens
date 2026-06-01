// Script purpose: Lets the player press E near an NPC to start a dialogue sequence.
// Key Inspector variables:
// - dialogueSequence: Lines this NPC will show.
// - dialoguePanel: Shared scene dialogue panel.
// - interactPrompt: Optional world-space prompt shown only while the player is nearby.
using UnityEngine;

public class NpcDialogueTrigger : MonoBehaviour
{
    // Dialogue content assigned per NPC.
    public DialogueSequence dialogueSequence;

    // Shared panel used to display the sequence.
    public DialoguePanelController dialoguePanel;

    // Prompt shown while the player is in range.
    public GameObject interactPrompt;

    // Key used to start the dialogue.
    public KeyCode interactKey = KeyCode.E;

    // Closing on exit keeps short demo conversations local to the NPC.
    public bool closeWhenPlayerLeaves = true;

    private bool playerInRange;

    private void Awake()
    {
        SetPromptVisible(false);
    }

    private void Update()
    {
        if (!playerInRange || dialoguePanel == null || dialoguePanel.IsOpen)
        {
            SetPromptVisible(playerInRange && dialoguePanel != null && !dialoguePanel.IsOpen);
            return;
        }

        if (Input.GetKeyDown(interactKey))
        {
            StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInRange = true;
        SetPromptVisible(true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // This keeps the prompt reliable when the player starts inside the trigger.
        playerInRange = true;
        SetPromptVisible(dialoguePanel != null && !dialoguePanel.IsOpen);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInRange = false;
        SetPromptVisible(false);

        if (closeWhenPlayerLeaves && dialoguePanel != null && dialoguePanel.IsOpen)
            dialoguePanel.Close();
    }

    private void StartDialogue()
    {
        if (dialogueSequence == null)
        {
            Debug.LogError("NpcDialogueTrigger: Dialogue Sequence is not assigned.", this);
            return;
        }

        dialoguePanel.BeginDialogue(dialogueSequence);
        SetPromptVisible(false);
    }

    private void SetPromptVisible(bool isVisible)
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(isVisible);
    }
}
