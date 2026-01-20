using System;
using _Datas;
using UnityEngine;
using System.Collections.Generic;
using _Scripts.Player.Definition;

public sealed class PlayerStatController : MonoBehaviour
{
    [Header("Base Config")]
    [SerializeField] private PlayerStatsConfig m_StatsConfig;

    // 내부 상태
    private PlayerStats m_BaseStats;
    private PlayerStats m_CurrentStats;
    private int m_CurrentHp;

    // Capacity는 가벼운 최적화(재할당 방지)로 8 정도 시작.
    private readonly List<StatModifier> m_Modifiers = new List<StatModifier>(8);

    public PlayerStats CurrentStats => m_CurrentStats;
    public int CurrentHp => m_CurrentHp;

    public event Action<int, int> OnHpChanged; // (current, max)

    private void Awake()
    {
        m_BaseStats = BuildBaseStats(m_StatsConfig);
        RebuildStats();

        m_CurrentHp = m_CurrentStats.MaxHp;
        OnHpChanged?.Invoke(m_CurrentHp, m_CurrentStats.MaxHp);
    }

    public void SetBaseConfig(PlayerStatsConfig config, bool keepHpRatio = true)
    {
        m_StatsConfig = config;

        float ratio = 1f;
        if (keepHpRatio)
        {
            int prevMax = Mathf.Max(1, m_CurrentStats.MaxHp);
            ratio = (float)m_CurrentHp / prevMax;
        }

        m_BaseStats = BuildBaseStats(m_StatsConfig);
        RebuildStats();

        if (keepHpRatio)
        {
            int newHp = Mathf.RoundToInt(m_CurrentStats.MaxHp * ratio);
            SetCurrentHp(newHp);
        }
        else
        {
            SetCurrentHp(Mathf.Min(m_CurrentHp, m_CurrentStats.MaxHp));
        }
    }

    private static PlayerStats BuildBaseStats(PlayerStatsConfig config)
    {
        if (config == null)
            return new PlayerStats(100, 3.5f, 5.5f);

        return new PlayerStats(config.MaxHp, config.WalkSpeed, config.SprintSpeed);
    }

    // ---- Modifier 관리 ----
    public void AddModifier(in StatModifier modifier)
    {
        m_Modifiers.Add(modifier);
        RebuildStats();
        SetCurrentHp(Mathf.Min(m_CurrentHp, m_CurrentStats.MaxHp));
    }

    public int RemoveModifiersBySourceId(int sourceId)
    {
        // 역순 제거로 안전/빠르게 처리
        int removed = 0;
        for (int i = m_Modifiers.Count - 1; i >= 0; i--)
        {
            if (m_Modifiers[i].SourceId == sourceId)
            {
                m_Modifiers.RemoveAt(i);
                removed++;
            }
        }

        if (removed > 0)
        {
            RebuildStats();
            SetCurrentHp(Mathf.Min(m_CurrentHp, m_CurrentStats.MaxHp));
        }

        return removed;
    }

    public void ClearAllModifiers()
    {
        if (m_Modifiers.Count == 0) return;

        m_Modifiers.Clear();
        RebuildStats();
        SetCurrentHp(Mathf.Min(m_CurrentHp, m_CurrentStats.MaxHp));
    }

    public void RebuildStats()
    {
        PlayerStats result = m_BaseStats;

        for (int i = 0; i < m_Modifiers.Count; i++)
        {
            ApplyModifier(ref result, m_Modifiers[i]);
        }

        // 안전 클램프
        if (result.MaxHp < 1) result.MaxHp = 1;
        if (result.WalkSpeed < 0f) result.WalkSpeed = 0f;
        if (result.SprintSpeed < 0f) result.SprintSpeed = 0f;

        m_CurrentStats = result;
    }

    private static void ApplyModifier(ref PlayerStats stats, in StatModifier mod)
    {
        switch (mod.StatType)
        {
            case PlayerStatType.MaxHp:
                if (mod.Operator == StatModifierOperator.Add) stats.MaxHp += Mathf.RoundToInt(mod.Value);
                else stats.MaxHp = Mathf.RoundToInt(stats.MaxHp * mod.Value);
                break;

            case PlayerStatType.WalkSpeed:
                if (mod.Operator == StatModifierOperator.Add) stats.WalkSpeed += mod.Value;
                else stats.WalkSpeed *= mod.Value;
                break;

            case PlayerStatType.SprintSpeed:
                if (mod.Operator == StatModifierOperator.Add) stats.SprintSpeed += mod.Value;
                else stats.SprintSpeed *= mod.Value;
                break;
        }
    }

    public void Damage(int amount)
    {
        if (amount <= 0) return;
        SetCurrentHp(m_CurrentHp - amount);
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        SetCurrentHp(m_CurrentHp + amount);
    }

    public void SetHpToMax()
    {
        SetCurrentHp(m_CurrentStats.MaxHp);
    }

    private void SetCurrentHp(int newValue)
    {
        int max = m_CurrentStats.MaxHp;
        int clamped = Mathf.Clamp(newValue, 0, max);
        if (clamped == m_CurrentHp) return;

        m_CurrentHp = clamped;
        OnHpChanged?.Invoke(m_CurrentHp, max);
    }
}
