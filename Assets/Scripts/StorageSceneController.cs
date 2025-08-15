
using UnityEngine;

// 이 스크립트는 storage 씬의 배경 설정 및 초기 상태를 관리합니다.
public class StorageSceneController : MonoBehaviour
{
    // 유니티 에디터에서 설정할 배경 이미지
    public Sprite backgroundSprite;

    void Start()
    {
        // --- 배경 자동 설정 ---
        // 'Background' 라는 이름의 새 게임 오브젝트를 만듭니다.
        GameObject backgroundObject = new GameObject("Background");

        // 만든 오브젝트에 이미지를 표시할 수 있는 SpriteRenderer 컴포넌트를 추가합니다.
        SpriteRenderer spriteRenderer = backgroundObject.AddComponent<SpriteRenderer>();

        // SpriteRenderer에 우리가 설정한 이미지를 넣어줍니다.
        spriteRenderer.sprite = backgroundSprite;

        // 다른 오브젝트들에 가려지지 않도록 정렬 순서를 맨 뒤로 보냅니다.
        spriteRenderer.sortingOrder = -10;

        // 카메라의 정중앙에 위치시킵니다.
        backgroundObject.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
    }
}
