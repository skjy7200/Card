using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Input System 사용을 위해 추가

public class StorageSceneController : MonoBehaviour
{
    [Header("UI 요소 연결")]
    public Button doorButton;
    public Button trashCanButton;
    public Button leftArrowButton;
    public Button rightArrowButton;
    public Button moveButton; // 이동 버튼
    public Button deckButton; // 덱 버튼
    public Button settingsButton; // 설정 버튼

    [Header("배경 설정")]
    public Sprite doorRoomSprite; // Door_Room.jpeg 이미지
    public Sprite storeRoomSprite; // Store_Room.jpeg 이미지
    public Sprite storeRoomSprite2; // Store_Room2.jpg 이미지

    [Header("Interaction UI (from UIManager)")]
    public GameObject doorInteractionPanel; // 문 상호작용 패널
    public GameObject cardAcquisitionPanel; // 카드 획득 패널
    public Image cardImage; // 카드를 보여줄 이미지
    public Sprite cardSprite; // 표시할 카드 스프라이트 (box.png)

    private Image backgroundComponent; // 배경을 표시할 Image 컴포넌트

    void Start()
    {
        // --- 배경 컴포넌트 찾기 및 초기 설정 ---
        GameObject backgroundObject = GameObject.Find("Background");
        if (backgroundObject != null) backgroundComponent = backgroundObject.GetComponent<Image>();
        
        // --- 디버깅: 흰 화면 문제 확인 ---
        if (backgroundComponent == null)
        {
            Debug.LogError("'Background' 게임 오브젝트 또는 Image 컴포넌트를 찾을 수 없습니다. 씬에 'Background'라는 이름의 UI Image가 있는지 확인해주세요.");
            return; 
        }
        if (doorRoomSprite == null)
        {
            Debug.LogError("'Door Room Sprite'가 인스펙터에 할당되지 않았습니다. 흰 화면이 표시될 수 있습니다.");
        }

        // --- UI 요소들 이름으로 찾기 (에디터에서 연결 안됐을 경우) ---
        if (doorButton == null) doorButton = GameObject.Find("DoorButton")?.GetComponent<Button>();
        if (trashCanButton == null) trashCanButton = GameObject.Find("TrashCanButton")?.GetComponent<Button>();
        if (leftArrowButton == null) leftArrowButton = GameObject.Find("LeftArrowButton")?.GetComponent<Button>();
        if (rightArrowButton == null) rightArrowButton = GameObject.Find("RightArrowButton")?.GetComponent<Button>();

        // --- 버튼 기능 연결 ---
        if (doorButton != null) doorButton.onClick.AddListener(OnDoorButtonClick);
        if (trashCanButton != null) trashCanButton.onClick.AddListener(OnTrashCanButtonClick);
        if (leftArrowButton != null) leftArrowButton.onClick.AddListener(() => ChangeBackground(-1));
        if (rightArrowButton != null) rightArrowButton.onClick.AddListener(() => ChangeBackground(1));
        if (moveButton != null) moveButton.onClick.AddListener(ToggleArrowButtons);
        if (deckButton != null) deckButton.onClick.AddListener(OnDeckButtonClick);
        if (settingsButton != null) settingsButton.onClick.AddListener(OnSettingsButtonClick);

        // --- 패널 및 화살표 초기화 ---
        if (cardAcquisitionPanel != null)
        {
            cardAcquisitionPanel.SetActive(false);
            // 패널의 자식 오브젝트에 있는 Button 컴포넌트를 찾아서 리스너 연결 (더 안정적인 방법)
            Button imageButton = cardAcquisitionPanel.GetComponentInChildren<Button>();
            if (imageButton != null)
            {
                imageButton.onClick.AddListener(OnAcquireButtonClick);
            }
            else
            {
                Debug.LogError("Card Acquisition Panel 또는 그 자식 오브젝트에서 Button 컴포넌트를 찾을 수 없습니다.");
            }
        }
        if (doorInteractionPanel != null) doorInteractionPanel.SetActive(false);
        if (leftArrowButton != null) leftArrowButton.gameObject.SetActive(false);
        if (rightArrowButton != null) rightArrowButton.gameObject.SetActive(false);

        // --- 버튼 투명화 ---
        if (doorButton != null) MakeButtonTransparent(doorButton);
        if (trashCanButton != null) MakeButtonTransparent(trashCanButton);

        // --- 초기 상태 설정 ---
        backgroundComponent.sprite = doorRoomSprite;
        UpdateVisibleButtons();
    }

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 감지 (New Input System)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 마우스 위치에서 2D 레이캐스트 발사
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            // 레이캐스트가 어떤 Collider에 닿았다면
            if (hit.collider != null)
            {
                // 닿은 오브젝트의 태그에 따라 다른 동작 수행
                if (hit.collider.CompareTag("Door"))
                {
                    OnDoorButtonClick();
                }
                else if (hit.collider.CompareTag("TrashCan"))
                {
                    OnTrashCanButtonClick();
                }
            }
        }
    }

    void ToggleArrowButtons()
    {
        if (leftArrowButton != null && rightArrowButton != null)
        {
            bool areArrowsActive = leftArrowButton.gameObject.activeSelf;
            leftArrowButton.gameObject.SetActive(!areArrowsActive);
            rightArrowButton.gameObject.SetActive(!areArrowsActive);
        }
    }

    void ChangeBackground(int direction)
    {
        if (backgroundComponent == null) return;
        Sprite currentSprite = backgroundComponent.sprite;
        Sprite nextSprite = null;

        if (currentSprite == doorRoomSprite) nextSprite = (direction > 0) ? storeRoomSprite : storeRoomSprite2;
        else if (currentSprite == storeRoomSprite) nextSprite = (direction > 0) ? storeRoomSprite2 : doorRoomSprite;
        else if (currentSprite == storeRoomSprite2) nextSprite = (direction > 0) ? doorRoomSprite : storeRoomSprite;
        else nextSprite = doorRoomSprite;

        if (nextSprite != null)
        {
            backgroundComponent.sprite = nextSprite;
            UpdateVisibleButtons();
        }
    }

    void UpdateVisibleButtons()
    {
        if (backgroundComponent == null) return;
        doorButton?.gameObject.SetActive(false);
        trashCanButton?.gameObject.SetActive(false);

        if (backgroundComponent.sprite == doorRoomSprite) doorButton?.gameObject.SetActive(true);
        else if (backgroundComponent.sprite == storeRoomSprite) trashCanButton?.gameObject.SetActive(true);
    }

    void OnDoorButtonClick()
    {
        Debug.Log("문 버튼 클릭됨. SampleScene으로 이동합니다.");
        SceneManager.LoadScene("SampleScene");
    }

    void MakeButtonTransparent(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null) { var color = buttonImage.color; color.a = 0f; buttonImage.color = color; }
    }

    public void OnDeckButtonClick()
    {
        Debug.Log("덱 버튼이 클릭되었습니다.");
        // 여기에 나중에 덱 관련 기능을 추가할 수 있습니다.
    }

    public void OnSettingsButtonClick()
    {
        Debug.Log("설정 버튼이 클릭되었습니다.");
        // 여기에 나중에 설정 관련 기능을 추가할 수 있습니다.
    }

    public void ShowDoorInteraction(bool show) { if(doorInteractionPanel != null) doorInteractionPanel.SetActive(show); }

    public void ShowCardAcquisition(bool show)
    {
        if(cardAcquisitionPanel != null) 
        {
            cardAcquisitionPanel.SetActive(show);
            if(show && cardImage != null && cardSprite != null) cardImage.sprite = cardSprite;
        }
    }

    public void OnAcquireButtonClick() { Debug.Log("OnAcquireButtonClick CALLED! Closing panel."); ShowCardAcquisition(false); }

    public void OnTrashCanButtonClick() { Debug.Log("휴지통 버튼 클릭됨."); ShowCardAcquisition(true); }
}
