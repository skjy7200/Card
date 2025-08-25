
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StorageSceneController : MonoBehaviour
{
    [Header("UI 요소 연결")]
    public Button doorButton;
    public Button leftArrowButton;
    public Button rightArrowButton;
    public Button moveButton;
    public Button deckButton;
    public Button settingsButton;

    [Header("덱 버튼 클릭 시 보여줄 카드")]
    public Sprite deckCardSprite;

    [Header("휴지통 버튼 설정")]
    public Button trashCanButton;
    public Sprite trashCanContentSprite;

    [Header("중앙에 크게 보여줄 카드 UI")]
    // 이 곳에 인스펙터에서 새로 만든 Image UI를 연결합니다.
    public Image largeCardDisplay; 

    [Header("배경 설정")]
    public Sprite doorRoomSprite;
    public Sprite storeRoomSprite;
    public Sprite storeRoomSprite2;

    [Header("상점 카드 오브젝트")]
    public GameObject store1Cards;
    public GameObject store2Cards;

    private Image backgroundComponent;

    void Start()
    {
        GameObject backgroundObject = GameObject.Find("Background");
        if (backgroundObject != null) backgroundComponent = backgroundObject.GetComponent<Image>();
        
        if (backgroundComponent == null)
        {
            Debug.LogError("'Background' 게임 오브젝트 또는 Image 컴포넌트를 찾을 수 없습니다.");
            return; 
        }

        // 버튼 이벤트 연결
        if (doorButton != null) doorButton.onClick.AddListener(OnDoorButtonClick);
        if (leftArrowButton != null) leftArrowButton.onClick.AddListener(() => ChangeBackground(-1));
        if (rightArrowButton != null) rightArrowButton.onClick.AddListener(() => ChangeBackground(1));
        if (moveButton != null) moveButton.onClick.AddListener(ToggleArrowButtons);
        if (deckButton != null)
        {
            deckButton.onClick.AddListener(OnDeckButtonClick);
        }
        else
        {
            Debug.LogError("Deck Button이 할당되지 않았습니다. 인스펙터에서 연결해주세요.");
        }
        if (settingsButton != null) settingsButton.onClick.AddListener(OnSettingsButtonClick);

        // 휴지통 버튼 이벤트 연결 (새로 추가)
        if (trashCanButton != null)
        {
            trashCanButton.onClick.AddListener(OnTrashCanButtonClick);
        }

        // 크게 보여줄 카드 이미지의 버튼 컴포넌트에 숨기기 기능 연결
        if (largeCardDisplay != null)
        {
            Button cardButton = largeCardDisplay.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.AddListener(HideLargeCard);
            }
            else
            {
                Debug.LogError("largeCardDisplay 오브젝트에 Button 컴포넌트가 없습니다. 클릭해서 숨기려면 Button 컴포넌트를 추가해주세요.");
            }
            // 시작할 때는 숨겨둡니다.
            largeCardDisplay.gameObject.SetActive(false);
        }

        // 초기 UI 상태 설정
        if (leftArrowButton != null) leftArrowButton.gameObject.SetActive(false);
        if (rightArrowButton != null) rightArrowButton.gameObject.SetActive(false);
        if (doorButton != null) MakeButtonTransparent(doorButton);

        backgroundComponent.sprite = doorRoomSprite;
        UpdateSceneState();
    }

    // 새롭게 추가된 카드 표시 기능
    public void ShowLargeCard(Sprite cardSpriteToShow)
    {
        if (largeCardDisplay == null)
        {
            Debug.LogError("largeCardDisplay가 할당되지 않았습니다. StorageSceneController 인스펙터에서 UI Image를 연결해주세요.");
            return;
        }
        if (cardSpriteToShow == null)
        {
            Debug.LogWarning("표시할 카드의 Sprite가 null입니다.");
            return;
        }

        largeCardDisplay.sprite = cardSpriteToShow;
        largeCardDisplay.gameObject.SetActive(true);
    }

    // 새롭게 추가된 카드 숨기기 기능
    public void HideLargeCard()
    {
        if (largeCardDisplay != null)
        {
            largeCardDisplay.gameObject.SetActive(false);
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
            UpdateSceneState();
        }
    }

    void UpdateSceneState()
    {
        if (backgroundComponent == null) return;
        Sprite currentSprite = backgroundComponent.sprite;
        doorButton?.gameObject.SetActive(currentSprite == doorRoomSprite);
        store1Cards?.SetActive(currentSprite == storeRoomSprite);
        store2Cards?.SetActive(currentSprite == storeRoomSprite2);
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
        if (deckCardSprite != null)
        {
            ShowLargeCard(deckCardSprite);
        }
        else
        {
            Debug.LogWarning("Deck Card Sprite가 할당되지 않았습니다.");
        }
    }

    public void OnSettingsButtonClick()
    {
        Debug.Log("설정 버튼이 클릭되었습니다.");
    }

    // 휴지통 버튼 클릭 시 호출될 새 함수
    public void OnTrashCanButtonClick()
    {
        if (trashCanContentSprite != null)
        {
            ShowLargeCard(trashCanContentSprite);
        }
        else
        {
            Debug.LogWarning("Trash Can Content Sprite가 할당되지 않았습니다.");
        }
    }
}
