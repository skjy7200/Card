using UnityEngine;
using System;

public class Health : MonoBehaviour 
{

    public UnitData data;

    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }

    public event Action<float, int, int> OnHPChanged; // normalized, current, max

    void Awake()
    {
        if (data != null) InitializeFrom(data);
        else // 데이터가 없으면 기본값
        {
            MaxHP = Mathf.Max(1, MaxHP == 0 ? 20 : MaxHP);
            CurrentHP = MaxHP;
            OnHPChanged?.Invoke((float)CurrentHP / MaxHP, CurrentHP, MaxHP);
        }
    }

    public void InitializeFrom(UnitData d)
    {
        data = d;
        MaxHP = Mathf.Max(1, d.maxHP);
        CurrentHP = MaxHP;
        OnHPChanged?.Invoke((float)CurrentHP / MaxHP, CurrentHP, MaxHP);
    }

    public void Damage(int amount)
    {
        SetHP(CurrentHP - amount);
    }

    public void Heal(int amount)
    {
        SetHP(CurrentHP + amount);
    }

    public void SetHP(int value)
    {
        CurrentHP = Mathf.Clamp(value, 0, MaxHP);
        OnHPChanged?.Invoke((float)CurrentHP / MaxHP, CurrentHP, MaxHP);
    }
}
