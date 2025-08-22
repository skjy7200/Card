using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Effects/Damage")]
public class DamageEffectSO : CardEffectSO
{
    public int damage = 1;
    public GameObject attackEffectPrefab;

    public override void Execute(BattleContext ctx, UnitBase caster, IList<UnitBase> targets)
    {
        if (targets == null || targets.Count == 0) return;

        var eff = new Effect { effectType = EffectType.Damage, value = damage };
        Debug.Log($"[DamageEffectSO] cast by {caster?.unitName}, damage={damage}, targets={targets.Count}");

        foreach (var t in targets)
        {
            if (!t) { Debug.Log("[DamageEffectSO] null target"); continue; }
            t.ApplyEffect(eff);
            Debug.Log($"[DamageEffectSO] applied to {t.unitName}, HP now {t.currentHP}");

            // 연출/로그 원하면 여기서:
            // 2. 카드별 공격 애니메이션 프리팹 재생
            if (attackEffectPrefab != null)
            {
                Debug.Log("공격 애니메이션 실행");
                GameObject fx = Object.Instantiate(
                    attackEffectPrefab,
                    t.transform.position, // 적 위치
                    Quaternion.identity
                );
                Debug.Log("Effect Spawned: " + fx.name);

                Animator animator = fx.GetComponent<Animator>();

                float length = animator.GetCurrentAnimatorStateInfo(0).length;
                Object.Destroy(fx, length);
            }

            // ctx?.Log($"{t.unitName} takes {damage} (HP: {t.currentHP})");
        }
    }
}
