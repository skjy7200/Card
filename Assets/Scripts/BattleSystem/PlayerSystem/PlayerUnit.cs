using UnityEngine;
using System.Collections.Generic;

public class PlayerUnit : UnitBase
{

    [SerializeField] private DamageFlash hitEffect;
    private PlayerEffectController playerEffects; // 이름 변경 권장

    protected override void Awake() // private → protected override
    {
        base.Awake();                // HP 초기화 보장
        playerEffects = GetComponent<PlayerEffectController>();
        if (!hitEffect) hitEffect = GetComponentInChildren<DamageFlash>(true);
    }

    private void Start()
    {
        Debug.Log($"{unitName}(플레이어 유닛) 생성 완료 HP={currentHP}/{maxHP}");
    }

    public void HandleInput()
    {
        Debug.Log($"{unitName} 입력 처리 중...");
    }

    public override void ApplyEffects(IEnumerable<Effect> effects)
    {
        // 이 enumerable을 두 번 돌릴 수 있게 안전하게 복제 (예방용)
        var list = effects as IList<Effect> ?? new List<Effect>(effects);

        bool hasDamage = false;
        for (int i = 0; i < list.Count; i++)
            if (list[i].effectType == EffectType.Damage && list[i].value > 0) { hasDamage = true; break; }

        base.ApplyEffects(list); //  같은 시그니처 호출

        if (hasDamage)
        {
            hitEffect?.OnDamaged();
            // EffectManager.Instance?.HitStop(0.05f);
            // EffectManager.Instance?.Shake();
        }

        Debug.Log("[PlayerUnit] ApplyEffects 호출됨");
    }

    protected override void OnDeath()
    {
        // 플레이어 전용 처리(예: UI 띄우기)를 base 이전에 할지 이후에 할지 결정
        // PlayerHUD.ShowRetry();
        base.OnDeath(); // base에서 Destroy 예약됨
    }
}