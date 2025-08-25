using UnityEngine;

public class PlayerTurnState : ITurnState
{
    public void Enter()
    {
        Debug.Log("▶ 플레이어 턴 시작");

        TurnManager.Instance.IncrementTurnCount();

        // AP 회복 로직
        if (TurnManager.Instance.TurnCount == 1)
            TurnManager.Instance.SetAP(3); // 첫 턴은 3
        else
            TurnManager.Instance.AddAP(1); // 이후는 1씩 추가

        Debug.Log($"[플레이어 턴 시작] 턴 {TurnManager.Instance.TurnCount}, AP: {TurnManager.Instance.currentAP}");
    }

    public void Update()
    {
        // 플레이어 입력 감지 등 (선택적으로 처리)
    }

    public void Exit()
    {
        Debug.Log(" 플레이어 턴 종료");
    }
}

