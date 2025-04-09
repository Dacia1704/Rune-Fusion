using System;
using TMPro;
using UnityEngine;

public class UIEffect : UIBase, IPoolingObject
{
    public PoolingObjectPropsSO PoolingObjectPropsSO { get; set; }
    
    private TextMeshProUGUI durationText;

    private void Awake()
    {
        durationText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetDuration(int duration)
    {
        durationText.SetText(duration.ToString());
    }
}