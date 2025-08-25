using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class APDisplay : MonoBehaviour
{
    public GameObject apIconPrefab; // 아이콘 프리팹
    public Transform iconParent; // 아이콘이 들어갈 부모
    public Sprite apLabelSprite;

    private Image apLabelImage;
    private List<GameObject> apIcons = new();

    void Awake()
    {
        if (iconParent == null) iconParent = transform;

        // AP 라벨 준비(없으면 생성)
        var found = iconParent.Find("APLabel");
        if (found != null) apLabelImage = found.GetComponent<Image>();
        else
        {
            var go = new GameObject("APLabel", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(iconParent, false);
            apLabelImage = go.GetComponent<Image>();
            apLabelImage.raycastTarget = false;
        }

        if (apLabelSprite != null) apLabelImage.sprite = apLabelSprite;

        // 항상 맨 왼쪽(첫 번째) 유지
        apLabelImage.transform.SetSiblingIndex(0);
    }

    public void UpdateAP(int currentAP)
    {

        int currentCount = apIcons.Count;

        // 아이콘이 부족하면 추가
        if (currentCount < currentAP)
        {
            for (int i = currentCount; i < currentAP; i++)
            {
                GameObject icon = Instantiate(apIconPrefab, iconParent);
                apIcons.Add(icon);

                // (선택) 등장 애니메이션
                icon.transform.localScale = Vector3.zero;
                icon.transform.DOScale(Vector3.one, 0.2f);
            }
        }
        // 아이콘이 많으면 제거
        else if (currentCount > currentAP)
        {
            for (int i = currentCount - 1; i >= currentAP; i--)
            {
                GameObject iconToRemove = apIcons[i];
                apIcons.RemoveAt(i);

                // (선택) 제거 애니메이션 후 파괴
                iconToRemove.transform.DOScale(Vector3.zero, 0.2f)
                    .OnComplete(() => Destroy(iconToRemove));
            }
        }
    }
}
