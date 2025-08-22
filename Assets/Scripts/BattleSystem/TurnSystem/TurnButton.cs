using UnityEngine;
using UnityEngine.UI;

public class TurnButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            // 상태 전환을 FSM 구조에 맞게 처리
            if (TurnManager.Instance.CurrentState is PlayerTurnState)
                TurnManager.Instance.ChangeState(new EnemyTurnState());
        });
    }
}
