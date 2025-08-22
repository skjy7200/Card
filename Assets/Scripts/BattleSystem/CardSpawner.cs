using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class CardSpawner : MonoBehaviour
{
    public Transform deckPosition;
    public Transform handPanel;
    public GameObject cardPrefab;

    public CardData sampleCardData;

    private List<RectTransform> handCards = new List<RectTransform>();

    // 곡선 위치 계산 함수
    private Vector2 CalculateArcPosition(int index, int totalCount, float radius, float angleRange)
    {
        if (totalCount == 1) return Vector2.zero;

        float angleStep = angleRange / (totalCount - 1);
        float startAngle = -angleRange / 2f;
        float angle = startAngle + index * angleStep;

        float rad = angle * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad) * radius;
        float y = Mathf.Cos(rad) * radius - 300f;

        return new Vector2(x, y);
    }

    private float GetRotationZ(int index, int totalCount, float angleRange)
    {
        if (totalCount == 1) return 0;
        float angleStep = angleRange / (totalCount - 1);
        float startAngle = -angleRange / 2f;
        float angle = startAngle + index * angleStep;

        return -angle; 
    }

    private Vector2 CalculateCardPosition(int index, int totalCount, float cardWidth, float spacing)
    {
        float totalWidth = (cardWidth + spacing) * (totalCount - 1);
        float startX = -totalWidth / 2f;
        float x = startX + index * (cardWidth + spacing);
        return new Vector2(x, 0);
    }

    public void SpawnCard()
    {
        GameObject card = Instantiate(cardPrefab);

        card.transform.SetParent(handPanel, false);
        card.SetActive(true); // 먼저 활성화

        RectTransform rect = card.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, deckPosition.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handPanel as RectTransform,
            screenPos,
            Camera.main, // 또는 null
            out Vector2 startPos
        );
        rect.anchoredPosition = startPos;

        CanvasGroup canvasGroup = card.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = card.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;


        CardUI cardUI = card.GetComponent<CardUI>();

        
        if (cardUI != null)
        {
            // 가장 앞의 적을 기본 타겟으로 설정
            UnitBase defaultTarget = null;

            if (BattleManager.Instance.enemyUnits != null && BattleManager.Instance.enemyUnits.Count > 0)
            {
                defaultTarget = BattleManager.Instance.enemyUnits[0]; // 첫 번째 적
            }
            cardUI.SetCard(sampleCardData, defaultTarget);

            Button button = card.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => cardUI.OnClick());
            }
        }

         if (cardUI.backgroundImage != null)
        {
            RectTransform bgRect = cardUI.backgroundImage.rectTransform;
            bgRect.sizeDelta = new Vector2(120f, 180f); // 카드 크기와 동일하게 설정
            Debug.Log("backgroundImage rect sizeDelta set to: " + bgRect.sizeDelta);

        }

        float cardWidth = 120f;
        float spacing = 20f;

        //  현재 카드의 인덱스
        int newIndex = handCards.Count;
        Vector2 targetPos = CalculateCardPosition(newIndex, handCards.Count + 1, cardWidth, spacing);

        // 카드 리스트에 추가
        handCards.Add(rect);
       

        UpdateCardPositions();
    }

    public void RemoveCard(RectTransform card)
    {
        if (handCards.Contains(card))
        {
            handCards.Remove(card);
            UpdateCardPositions(); // 제거 후 위치 재정렬
        }
    }

    private void UpdateCardPositions()
    {
        float radius = 300f;
        float angleRange = 60f;

        for (int i = 0; i < handCards.Count; i++)
        {
            RectTransform rect = handCards[i];
            Vector2 targetPos = CalculateArcPosition(i, handCards.Count, radius, angleRange);
            float rotationZ = GetRotationZ(i, handCards.Count, angleRange);
     

            // DOTween 애니메이션
            Sequence seq = DOTween.Sequence();
            seq.Join(rect.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutCubic));
            seq.Join(rect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            seq.Join(rect.DORotateQuaternion(Quaternion.Euler(0, 0, rotationZ), 0.5f));
            seq.Join(rect.GetComponent<CanvasGroup>().DOFade(1f, 0.3f));
     

            seq.Play();
        }
    }
}
