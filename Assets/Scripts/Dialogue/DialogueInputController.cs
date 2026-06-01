// Script purpose: Advances or closes an open dialogue box.
// Key Inspector variables:
// - dialoguePanel: Panel that receives advance and close commands.
// - advanceKey / alternateAdvanceKey / submitKey: Keys that advance dialogue.
// - closeKey: Key that closes dialogue early.
using UnityEngine;

public class DialogueInputController : MonoBehaviour
{
    // Dialogue UI controller assigned in the scene.
    public DialoguePanelController dialoguePanel;

    // Main continue key.
    public KeyCode advanceKey = KeyCode.E;

    // Secondary continue key.
    public KeyCode alternateAdvanceKey = KeyCode.Space;

    // Keyboard submit key.
    public KeyCode submitKey = KeyCode.Return;

    // Early close key.
    public KeyCode closeKey = KeyCode.Escape;

    private void Update()
    {
        if (dialoguePanel == null || !dialoguePanel.IsOpen)
        {
            return;
        }

        if (Input.GetKeyDown(closeKey))
        {
            dialoguePanel.Close();
            return;
        }

        if (Input.GetKeyDown(advanceKey) || Input.GetKeyDown(alternateAdvanceKey) || Input.GetKeyDown(submitKey))
        {
            dialoguePanel.Advance();
        }
    }
}
