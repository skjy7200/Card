using System.Collections;
using UnityEngine;

public class EnemyTurnState : ITurnState
{
    public void Enter()
    {
        Debug.Log("적 턴 시작");
        TurnManager.Instance.StartCoroutine(EnemyTurnRoutine());
    }

    public void Update() { }

    public void Exit()
    {
        Debug.Log("적 턴 종료");
    }

    private IEnumerator EnemyTurnRoutine()
    {
        foreach (var enemy in BattleManager.Instance.enemyUnits)
        {
            if (enemy != null && enemy.currentHP > 0)
            {
                enemy.ExecuteAI(); // 내부에서 aiLogic.ExecuteAI(this)
                yield return new WaitForSeconds(1f);
            }
        }

        TurnManager.Instance.ChangeState(new PlayerTurnState());
    }
}
