using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffectSO : ScriptableObject
{
    [TextArea] public string description;

    // 실행: caster(사용자), targets(대상들), 컨텍스트(매니저 모음)
    public abstract void Execute(BattleContext ctx, UnitBase caster, IList<UnitBase> targets);
}
