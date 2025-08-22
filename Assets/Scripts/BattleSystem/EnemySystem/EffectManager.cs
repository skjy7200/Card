// EffectManager.cs
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }
    [SerializeField] private DamageFlash screenFlash;  // 전역 플래시(선택)
    [SerializeField] private AudioSource audioSource;  // 공용 SFX(선택)


    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }

        Debug.Log("EffectManager 인스턴스 생성");
        Instance = this;
        // DontDestroyOnLoad(gameObject); // 전역 유지하고 싶으면 주석 해제
    }

    /// <summary>타깃 유닛의 DamageFlash만 재생</summary>
    public void PlayTargetFlash(Transform target)
    {
        if (target == null)
        {
            Debug.Log("타겟이 null입니다.");
            return;
        }
        Debug.Log("타격 애니메이션 실행됨");
        var flash = target.GetComponentInChildren<DamageFlash>();
        flash?.OnDamaged();
    }
}
