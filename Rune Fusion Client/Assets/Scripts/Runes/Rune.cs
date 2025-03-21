using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rune : MonoBehaviour,IPoolingObject
{
    [HideInInspector] public int Row;
    [HideInInspector] public int Col;
    [field: SerializeField] public RuneType Type{get;private set;}
    [field: SerializeField] public RuneForm Form{get;private set;}

    public bool IsProtected { get; private set; }
    private ProtectRuneLayer protectRuneLayer;
    public PoolingObjectPropsSO PoolingObjectPropsSO { get; set; }
    
    
    
    [HideInInspector]public TextMeshPro TextPos;
    
    private void Awake()
    {
        TextPos = GetComponentInChildren<TextMeshPro>();
        protectRuneLayer = GetComponentInChildren<ProtectRuneLayer>();
    }

    private void OnEnable()
    {
        BreakProtectLayer();
    }

    public void SetRune(int row, int col)
    {
        Row = row;
        Col = col;
        TextPos.text = Row + " " + Col;
    }

    public void CheckMatches(SwapType swapType = SwapType.None)
    {
        GameManager.Instance.RuneManager.OnRuneChangePosition?.Invoke(Tuple.Create<int, int>(Row,Col),swapType);
    }

    public void ProtectRune()
    {
        IsProtected = true;
        protectRuneLayer.Appear();
    }
    public void BreakProtectLayer()
    {
        // Debug.Log($"Break protected row{Row}, col{Col}");
        IsProtected = false;
        protectRuneLayer.Disappear();
    }
    
    
    
    
}

