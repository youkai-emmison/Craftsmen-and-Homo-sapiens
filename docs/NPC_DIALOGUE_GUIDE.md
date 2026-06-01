# NPC Dialogue Guide

## 当前目标

NPC 对话系统只负责最小剧情演示：玩家靠近 NPC，按 `E` 弹出底部对话框，再用 `E` / `Space` / `Enter` 逐句推进。它不是任务系统，也不记录已读状态。

## 脚本职责

- `DialogueLine`：一行台词数据，包含说话人和正文。
- `DialogueSequence`：一组台词，使用 ScriptableObject 配置。
- `NpcDialogueTrigger`：挂在 NPC 上，检测玩家进入触发范围并按 `E` 开始对话。
- `DialoguePanelController`：只负责对话框显示、切换下一句、关闭。
- `DialogueInputController`：只负责对话打开后的继续和关闭输入。
- `NpcDialogueSceneSetupBuilder`：编辑器工具，用来在 `SampleScene` 里创建演示 NPC、对话框和两组台词资源。

## Unity 里怎么创建演示 NPC

1. 打开 `Assets/Scenes/SampleScene.unity`。
2. 点击顶部菜单 `Tools > Dialogue > Create Demo NPC Dialogue Setup`。
3. Hierarchy 里应该出现：
   - `NPC_ArchivistGuide`
   - `NPC_FieldTechnician`
   - `InventoryCanvas/DialoguePanel`
4. Project 里应该出现：
   - `Assets/Dialogue/OpeningGuideDialogue.asset`
   - `Assets/Dialogue/MidRoomWarningDialogue.asset`

## 如何修改台词

打开 `Assets/Dialogue/OpeningGuideDialogue.asset` 或 `Assets/Dialogue/MidRoomWarningDialogue.asset`，在 Inspector 里修改 `lines`：

- `speakerName`：对话框左上角显示的名字。
- `content`：当前行正文。

需要更多台词时，只增加 `lines` 数组长度即可。当前阶段不要做分支选项、任务状态或复杂剧情树。

## NPC 配置

NPC 需要：

- `SpriteRenderer`：临时占位外观。
- `BoxCollider2D`：勾选 `Is Trigger`，作为玩家靠近范围。
- `NpcDialogueTrigger`：拖入 `DialogueSequence`、`DialoguePanel` 和 `InteractPrompt`。

玩家必须设置 `Tag = Player`，否则 `NpcDialogueTrigger` 不会响应。

## 输入规则

- 靠近 NPC 后按 `E`：开始对话。
- 对话打开时按 `E` / `Space` / `Enter`：下一句。
- 对话打开时按 `Esc`：关闭对话框。
- 对话打开时攻击和背包快捷键会被屏蔽，避免一边对话一边误操作。

## 测试清单

1. Play 后靠近出生区 NPC，能看到 `Press E`。
2. 按 `E` 后底部对话框出现。
3. 按 `E` / `Space` / `Enter` 能逐句推进。
4. 最后一行后对话框自动关闭。
5. 按 `Esc` 可以提前关闭。
6. 离开 NPC 后不能再触发对话。
7. Player 移动、跳跃、攻击、技力条、背包和制作面板在非对话状态下仍正常。

如果看不到 NPC 或提示，先在 Hierarchy 搜索 `NPC_ArchivistGuide`。它应该在玩家出生点右侧一点点，提示文字是 `E Talk`。如果场景里没有它，点击 `Tools > Dialogue > Create Demo NPC Dialogue Setup` 重新生成。

## 暂时不做

- 分支剧情。
- 任务系统。
- 好感度。
- 语音。
- 打字机复杂特效。
- 剧情 CG。
- 存档记录已读状态。
