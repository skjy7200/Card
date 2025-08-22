using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private UnityEngine.Rendering.Volume postProcessVolume;
    [SerializeField] private float flashInTime = 0.05f;
    [SerializeField] private float holdTime = 0.05f;
    [SerializeField] private float fadeOutTime = 0.25f;

    private bool isFlashing;

    void Awake()
    {
        if (postProcessVolume != null)
            postProcessVolume.weight = 0f;
    }

    public void OnDamaged()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!isFlashing) StartCoroutine(FlashRoutine());
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        isFlashing = true;
        float t = 0f;
        while (t < flashInTime)
        {
            t += Time.deltaTime;
            postProcessVolume.weight = Mathf.Clamp01(t / flashInTime);
            yield return null;
        }
        postProcessVolume.weight = 1f;
        yield return new WaitForSeconds(holdTime);
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            postProcessVolume.weight = 1f - Mathf.Clamp01(t / fadeOutTime);
            yield return null;
        }
        postProcessVolume.weight = 0f;
        isFlashing = false;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DamageFlash))]
    public class DamageFlashEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DamageFlash script = (DamageFlash)target;
            if (GUILayout.Button("Test Damage Flash"))
            {
                script.OnDamaged();
            }
        }
    }
#endif
}

