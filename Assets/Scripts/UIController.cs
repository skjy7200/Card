using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // --- Unity 에디터에서 연결할 UI 요소들 ---
    [Header("1. UI 요소 연결")]
    public Image backgroundImageComponent;
    public Button moveButton;
    public Button deckButton;       // 덱 버튼 추가
    public Button settingsButton;   // 설정 버튼 추가
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    [Header("2. 배경 이미지 파일 연결")]
    public Sprite startSprite;      // 게임 시작 시 보일 기본 배경 (IMG_4008)
    public Sprite image4007Sprite;  // IMG_4007 이미지
    public Sprite image4009Sprite;  // IMG_4009 이미지

    void Start()
    {
        // 1. 초기 배경 설정
        if (backgroundImageComponent != null && startSprite != null)
        {
            backgroundImageComponent.sprite = startSprite;
            Debug.Log("초기 배경 이미지 할당 성공: " + startSprite.name);
        }
        else
        {
            Debug.LogWarning("초기 배경 이미지 할당 실패. backgroundImageComponent: " + (backgroundImageComponent != null) + ", startSprite: " + (startSprite != null));
        }

        // 2. 화살표 버튼 숨기기
        if (leftArrowButton != null) leftArrowButton.SetActive(false);
        if (rightArrowButton != null) rightArrowButton.SetActive(false);

        // 3. 각 버튼에 클릭 이벤트 연결
        if (moveButton != null) moveButton.onClick.AddListener(ToggleArrowButtons);
        if (deckButton != null) deckButton.onClick.AddListener(OnDeckButtonClick); // 덱 버튼 이벤트 연결
        if (settingsButton != null) settingsButton.onClick.AddListener(OnSettingsButtonClick); // 설정 버튼 이벤트 연결

        // Ensure Button component exists before adding listener
        if (leftArrowButton != null)
        {
            Button leftButton = leftArrowButton.GetComponent<Button>();
            if (leftButton != null)
            {
                leftButton.onClick.AddListener(() => ChangeBackground(-1));
            }
            else
            {
                Debug.LogError("LeftArrowButton에 Button 컴포넌트가 없습니다.");
            }
        }
        if (rightArrowButton != null)
        {
            Button rightButton = rightArrowButton.GetComponent<Button>();
            if (rightButton != null)
            {
                rightButton.onClick.AddListener(() => ChangeBackground(1));
            }
            else
            {
                Debug.LogError("RightArrowButton에 Button 컴포넌트가 없습니다.");
            }
        }
    }

    // "이동" 버튼 기능
    void ToggleArrowButtons()
    {
        bool areArrowsActive = leftArrowButton.activeSelf;
        leftArrowButton.SetActive(!areArrowsActive);
        rightArrowButton.SetActive(!areArrowsActive);
    }

    // "덱" 버튼 기능
    void OnDeckButtonClick()
    {
        Debug.Log("덱 버튼이 클릭되었습니다.");
        // 여기에 나중에 덱 관련 기능을 추가할 수 있습니다.
    }

    // "설정" 버튼 기능
    void OnSettingsButtonClick()
    {
        Debug.Log("설정 버튼이 클릭되었습니다.");
        // 여기에 나중에 설정 관련 기능을 추가할 수 있습니다.
    }

    // 배경 변경 기능
    void ChangeBackground(int direction)
    {
        Sprite currentSprite = backgroundImageComponent.sprite;
        Sprite nextSprite = null;

        if (currentSprite.name == startSprite.name) // 현재 IMG_4008
        {
            if (direction == -1) // 왼쪽 화살표
            {
                nextSprite = image4007Sprite;
            }
            else if (direction == 1) // 오른쪽 화살표
            {
                nextSprite = image4009Sprite;
            }
        }
        else if (currentSprite.name == image4007Sprite.name) // 현재 IMG_4007
        {
            if (direction == -1) // 왼쪽 화살표
            {
                nextSprite = image4009Sprite;
            }
            else if (direction == 1) // 오른쪽 화살표
            {
                nextSprite = startSprite;
            }
        }
        else if (currentSprite.name == image4009Sprite.name) // 현재 IMG_4009
        {
            if (direction == -1) // 왼쪽 화살표
            {
                nextSprite = startSprite;
            }
            else if (direction == 1) // 오른쪽 화살표
            {
                nextSprite = image4007Sprite;
            }
        }

        if (backgroundImageComponent != null && nextSprite != null)
        {
            backgroundImageComponent.sprite = nextSprite;
            Debug.Log("배경 변경됨: " + nextSprite.name); // 이 줄 추가
        }
        else
        {
            Debug.LogWarning("배경 변경 실패: backgroundImageComponent 또는 nextSprite가 null입니다."); // 이 줄 추가
        }
    }
}
