
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickableObject : MonoBehaviour
{
    [Tooltip("클릭했을 때 중앙에 크게 보여줄 카드의 이미지입니다.")]
    public Sprite cardSprite;

    void Start()
    {
        // 씬에 있는 StorageSceneController를 찾습니다.
        StorageSceneController storageSceneController = FindObjectOfType<StorageSceneController>();
        if (storageSceneController == null)
        {
            Debug.LogError("씬에서 StorageSceneController를 찾을 수 없습니다. ClickableObject가 작동하지 않습니다.");
            return; 
        }

        // 이 게임 오브젝트의 Button 컴포넌트를 가져옵니다.
        Button clickableButton = GetComponent<Button>();
        
        // 버튼 클릭 시 storageSceneController의 ShowLargeCard 함수를 호출하도록 리스너를 추가합니다.
        clickableButton.onClick.AddListener(() => storageSceneController.ShowLargeCard(cardSprite));
        
        Debug.Log(gameObject.name + "에 카드 표시 기능이 성공적으로 연결되었습니다.");
    }
}
