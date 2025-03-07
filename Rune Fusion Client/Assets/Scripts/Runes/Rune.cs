using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [HideInInspector] public int Row;
    [HideInInspector] public int Col;
    [HideInInspector] public RuneType Type;
    [HideInInspector] public RuneForm Form;
    
    public TextMeshPro TextPos;
    
    private void Awake()
    {
        TextPos = GetComponentInChildren<TextMeshPro>();
    }

    public void SetRune(int row, int col)
    {
        Row = row;
        Col = col;
        TextPos.text = Row + " " + Col;
    }
}

