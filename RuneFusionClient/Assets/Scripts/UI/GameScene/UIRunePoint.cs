using System;
using TMPro;

public class UIRunePoint: UIBase
{
    private TextMeshProUGUI pointText;

    private void Awake()
    {
        pointText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetPointText(int pointValue)
    {
        pointText.text = pointValue.ToString();
    }
}