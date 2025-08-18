
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StorageSceneController : MonoBehaviour
{
    [Header("UI 요소 연결")]
    public Button doorButton;
    public Button trashCanButton;
    public Button leftArrowButton;
    public Button rightArrowButton;

    [Header("배경 설정")]
    public Sprite doorRoomSprite; // Door_Room.jpeg 이미지
    public Sprite storeRoomSprite; // Store_Room.jpeg 이미지
    
    [Header("아이템 획득 UI")]
    public GameObject itemAcquiredPanel; // 방금 만든 ItemAcquiredPanel

    private Image backgroundComponent; // 배경을 표시할 Image 컴포넌트

    void Start()
    {
        // --- 배경 컴포넌트 찾기 및 초기 설정 ---
        GameObject backgroundObject = GameObject.Find("Background");
        if (backgroundObject != null) backgroundComponent = backgroundObject.GetComponent<Image>();
        else Debug.LogError("Hierarchy에서 'Background'라는 이름의 UI Image 오브젝트를 찾을 수 없습니다.");

        // --- UI 요소들 이름으로 찾기 (에디터에서 연결 안됐을 경우) ---
        if (doorButton == null) doorButton = GameObject.Find("DoorButton")?.GetComponent<Button>();
        if (trashCanButton == null) trashCanButton = GameObject.Find("TrashCanButton")?.GetComponent<Button>();
        if (leftArrowButton == null) leftArrowButton = GameObject.Find("LeftArrowButton")?.GetComponent<Button>();
        if (rightArrowButton == null) rightArrowButton = GameObject.Find("RightArrowButton")?.GetComponent<Button>();
        if (itemAcquiredPanel == null) itemAcquiredPanel = GameObject.Find("ItemAcquiredPanel");

        // --- 버튼 기능 연결 ---
        if (doorButton != null) doorButton.onClick.AddListener(OnDoorButtonClick);
        if (trashCanButton != null) trashCanButton.onClick.AddListener(OnTrashCanButtonClick);
        if (leftArrowButton != null) leftArrowButton.onClick.AddListener(() => ChangeBackground(-1));
        if (rightArrowButton != null) rightArrowButton.onClick.AddListener(() => ChangeBackground(1));
        
        // 획득 패널의 닫기 기능 연결
        if (itemAcquiredPanel != null)
        {
            Button panelButton = itemAcquiredPanel.GetComponent<Button>();
            if (panelButton != null) panelButton.onClick.AddListener(CloseItemAcquiredPanel);
            itemAcquiredPanel.SetActive(false); // 시작할 땐 비활성화
        }

        // --- 버튼 투명화 ---
        if (doorButton != null) MakeButtonTransparent(doorButton);
        if (trashCanButton != null) MakeButtonTransparent(trashCanButton);

        // --- 초기 상태 설정 ---
        if(backgroundComponent != null) backgroundComponent.sprite = doorRoomSprite;
        UpdateVisibleButtons();
    }

    void ChangeBackground(int direction)
    {
        if (backgroundComponent.sprite == doorRoomSprite) backgroundComponent.sprite = storeRoomSprite;
        else backgroundComponent.sprite = doorRoomSprite;
        UpdateVisibleButtons();
    }

    void UpdateVisibleButtons()
    {
        if (backgroundComponent.sprite == doorRoomSprite)
        {
            doorButton?.gameObject.SetActive(true);
            trashCanButton?.gameObject.SetActive(false);
        }
        else if (backgroundComponent.sprite == storeRoomSprite)
        {
            doorButton?.gameObject.SetActive(false);
            trashCanButton?.gameObject.SetActive(true);
        }
    }

    void OnDoorButtonClick()
    {
        Debug.Log("문 버튼 클릭됨. SampleScene으로 이동합니다.");
        SceneManager.LoadScene("SampleScene");
    }

    // 휴지통 버튼 클릭 시 획득 패널 표시
    void OnTrashCanButtonClick()
    {
        Debug.Log("휴지통 버튼 클릭됨.");
        if (itemAcquiredPanel != null) itemAcquiredPanel.SetActive(true);
    }

    // 획득 패널 닫기
    void CloseItemAcquiredPanel()
    {
        if (itemAcquiredPanel != null) itemAcquiredPanel.SetActive(false);
    }

    void MakeButtonTransparent(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            var color = buttonImage.color;
            color.a = 0f;
            buttonImage.color = color;
        }
    }
}
