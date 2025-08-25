using UnityEngine;

public class EnemyUnit : UnitBase
{
    public UnitData unitData { get; private set; }
    public EnemyAILogic aiLogic { get; private set; }

    public bool IsInitialized { get; private set; } = false;

    // 외부에서 호출 (예: BattleManager, SpawnManager)
    public void Initialize(UnitData data, EnemyAILogic ai)
    {
        unitData = data;
        aiLogic = ai;

        unitName = data.unitName;
        maxHP = data.maxHP;
        currentHP = maxHP;

        var sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null && data.unitSprite != null)
        {
            sr.sprite = data.unitSprite;
            Debug.Log("적 스프라이트 연결 완료");
        }

        Debug.Log($"{unitName}(적 유닛) 초기화 완료");
        IsInitialized = true;
    }

    private void Awake()
    {
        // 경고만 출력, 초기화는 Initialize에서 한다
        if (!IsInitialized)
        {
            Debug.LogWarning("EnemyUnit이 Initialize 되지 않았습니다. 생성 직후 Initialize를 호출해야 합니다.");
        }
    }

    public void ExecuteAI()
    {
        if (aiLogic != null)
        {
            aiLogic.ExecuteAI(this);
        }
        else
        {
            Debug.LogWarning($"{unitName}의 aiLogic이 설정되지 않았습니다.");
        }
    }
}
