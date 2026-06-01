// Script purpose: Holds a small ordered sequence of NPC dialogue lines.
// Key Inspector variables:
// - sequenceName: Editor-friendly label for this dialogue.
// - lines: Lines shown from first to last.
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Dialogue/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    // Editor label for quick identification.
    public string sequenceName;

    // Ordered dialogue content.
    public DialogueLine[] lines;
}
