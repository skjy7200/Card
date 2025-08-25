using UnityEngine;
using System.Collections.Generic;
using System;

public class UnitBase : MonoBehaviour
{
    public string unitName;
    public int maxHP = 20;
    public int currentHP;
    public int shield;
    public List<TimedEffect<BuffType>> activeBuffs = new();
    public List<TimedEffect<DebuffType>> activeDebuffs = new();
    public List<TimedEffect<StatusEffectType>> activeStatusEffects = new();

    private bool _isDead;
    public event Action<UnitBase> Died;

    protected virtual void Awake() => currentHP = maxHP;

    public virtual void ApplyEffects(IEnumerable<Effect> effects)
    {
        if (effects == null) return;

        // 1) 묶어서 정렬/그룹화 (순서 정책 예시)
        // 예: Cleanse → Shield → Damage → Buff → Debuff → Status 순으로
        var bucket = new Dictionary<EffectType, List<Effect>>();
        foreach (var e in effects)
        {
            if (!bucket.TryGetValue(e.effectType, out var list))
            {
                list = new List<Effect>();
                bucket[e.effectType] = list;
            }
            list.Add(e);
        }

        // 2) 순서 정의 (게임 규칙에 맞게 조정)
        ApplyCleanse(bucket);
        ApplyShields(bucket);
        ApplyDamages(bucket);   // 쉴드와의 상호작용 확정
        ApplyBuffs(bucket);
        ApplyDebuffs(bucket);
        ApplyStatuses(bucket);

        // 3) 공통 후처리(사망 체크/이벤트 발생/로그 등)
        AfterEffectsResolved();
    }

    public virtual void ApplyEffect(Effect e)
    {
        if (e == null) return;
        ApplyEffects(new[] { e });
    }

    void ApplyCleanse(Dictionary<EffectType, List<Effect>> b)
    {
        if (!b.TryGetValue(EffectType.Cleanse, out var list)) return;
        foreach (var _ in list)
        {
            activeDebuffs.Clear();
            activeStatusEffects.Clear();
            Debug.Log($"{unitName} 디버프/상태이상 해제");
        }
    }

    void ApplyShields(Dictionary<EffectType, List<Effect>> b)
    {
        if (!b.TryGetValue(EffectType.Shield, out var list)) return;
        foreach (var e in list)
        {
            shield += (int)e.value;
            Debug.Log($"{unitName} 방어 {e.value} 획득 (총 {shield})");
        }
    }

    void ApplyDamages(Dictionary<EffectType, List<Effect>> b)
    {
        if (!b.TryGetValue(EffectType.Damage, out var list)) return;
        foreach (var e in list)
        {
            int raw = Mathf.Max(0, (int)e.value);
            int blocked = Mathf.Min(shield, raw);
            shield -= blocked;
            int dmg = raw - blocked;
            currentHP = Mathf.Max(0, currentHP - dmg);
            Debug.Log($"{unitName} {dmg} 피해 (쉴드 {blocked} 흡수, 남은 HP {currentHP}, 쉴드 {shield})");
            if (currentHP <= 0) OnDeath();
        }
    }

    void ApplyBuffs(Dictionary<EffectType, List<Effect>> b)
    {
        if (!b.TryGetValue(EffectType.Buff, out var list)) return;
        foreach (var e in list)
            activeBuffs.Add(new TimedEffect<BuffType>(e.buffType, e.duration));
    }

    void ApplyDebuffs(Dictionary<EffectType, List<Effect>> b)
    {
        if (!b.TryGetValue(EffectType.Debuff, out var list)) return;
        foreach (var e in list)
            activeDebuffs.Add(new TimedEffect<DebuffType>(e.debuffType, e.duration));
    }

    void ApplyStatuses(Dictionary<EffectType, List<Effect>> b)
    {
        if (!b.TryGetValue(EffectType.StatusEffect, out var list)) return;
        foreach (var e in list)
            activeStatusEffects.Add(new TimedEffect<StatusEffectType>(e.statusEffect, e.duration));
    }

    protected virtual void AfterEffectsResolved()
    {
        // 사망 애니, 콜백, 로그 묶음 등 공통 후처리
    }

    protected virtual void OnDeath()
    {
        if (_isDead) return;      // 중복 호출 방지
        _isDead = true;

        Debug.Log($"{unitName}이(가) 쓰러졌습니다.");

        // 연출 (선택)
        GetComponentInChildren<DamageFlash>()?.OnDamaged(); // 마지막 플래시 등
        // EffectManager.Instance?.PlayHitVFX(transform.position);
        // EffectManager.Instance?.PlayHitSFX();

        // 충돌/입력 비활성화 (선택)
        foreach (var col in GetComponentsInChildren<Collider2D>()) col.enabled = false;

        // 외부 알림 (선택)
        Died?.Invoke(this);
        // BattleManager.Instance?.OnUnitDied(this); // 구현했을 때만 사용

        // 파괴 타이밍 (애니메이션이 있다면 Destroy 지연)
        Destroy(gameObject, 0.5f);
    }

}
