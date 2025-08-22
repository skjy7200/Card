using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    private BattleContext _ctx;

    public Transform playerSpawnPoint;
    public GameObject playerPrefab;

    public GameObject enemyPrefab; // 프리팹은 유지

    //  적 유닛 스폰 정보
    public List<SpawnManager.EnemySpawnData> enemySpawns;

    public PlayerUnit player;
    public List<EnemyUnit> enemyUnits = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        //  플레이어 생성
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity).GetComponent<PlayerUnit>();


        var spawnManager = FindFirstObjectByType<SpawnManager>();
        if (spawnManager != null)
        {
            enemyUnits = spawnManager.enemyUnits;  // 참조만 함
            Debug.Log($"[BattleManager] 적 유닛 수: {enemyUnits.Count}");
            foreach (var e in enemyUnits)
            {
                Debug.Log($"[적 유닛] {e?.unitName} - HP: {e?.currentHP}");
            }
            // 플레이어/적 스폰 이후
            _ctx = new BattleContext(this /*, randomSeed: 원하는 값*/);
        }
        else
        {
            Debug.LogError("SpawnManager를 찾을 수 없습니다.");
        }

    }

    public bool UseCard(CardData card, List<UnitBase> targets)
    {
        Debug.Log("UseCard 실행 되었음1");
        if (card == null) { Debug.LogError("[UseCard] card is NULL"); return false; }
        var ctx = _ctx ??= new BattleContext(this);
        Debug.Log("UseCard 실행 되었음2");
        if (currentState != BattleState.PlayerTurn) return false; // 턴 보호
        Debug.Log("UseCard 실행 되었음3");
        if (player == null)
        {
            Debug.LogError("[UseCard] player is NULL. (프리팹에 PlayerUnit 붙었나? GetComponent 실패?)");
            return false;
        }
        if (player.currentHP <= 0)
        {
            Debug.LogWarning($"[UseCard] player dead or HP<=0 (HP={player.currentHP})");
            return false;
        }
        // AP 선체크/소모(컨텍스트 한 곳에서 처리)
        /*if (card.cost > 0 && !ctx.SpendAP(card.cost))
        {
            ctx.Log($"AP 부족: 필요 {card.cost}, 보유 {ctx.CurrentAP}");
            return false;
        }*/
        Debug.Log("UseCard 실행 되었음5");

        IList<UnitBase> targetList =
            (targets != null && targets.Count > 0) ? (IList<UnitBase>)targets
                                                   : System.Array.Empty<UnitBase>();

        if (card.effects == null)
        {
            Debug.LogWarning("[UseCard] card.effects == NULL");
            return false;
        }
        if (card.effects.Count == 0)
        {
            Debug.LogWarning("[UseCard] card.effects.Count == 0 (비어있음)");
            return false;
        }

        Debug.Log("UseCard 실행 되었음6");
        foreach (var effectSO in card.effects)
        {
            if (effectSO == null)
            {
                Debug.Log("effectSO가 null");
                continue;
            }
            Debug.Log("UseCard 실행 되었음7");
            effectSO.Execute(ctx, player, targetList);
        }
        return true;
    }

    private UnitBase GetFirstAliveEnemy()
    {
        foreach (var enemy in enemyUnits)
        {
            if (enemy != null && enemy.currentHP > 0)
                return enemy;
        }
        return null;
    }

    public enum BattleState { PlayerTurn, EnemyTurn }
    public BattleState currentState = BattleState.PlayerTurn;
}
