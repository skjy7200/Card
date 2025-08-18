
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public UIManager uiManager; // UIManager를 연결할 변수

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치에서 2D 레이캐스트 발사
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            // 레이캐스트가 어떤 Collider에 닿았다면
            if (hit.collider != null)
            {
                // 닿은 오브젝트의 태그에 따라 다른 동작 수행
                if (hit.collider.CompareTag("Door"))
                {
                    uiManager.ShowDoorInteraction(true);
                }
                else if (hit.collider.CompareTag("TrashCan"))
                {
                    uiManager.ShowCardAcquisition(true);
                }
            }
        }
    }
}
