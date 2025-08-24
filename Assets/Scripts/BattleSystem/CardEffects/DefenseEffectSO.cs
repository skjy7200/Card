using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Effects/Defense")]
public class DefenseEffectSO : CardEffectSO
{
    public int defense = 1;
    public GameObject defenseEffectPrefab;

    public override void Execute(BattleContext ctx, UnitBase caster, IList<UnitBase> targets)
    {
        if (targets == null || targets.Count == 0) return;

        var eff = new Effect { effectType = EffectType.Shield, value = defense };

        foreach (var t in targets)
        {
            if (!t) continue;

            t.ApplyEffect(eff);
            Debug.Log($"[DefenseEffectSO] {t.unitName} 방어력 + {defense}, 현재 쉴드 {t.shield}");

        }

    }

}
