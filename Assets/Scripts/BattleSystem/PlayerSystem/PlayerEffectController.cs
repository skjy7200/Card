using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// PlayerEffectController.cs
public class PlayerEffectController : MonoBehaviour
{
    public Camera playerCamera;
    public Image hitFlashImage;
    public AudioSource audioSource;
    public AudioClip hitSound;
    public RectTransform canvasRect;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    public void PlayHitEffect()
    {
        playerCamera.transform.DOShakePosition(0.2f, 0.3f, 20);
        Debug.Log("[디버그] playerCamera 자동 할당됨: " + playerCamera?.name);


        // 캔버스 자동 할당
        if (canvasRect == null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();

            if (canvas != null)
            {
                canvasRect = canvas.GetComponent<RectTransform>();
                Debug.Log("[디버그] canvasRect 자동 할당됨: " + canvasRect.name);
            }
            else
            {
                Debug.LogWarning("Canvas를 찾을 수 없습니다.");
            }
        }
    }
}
