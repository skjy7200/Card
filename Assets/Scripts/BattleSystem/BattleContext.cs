using System.Collections.Generic;
using UnityEngine;

public class BattleContext
{
    public BattleManager BattleManager { get; }
    public TurnManager Turn => TurnManager.Instance; // 짧은 별칭
    public System.Random Random { get; }

    // 편의 프로퍼티 (널 세이프)
    public int TurnCount => Turn != null ? Turn.TurnCount : 0;
    public int CurrentAP => Turn != null ? Turn.currentAP : 0;
    public ITurnState CurrentTurnState => Turn != null ? Turn.CurrentState : null;

    // 라이브 뷰: 항상 BattleManager의 최신 상태를 보여줌
    public IReadOnlyList<UnitBase> EnemyUnits =>
        (BattleManager != null && BattleManager.enemyUnits != null)
        ? BattleManager.enemyUnits // List<EnemyUnit> -> IReadOnlyList<UnitBase> 로 암시 변환 OK
        : System.Array.Empty<UnitBase>();

    public IReadOnlyList<UnitBase> PlayerUnits =>
        BattleManager?.player != null
        ? new UnitBase[] { BattleManager.player }
        : System.Array.Empty<UnitBase>();

    // (선택) 단일 플레이어 바로 접근
    public PlayerUnit Player => BattleManager?.player;

    public BattleContext(BattleManager manager, int? randomSeed = null)
    {
        BattleManager = manager;
        Random = randomSeed.HasValue ? new System.Random(randomSeed.Value) : new System.Random();
    }

    /// AP 소모: 성공시 true, 실패시 false
    public bool SpendAP(int amount)
    {
        if (Turn == null) { Debug.LogWarning("[BattleContext] TurnManager.Instance 없음"); return false; }
        if (amount <= 0) return true;
        if (Turn.currentAP < amount)
        {
            Debug.Log("[BattleContext] AP 부족");
            return false;
        }
        Turn.UseAP(amount);
        return true;
    }

    public void GainAP(int amount)
    {
        if (Turn == null) { Debug.LogWarning("[BattleContext] TurnManager.Instance 없음"); return; }
        if (amount == 0) return;
        Turn.AddAP(amount);
    }

    public void SetAP(int value)
    {
        if (Turn == null) { Debug.LogWarning("[BattleContext] TurnManager.Instance 없음"); return; }
        Turn.SetAP(value);
    }

    public void NextTurn()
    {
        if (Turn == null) { Debug.LogWarning("[BattleContext] TurnManager.Instance 없음"); return; }
        Turn.IncrementTurnCount();
    }

    public void Log(string message) => Debug.Log($"[BattleContext] {message}");
}
