using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIFloatingText : MonoBehaviour, IPoolingObject
{
    private FloatingTextObjectPooling objectPooling;
    public PoolingObjectPropsSO PoolingObjectPropsSO { get; set; }
    private float floatDistance = 5f;      // Khoảng cách bay lên
    private float duration = 1f;            // Thời gian hiệu ứng
    
    private TextMeshProUGUI tmpText;       // Nếu dùng TMP
    private CanvasGroup canvasGroup;       // Để fade out

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void Show(FloatingTextObjectPooling pool, string damage, Color textColor)
    {
        objectPooling = pool;
        tmpText.text = damage;
        tmpText.color = textColor;
        canvasGroup.alpha = 0f; 
        RectTransform rect = GetComponent<RectTransform>();

        // Reset scale và position nếu cần
        rect.localScale = Vector3.one * 0.5f;

        Vector3 startPos = rect.anchoredPosition;
        Vector3 floatUpPos = startPos + new Vector3(0, floatDistance, 0);

        Sequence seq = DOTween.Sequence();

        // Giai đoạn 1: Fade in nhanh + phóng to (scale up)
        seq.Append(canvasGroup.DOFade(1f, 0.1f));
        seq.Join(rect.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack)); // phóng to nhẹ và bounce tại chỗ

        // Giai đoạn 2: Scale nhỏ lại 1 chút để tạo hiệu ứng bounce
        // seq.Append(rect.DOScale(1f, 0.1f).SetEase(Ease.InBack));

        // Giai đoạn 3: Bay lên + fade out
        seq.Append(rect.DOAnchorPos(floatUpPos, duration).SetEase(Ease.OutCubic));
        seq.Join(canvasGroup.DOFade(0f, duration));

        // Giai đoạn cuối: thu hồi object về pool
        seq.OnComplete(() => pool.ReleaseObject(this.gameObject));
    }


    
}
