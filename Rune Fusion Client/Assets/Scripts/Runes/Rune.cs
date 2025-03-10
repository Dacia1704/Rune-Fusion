using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Rune : MonoBehaviour,IPoolingObject
{
    [HideInInspector] public int Row;
    [HideInInspector] public int Col;
    [field: SerializeField] public RuneType Type{get;private set;}
    [field: SerializeField] public RuneForm Form{get;private set;}
    
    public PoolingObjectPropsSO PoolingObjectPropsSO { get; set; }
    
    
    
    [HideInInspector]public TextMeshPro TextPos;
    
    private void Awake()
    {
        TextPos = GetComponentInChildren<TextMeshPro>();
    }

    public void SetRune(int row, int col)
    {
        Row = row;
        Col = col;
        TextPos.text = Row + " " + Col;
        // GameManager.Instance.RuneManager.OnRuneChangePosition?.Invoke(Tuple.Create<int, int>(Row,Col));
    }

    public void CheckMatches()
    {
        GameManager.Instance.RuneManager.OnRuneChangePosition?.Invoke(Tuple.Create<int, int>(Row,Col));
        Debug.Log(5);
    }
    
    
}

