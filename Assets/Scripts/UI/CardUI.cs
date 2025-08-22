using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text cardNameText;
    public Image cardArtImage;
    public TMP_Text cardCostText;
    public Image backgroundImage;

    private CardData cardData;

    private UnitBase targetUnit;

    public float hoverScale = 1.2f;
    public float hoverDuration = 0.2f;
    public float hoverMoveY = 30f;

    private Vector3 originalScale;
    private Vector2 hoverBasePos;
    private Tween hoverTween;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void SetCard(CardData data, UnitBase autotarget = null)
    {
        if (data == null)
        {
            Debug.LogWarning("CardData is null");
            return;
        }

        if (backgroundImage != null)
        {
            backgroundImage.transform.SetAsFirstSibling();
            backgroundImage.color = Color.white;
        }

        cardData = data;
        targetUnit = autotarget ?? GetAutoTarget(data.targetType);

        Debug.Log($"[SetCard] 카드: {data.cardName}, 자동 타겟: {targetUnit?.unitName ?? "없음"}");

        cardNameText.text = data.cardName;
        cardArtImage.sprite = data.cardArt;
        cardCostText.text = data.cost.ToString();

    }

    public void OnClick()
    {

        if (cardData == null || BattleManager.Instance == null)
        {
            Debug.LogWarning("카드 데이터 또는 BattleManager가 설정되지 않았습니다.");
            return;
        }

        Debug.Log($"카드 클릭됨: {cardData.cardName}");

        int cost = cardData.cost;

        // AP 부족하면 사용 불가
        if (TurnManager.Instance.currentAP < cost)
        {
            Debug.Log("AP 부족으로 카드를 사용할 수 없습니다.");
            return;
        }

        List<UnitBase> targets = GetTargets(cardData.targetType);

        Debug.Log($"[CardUI.OnClick] {targets.Count}");

        if (targets.Count == 0)
        {
            Debug.Log("타겟이 없습니다.");
            return;
        }

        RectTransform rt = GetComponent<RectTransform>();

        // 1. 캔버스 기준 잡기
         Canvas canvas = GetComponentInParent<Canvas>();
         RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        /*
         // 2. 유닛의 월드 좌표 → 스크린 → UI 좌표 변환
         Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
         Vector2 uiPos;
         RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out uiPos);

         Debug.Log($"[좌표 디버그] screenPos={screenPos}, uiPos={uiPos}");*/


        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;


        // 4. 카드 복제본 생성
        GameObject clone = Instantiate(gameObject, canvasRect);
        RectTransform cloneRT = clone.GetComponent<RectTransform>();
        cloneRT.anchoredPosition = rt.anchoredPosition;
        cloneRT.localScale = rt.localScale;

        // 카드 리스트에서 제거
        var spawner = FindFirstObjectByType<CardSpawner>();
        if (spawner != null)
            spawner.RemoveCard(rt);
        // 5. 원본 제거 (레이아웃에서 영향 받지 않도록)
        Destroy(gameObject);

        if (targets.Count == 1)
        {

            UnitBase t = targets[0];

            if (t == null || t.currentHP <= 0)
            {
                Debug.Log("유효한 단일 타겟이 없습니다.");
                return;
            }

            Vector3 screenPos = cam != null
                    ? cam.WorldToScreenPoint(t.transform.position)
                    : Camera.main.WorldToScreenPoint(t.transform.position);

            Vector2 uiPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cam, out uiPos);

            Debug.Log($"[좌표 디버그] screenPos={screenPos}, uiPos={uiPos}");


            Debug.Log(" 이동 시작: " + uiPos);

            Sequence seq = DOTween.Sequence();

            // 이동 + 커짐
            seq.Append(cloneRT.DOAnchorPos(uiPos, 0.5f).SetEase(Ease.OutCubic));
            seq.Join(cloneRT.DOScale(1.2f, 0.5f));

            // 도착 후 작아짐
            seq.Append(cloneRT.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));

            seq.OnComplete(() =>
                {
                    Debug.Log(" 이동 완료");
                    Debug.Log($"anchoredPosition: {cloneRT.anchoredPosition}");
                    Debug.Log($"localPosition: {cloneRT.localPosition}");

                    BattleManager.Instance.UseCard(cardData, targets);

                    TurnManager.Instance.UseAP(cost);
                   
                    Destroy(clone);
                });
        }
    }

    private void ShowCardSlashEffect(RectTransform canvasRect, Vector2 uiPos)
    {
        GameObject effect = new GameObject("SlashEffect", typeof(RectTransform), typeof(CanvasRenderer), typeof(UnityEngine.UI.Image));
        effect.transform.SetParent(canvasRect, false);
        RectTransform rt = effect.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(120, 20);
        rt.anchoredPosition = uiPos;

        var img = effect.GetComponent<UnityEngine.UI.Image>();
        img.color = new Color(1f, 1f, 1f, 0.8f); // 흰색 슬래시

        // 45도 회전된 슬래시
        rt.rotation = Quaternion.Euler(0, 0, 45f);

        rt.DOScaleX(1.5f, 0.1f).SetEase(Ease.OutQuad);
        img.DOFade(0f, 0.3f).OnComplete(() => Destroy(effect));
    }

    private List<UnitBase> GetTargets(TargetType targetType)
    {
        List<UnitBase> list = new List<UnitBase>();

        switch (targetType)
        {
            case TargetType.Self:
                list.Add(BattleManager.Instance.player);
                break;

            case TargetType.Enemy:
                var firstEnemy = BattleManager.Instance.enemyUnits
                    .FirstOrDefault(e => e != null && e.currentHP > 0);
                if (firstEnemy != null) list.Add(firstEnemy);
                break;

            case TargetType.AllEnemies:
                list.AddRange(BattleManager.Instance.enemyUnits
                    .Where(e => e != null && e.currentHP > 0));
                break;

            case TargetType.Any:
                // 여기서 UI 선택창 열어서 플레이어가 고른 대상만 넣도록 가능
                break;

            case TargetType.None:
                // 효과만 발동하고 대상 없음
                break;
        }

        return list;
    }

    private UnitBase GetAutoTarget(TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.Self:
                return BattleManager.Instance.player;

            case TargetType.Enemy:
                var enemies = BattleManager.Instance.enemyUnits;
                if (enemies != null && enemies.Count > 0)
                {
                    // 첫 번째 살아있는 적 선택
                    foreach (var e in enemies)
                    {
                        if (e != null && e.currentHP > 0)
                            return e;
                    }
                }
                return null;

            case TargetType.AllEnemies:
            case TargetType.Any:
            case TargetType.None:
            default:
                return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) // 포인터가 카드 위에 호버링될 시
    {   
        hoverTween?.Kill(true);

        hoverBasePos = rectTransform.anchoredPosition; //  현재 위치를 기준점으로 저장

        hoverTween = DOTween.Sequence()
            .Join(rectTransform.DOScale(originalScale * hoverScale, hoverDuration).SetEase(Ease.OutBack))
            .Join(rectTransform.DOAnchorPos(hoverBasePos + new Vector2(0f, hoverMoveY), hoverDuration).SetEase(Ease.OutCubic));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTween?.Kill(true);

        hoverTween = DOTween.Sequence()
            .Join(rectTransform.DOScale(originalScale, hoverDuration).SetEase(Ease.OutBack))
            .Join(rectTransform.DOAnchorPos(hoverBasePos, hoverDuration).SetEase(Ease.OutCubic));
           
    }

}