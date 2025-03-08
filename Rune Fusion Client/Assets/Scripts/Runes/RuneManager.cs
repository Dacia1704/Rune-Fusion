using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    [field: SerializeField] public RuneManagerSO RuneManagerSO { get; private set; }
    public Rune[,] RunesMap {get;private set;} // list : start from bottom to top and from left to right;
    private float sizeTile;

    public void GenerateGrid(List<List<int>> typesList)
    { 
        GameManager.Instance.CheckIfMainThread();
        RunesMap = new Rune[GameManager.Instance.GameManagerSO.HeightRuneMap,GameManager.Instance.GameManagerSO.WidthRuneMap]; 
        sizeTile = CameraManager.Instance.GetWidthCamera()/ GameManager.Instance.GameManagerSO.WidthRuneMap;
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                GameObject tileObject = Instantiate(RuneManagerSO.SingleRuneList[typesList[y][x]],new Vector2(0,0),Quaternion.identity);
                Rune rune = tileObject.GetComponent<Rune>();
                rune.SetRune(y,x);
                rune.TextPos.text = $"{y} {x}";
                tileObject.transform.position = new Vector2( (x+1) * sizeTile - sizeTile/2 - GameManager.Instance.GameManagerSO.WidthRuneMap*sizeTile/2,
                    (y+1) * sizeTile - sizeTile/2 - GameManager.Instance.GameManagerSO.HeightRuneMap*sizeTile/2 );
                tileObject.transform.parent = transform;
                tileObject.name = $"tile {y}-{x}";
                RunesMap[y,x] = rune;
                tileObject.transform.localScale = new Vector3(sizeTile,sizeTile,sizeTile);
            }
        }
        
        
    }

    public float GetHeightRunes()
    {
        sizeTile = CameraManager.Instance.GetWidthCamera()/ GameManager.Instance.GameManagerSO.WidthRuneMap;
        return GameManager.Instance.GameManagerSO.HeightRuneMap * sizeTile;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">position is position of rune in unity world</param>
    /// <returns>index of rune in RunesMap</returns>
    public Tuple<int,int> GetRunesByPosition(Vector2 position)
    {
        for (int x = 0; x < GameManager.Instance.GameManagerSO.WidthRuneMap; x++)
        {
            for (int y = 0; y < GameManager.Instance.GameManagerSO.HeightRuneMap; y++)
            {
                if (RunesMap[y, x].transform.position.x - sizeTile / 2 < position.x &&
                    RunesMap[y, x].transform.position.x + sizeTile / 2 > position.x && 
                    RunesMap[y, x].transform.position.y - sizeTile / 2 < position.y &&
                    RunesMap[y, x].transform.position.y + sizeTile / 2 > position.y )
                {
                    return Tuple.Create(y, x);
                }
            }
        }
        return Tuple.Create(-1,-1);
    }

    
    public void SwapWithRightRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1,start.Item2+1));        
    }
    public void SwapWithLeftRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1,start.Item2-1));        
    }
    public void SwapWithTopRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1+1,start.Item2));        
    }
    public void SwapWithBottomRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1-1,start.Item2));        
    }

    
    /// <summary>
    /// Swap start rune with end rune
    /// </summary>
    /// <param name="start">start is index of rune start in RunesMap</param>
    /// <param name="end">end is index of end in RunesMap</param>
    public void SwapRunes(Tuple<int,int> start, Tuple<int,int> end)
    {
        Vector3 startPos = RunesMap[(int)start.Item1,(int)start.Item2].transform.position;
        Vector3 endPos = RunesMap[(int)end.Item1,(int)end.Item2].transform.position;
        RunesMap[(int)start.Item1, (int)start.Item2].GetComponent<SpriteRenderer>().sortingOrder = 1;
        RunesMap[(int)start.Item1, (int)start.Item2].transform.DOMove(endPos, GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic);
        RunesMap[(int)end.Item1,(int)end.Item2].transform.DOMove(startPos,  GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic);
        RunesMap[(int)start.Item1, (int)start.Item2].GetComponent<SpriteRenderer>().sortingOrder = 0;
        
        
        int startRow = RunesMap[(int)start.Item1, (int)start.Item2].Row;
        int startCol = RunesMap[(int)start.Item1, (int)start.Item2].Col;
        int endRow = RunesMap[(int)end.Item1, (int)end.Item2].Row;
        int endCol = RunesMap[(int)end.Item1, (int)end.Item2].Col;
        RunesMap[(int)end.Item1, (int)end.Item2].SetRune(startRow, startCol);
        RunesMap[(int)start.Item1, (int)start.Item2].SetRune(endRow, endCol);
        
        (RunesMap[(int)end.Item1,(int)end.Item2], RunesMap[(int)start.Item1,(int)start.Item2]) = (RunesMap[(int)start.Item1,(int)start.Item2], RunesMap[(int)end.Item1,(int)end.Item2]);
        
        MatchRune(Tuple.Create<int, int>(endRow,endCol));
        // MatchRune(Tuple.Create<int, int>(startRow,startCol));
    }

    public void MatchRune(Tuple<int,int> runeCheckIndex, bool isJustSwapping = false)
    {
        Debug.Log("Rune Check Index" + runeCheckIndex.ToString());
        RuneType runeTypeToCheck = RunesMap[runeCheckIndex.Item1,runeCheckIndex.Item2].Type;

        List<int> runeHorizontal = new List<int>();
        runeHorizontal.Add(2);
        ;
        int leftIndex = runeCheckIndex.Item2 -1;
        int rightIndex = runeCheckIndex.Item2 +1;
        while (leftIndex >= 0 || rightIndex < GameManager.Instance.GameManagerSO.WidthRuneMap)
        {
            // Debug.Log("left right " +leftIndex+" "+rightIndex);
            if (leftIndex >= 0)
            {
                if (RunesMap[runeCheckIndex.Item1, leftIndex].Type == runeTypeToCheck)
                {
                    runeHorizontal.Insert(0, 1);
                    leftIndex--;
                }
                else
                {
                    leftIndex = -1;
                }
            }

            if (rightIndex < GameManager.Instance.GameManagerSO.WidthRuneMap)
            {
                if (RunesMap[runeCheckIndex.Item1, rightIndex].Type == runeTypeToCheck)
                {
                    runeHorizontal.Add(1);
                    rightIndex++;
                }
                else
                {
                    rightIndex = GameManager.Instance.GameManagerSO.WidthRuneMap;
                }

            }
            // Debug.Log("list: " + string.Join(",", runeHorizontal));
        }

        List<int> runeVertical = new List<int>();
        runeVertical.Add(2);
        ;
        int bottomIndex = runeCheckIndex.Item1 -1;
        int topIndex = runeCheckIndex.Item1 +1;
        while (bottomIndex >= 0 || topIndex < GameManager.Instance.GameManagerSO.HeightRuneMap)
        {
            // Debug.Log("bottom top " +bottomIndex+" "+topIndex);
            if (bottomIndex >= 0)
            {
                if (RunesMap[bottomIndex,runeCheckIndex.Item2].Type == runeTypeToCheck)
                {
                    runeVertical.Insert(0, 1);
                    bottomIndex--;
                }
                else
                {
                    bottomIndex = -1;
                }
            }

            if (topIndex < GameManager.Instance.GameManagerSO.HeightRuneMap)
            {
                if (RunesMap[topIndex,runeCheckIndex.Item2].Type == runeTypeToCheck)
                {
                    runeVertical.Add(1);
                    topIndex++;
                }
                else
                {
                    topIndex = GameManager.Instance.GameManagerSO.HeightRuneMap;
                }

            }
            // Debug.Log("list: " + string.Join(",", runeVertical));
        }
        Debug.Log(runeHorizontal.Count +" "  + runeVertical.Count);

        if (runeHorizontal.Count >=5)
        {
            Debug.Log("5 ô liền");
            return;
        }
        if (runeHorizontal.Count >= 3 && runeVertical.Count >= 3)
        {
            Debug.Log("Bomb");
            return;
        }

        if (runeHorizontal.Count == 4 || runeVertical.Count == 4)
        {
            Debug.Log(" line");
            return;
        }

        if (runeHorizontal.Count == 3 || runeVertical.Count == 3)
        {
            Debug.Log(" nomal");
            return;
        }
        
    }
    


   
}
