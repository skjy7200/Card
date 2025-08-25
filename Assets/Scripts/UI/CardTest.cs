using UnityEngine;

public class CardTest : MonoBehaviour
{
    public CardUI cardUI;       // 카드 UI 프리팹
    public CardData cardData;   // 카드 데이터

    void Start()
    {
        cardUI.SetCard(cardData);
    }
}
