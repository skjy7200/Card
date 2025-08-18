
using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트 사용을 위해 추가

public class UIManager : MonoBehaviour
{
    [Header("Interaction UI")]
    public GameObject doorInteractionPanel; // 문 상호작용 패널
    public GameObject cardAcquisitionPanel; // 카드 획득 패널
    public Image cardImage; // 카드를 보여줄 이미지
    public Sprite cardSprite; // 표시할 카드 스프라이트 (box.png)

    void Start()
    {
        // 게임 시작 시 모든 패널을 비활성화
        if(doorInteractionPanel != null) doorInteractionPanel.SetActive(false);
        if(cardAcquisitionPanel != null) cardAcquisitionPanel.SetActive(false);
    }

    // 문 상호작용 UI를 보여주거나 숨기는 함수
    public void ShowDoorInteraction(bool show)
    {
        if(doorInteractionPanel != null) doorInteractionPanel.SetActive(show);
    }

    // 카드 획득 UI를 보여주거나 숨기는 함수
    public void ShowCardAcquisition(bool show)
    {
        if(cardAcquisitionPanel != null) 
        {
            cardAcquisitionPanel.SetActive(show);
            if(show && cardImage != null && cardSprite != null)
            {
                cardImage.sprite = cardSprite;
            }
        }
    }

    // 카드 획득 버튼 클릭 시 호출될 함수
    public void OnAcquireButtonClick()
    {
        Debug.Log("카드를 획득했습니다!");
        ShowCardAcquisition(false); // 패널 닫기
    }

    // 문/쓰레기통 버튼 클릭 시 호출될 함수들
    public void OnDoorButtonClick()
    {
        ShowDoorInteraction(true);
    }

    public void OnTrashCanButtonClick()
    {
        ShowCardAcquisition(true);
    }

    // 기존 버튼 핸들러
    public void OnMoveButtonClick()
    {
        Debug.Log("--- 이동 버튼 클릭 성공! ---");
    }

    public void OnDeckButtonClick()
    {
        Debug.Log("--- 덱 버튼 클릭 성공! ---");
    }

    public void OnSettingsButtonClick()
    {
        Debug.Log("--- 설정 버튼 클릭 성공! ---");
    }
}
