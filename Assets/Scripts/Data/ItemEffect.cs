using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    [Header("特效信息")]
    public string effectName;
    [TextArea] public string description;

    public abstract void ExecuteEffect(Transform target);
    public virtual void CancelEffect(Transform target) { }
}
