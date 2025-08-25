using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttackAI", menuName = "AI/BasicAttack")]
public class BasicAttackAI : EnemyAILogic
{
    public override void ExecuteAI(EnemyUnit self)
    {
        var target = BattleManager.Instance.player;
        if (target != null)
        {
            Debug.Log($"{self.unitName}이(가) 플레이어를 공격합니다.");
            var effect = new Effect { effectType = EffectType.Damage, value = 5 };
            target.ApplyEffect(effect);
        }
    }
}

