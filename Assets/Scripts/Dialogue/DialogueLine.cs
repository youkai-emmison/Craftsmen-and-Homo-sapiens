// Script purpose: Stores one line of NPC dialogue text.
// Key Inspector variables:
// - speakerName: Name shown in the dialogue box.
// - content: Body text shown for this line.
using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    // Speaker label shown above the line.
    public string speakerName;

    // Main dialogue text.
    [TextArea(2, 4)]
    public string content;
}
