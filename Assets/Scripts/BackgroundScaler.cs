
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    void Start()
    {
        // 현재 씬의 메인 카메라를 가져옵니다.
        Camera mainCamera = Camera.main;

        // SpriteRenderer 컴포넌트를 가져옵니다.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (mainCamera == null)
        {
            Debug.LogError("메인 카메라를 찾을 수 없습니다. 'MainCamera' 태그가 지정된 카메라가 씬에 있는지 확인하세요.");
            return;
        }

        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            Debug.LogError("SpriteRenderer 또는 Sprite가 Background 오브젝트에 없습니다. 배경 이미지를 할당했는지 확인하세요.");
            return;
        }

        // 카메라의 월드 단위 높이를 계산합니다. (Orthographic Size * 2)
        float cameraHeight = mainCamera.orthographicSize * 2f;
        // 카메라의 월드 단위 너비를 계산합니다. (높이 * 화면 비율)
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // 배경 스프라이트의 원본 크기를 월드 단위로 계산합니다.
        // (스프라이트의 픽셀 너비 / Pixels Per Unit)
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        // 배경이 카메라 뷰를 가득 채우도록 스케일 비율을 계산합니다.
        // 가로세로 비율이 맞지 않으면 이미지가 늘어나거나 줄어들 수 있습니다.
        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;

        // 계산된 스케일을 적용합니다.
        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        // 배경 오브젝트의 위치를 카메라 중앙으로 설정합니다.
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, transform.position.z);
    }
}
