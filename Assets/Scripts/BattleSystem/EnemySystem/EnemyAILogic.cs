using UnityEngine;

public abstract class EnemyAILogic : ScriptableObject
{
    public abstract void ExecuteAI(EnemyUnit self);
}
