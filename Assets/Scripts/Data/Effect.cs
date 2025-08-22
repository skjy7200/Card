// Assets/Scripts/Data/Effect.cs
using UnityEngine;

public enum EffectType
{
    Damage, // 적에게 피해
    Shield, // 방어도 부여, 방어도는 받은 피해를 흡수
    Buff, // 아군 또는 자신에게 능력치 상승 효과
    Debuff, // 적에게 능력치 감소 or 행동제한 효과 부여
    StatusEffect, // 중첩되거나 지속되는 상태 이상 부여
    Cleanse, // 아군의 디버프 또는 상태이상 해제
    ChangeDeck // 덱에 있는 카드에 영향을 주는 특수 효과 (덱 섞기, 특정 카드 추가/삭제 등)
}

public enum StatusEffectType
{
    None,       // 기본값
    Burn,       // 화상 (턴마다 피해)
    Poison,     // 중독 (지속 피해)
    Freeze,     // 행동 불가
    Stun,       // 기절 (턴 스킵)
    Vulnerable  // 받는 피해 증가
}

[System.Serializable]
public class Effect
{
    public EffectType effectType;
    public int value;
    public int duration;
    public string description;

    public BuffType buffType;             // EffectType == Buff일 때 사용
    public DebuffType debuffType;         // EffectType == Debuff일 때 사용
    public StatusEffectType statusEffect;
}


// BuffType, DebuffType 열거형도 필요하면 나중에 추가 가능.

public enum BuffType
{
    IncreaseAttack,    // 공격력 증가
    IncreaseDefense,   // 방어력 증가
    RegenHP,           // 매턴 체력 회복
    GainAP,            // AP 회복
    DrawExtraCard,     // 카드 추가 드로우
    Invincible         // 피해 무시
}

public enum DebuffType
{
    DecreaseAttack,    // 공격력 감소
    DecreaseDefense,   // 방어력 감소
    Bleeding,          // 출혈 (지속 피해)
    Silence,           // 카드 사용 불가
    Fragile,           // 방어 무시 피해 증가
    LoseAP             // AP 감소
}