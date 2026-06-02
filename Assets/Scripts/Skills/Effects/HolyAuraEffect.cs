using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 神圣光环效果 —— 激活后获得持续一段时间的复活增益。
/// 若玩家在增益期间死亡，则播放 HolyCross 特效并以半血复活。
/// </summary>
[CreateAssetMenu(fileName = "HolyAuraEffect", menuName = "Skills/Effects/Holy Aura")]
public class HolyAuraEffect : SkillEffect
{
    [Header("增益设置")]
    [SerializeField] private float buffDuration = 20f;

    [Header("复活设置")]
    [Range(0.1f, 1f)]
    [SerializeField] private float reviveHealthPercent = 0.5f;

    [Header("视觉")]
    [SerializeField] private Color buffTintColor = new Color(1f, 0.95f, 0.7f, 1f);

    // 每个施法者的独立状态（ScriptableObject 是共享资产，需要按 caster 区分）
    private readonly Dictionary<int, BuffState> activeBuffs = new Dictionary<int, BuffState>();

    private class BuffState
    {
        public bool active;
        public float timer;
        public Entity entity;
        public CharacterStats characterStats;
        public SpriteRenderer spriteRenderer;
        public Color originalColor;
        public HolyVfxSpawner vfxSpawner;
        public Func<bool> deathCallback; // 保存委托引用，用于正确取消订阅
    }

    public override bool IsActive => activeBuffs.Count > 0;

    /// <summary>
    /// 查询指定施法者的增益是否激活中。
    /// </summary>
    public bool IsBuffActiveFor(Transform caster)
    {
        return caster != null && activeBuffs.ContainsKey(caster.GetInstanceID())
            && activeBuffs[caster.GetInstanceID()].active;
    }

    public override bool Activate(Transform caster, SkillManager manager)
    {
        if (caster == null) return false;

        int id = caster.GetInstanceID();

        // 已激活时忽略重复
        if (activeBuffs.ContainsKey(id) && activeBuffs[id].active)
            return false;

        // 获取组件
        var entity = caster.GetComponent<Entity>();
        var characterStats = caster.GetComponent<CharacterStats>();
        var spriteRenderer = caster.GetComponentInChildren<SpriteRenderer>();
        var vfxSpawner = caster.GetComponent<HolyVfxSpawner>();

        if (entity == null) return false;

        // 捕获 caster 引用供委托使用
        Transform casterRef = caster;

        var state = new BuffState
        {
            active = true,
            timer = buffDuration,
            entity = entity,
            characterStats = characterStats,
            spriteRenderer = spriteRenderer,
            originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white,
            vfxSpawner = vfxSpawner
        };

        // 创建委托并保存引用，以便后续取消订阅
        state.deathCallback = () => HandleBeforeDeath(casterRef);

        activeBuffs[id] = state;

        // 订阅死亡拦截
        entity.OnBeforeDeath += state.deathCallback;

        // 视觉反馈
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(buffTintColor.r, buffTintColor.g, buffTintColor.b, state.originalColor.a);

        Debug.Log($"神圣光环已激活，持续 {buffDuration} 秒。");
        return true;
    }

    public override void Deactivate(Transform caster)
    {
        if (caster == null) return;

        int id = caster.GetInstanceID();
        if (!activeBuffs.TryGetValue(id, out var state)) return;

        DeactivateInternal(caster, state, id);
    }

    /// <summary>
    /// 由 SkillManager 每帧调用，更新增益计时器。
    /// </summary>
    public override void Tick(Transform caster, float deltaTime)
    {
        if (caster == null) return;

        int id = caster.GetInstanceID();
        if (!activeBuffs.TryGetValue(id, out var state) || !state.active) return;

        state.timer -= deltaTime;
        if (state.timer <= 0f)
        {
            DeactivateInternal(caster, state, id);
            Debug.Log("神圣光环已失效。");
        }
    }

    private bool HandleBeforeDeath(Transform caster)
    {
        int id = caster.GetInstanceID();
        if (!activeBuffs.TryGetValue(id, out var state) || !state.active)
            return false;

        // 执行复活
        RevivePlayer(state, caster);

        // 消耗增益
        DeactivateInternal(caster, state, id);

        return true; // 拦截死亡
    }

    private void RevivePlayer(BuffState state, Transform caster)
    {
        float reviveHealth;

        // 恢复 Entity 血量
        if (state.entity != null)
        {
            reviveHealth = state.entity.maxHealth * reviveHealthPercent;
            state.entity.currentHealth = reviveHealth;
        }

        // 同步 CharacterStats 血量
        if (state.characterStats != null)
        {
            reviveHealth = state.characterStats.maxHealth.Value * reviveHealthPercent;
            state.characterStats.currentHealth = reviveHealth;
        }

        // 生成 HolyCross 特效
        if (state.vfxSpawner != null)
            state.vfxSpawner.SpawnAt(caster.position);

        Debug.Log($"神圣光环触发！玩家以 {reviveHealthPercent * 100}% 血量复活。");
    }

    private void DeactivateInternal(Transform caster, BuffState state, int id)
    {
        state.active = false;

        // 取消死亡拦截订阅（使用保存的委托引用）
        if (state.entity != null && state.deathCallback != null)
            state.entity.OnBeforeDeath -= state.deathCallback;

        // 恢复原始颜色
        if (state.spriteRenderer != null)
            state.spriteRenderer.color = state.originalColor;

        activeBuffs.Remove(id);
    }
}
