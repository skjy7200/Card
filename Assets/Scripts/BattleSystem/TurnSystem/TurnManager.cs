using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    
    public int currentAP { get; private set; }
    private int turnCount = 0;

    private ITurnState currentState;
    public APDisplay apDisplay;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeState(new PlayerTurnState());
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(ITurnState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void IncrementTurnCount()
    {
        turnCount++;
    }

    public void SetAP(int value)
    {
        currentAP = value;
        apDisplay?.UpdateAP(currentAP);
    }

    public void AddAP(int value)
    {
        int newAP = currentAP + value;
        if (newAP > 6)
        {
            Debug.Log("최대 AP입니다");
            newAP = 6;
        }
        currentAP = newAP;
        apDisplay?.UpdateAP(currentAP);
    }

    public void UseAP(int amount)
    {
        currentAP = Mathf.Max(0, currentAP - amount);
        Debug.Log($"AP 사용됨: -{amount}, 남은 AP: {currentAP}");
        apDisplay?.UpdateAP(currentAP);
        // UI 연동 필요 시: APUIManager.Instance.UpdateAP(currentAP);
    }

    public ITurnState CurrentState => currentState; // 필요시 외부 접근용
    public int TurnCount => turnCount;


}
