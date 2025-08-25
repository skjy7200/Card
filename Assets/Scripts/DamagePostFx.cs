using UnityEngine;
using UnityEngine.Rendering;

public class DamagePostFx : MonoBehaviour
{
    [SerializeField] private Volume damageVolume; // Global Volume
    [SerializeField] private float flashInTime = 0.05f;   // 번쩍 올라가는 시간
    [SerializeField] private float holdTime = 0.05f;    // 잠깐 유지
    [SerializeField] private float fadeOutTime = 0.25f;   // 서서히 감소

    private bool isFlashing;

    void Awake()
    {
        if (damageVolume != null) damageVolume.weight = 0f;
    }

    public void OnDamaged()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!isFlashing) StartCoroutine(FlashRoutine());
        else RestartPeak(); // 연타 시 피크 연장하고 싶을 때 옵션
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        isFlashing = true;

        // 0 -> 1 (빠르게)
        float t = 0f;
        while (t < flashInTime)
        {
            t += Time.deltaTime;
            damageVolume.weight = Mathf.Clamp01(t / flashInTime);
            yield return null;
        }
        damageVolume.weight = 1f;

        // 유지
        yield return new WaitForSeconds(holdTime);

        // 1 -> 0 (천천히)
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            damageVolume.weight = 1f - Mathf.Clamp01(t / fadeOutTime);
            yield return null;
        }
        damageVolume.weight = 0f;

        isFlashing = false;
    }

    // 선택: 연타 시 피크를 갱신하고 싶다면 이렇게 확장 가능
    private void RestartPeak()
    {
        // 예: 그냥 weight를 즉시 1로 올려버리거나, 타이머를 리셋하도록 구성
        damageVolume.weight = 1f;
    }

   

}

