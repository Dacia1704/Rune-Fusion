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
    // [field: SerializeField] public RuneManagerSO RuneManagerSO { get; private set; }
    
    public RuneObjectPoolManager RuneObjectPoolManager { get; private set; }
    
    public Rune[,] RunesMap {get;private set;} // list : start from bottom to top and from left to right;
    private float sizeTile;

    public Action<Tuple<int, int>> OnRuneChangePosition; 
    
    
    private void Awake()
    {
        RuneObjectPoolManager = FindObjectOfType<RuneObjectPoolManager>();
    }

    private void Start()
    {
        OnRuneChangePosition += OnRuneChangePostionAction;
    }

    public void GenerateGrid(List<List<int>> typesList)
    { 
        GameManager.Instance.CheckIfMainThread();
        RunesMap = new Rune[GameManager.Instance.GameManagerSO.HeightRuneMap,GameManager.Instance.GameManagerSO.WidthRuneMap]; 
        sizeTile = CameraManager.Instance.GetWidthCamera()/ GameManager.Instance.GameManagerSO.WidthRuneMap;
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                GameObject tileObject = RuneObjectPoolManager.GetBasicRuneObjectFromIndex(typesList[y][x]);
                Rune rune = tileObject.GetComponent<Rune>();
                rune.Row = y;
                rune.Col = x;
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
    public void SwapRunes(Tuple<int, int> start, Tuple<int, int> end)
    {
        Vector3 startPos = RunesMap[start.Item1, start.Item2].transform.position;
        Vector3 endPos = RunesMap[end.Item1, end.Item2].transform.position;

        RunesMap[start.Item1, start.Item2].GetComponent<SpriteRenderer>().sortingOrder = 1;
        Sequence swapSequence = DOTween.Sequence();
        swapSequence
            .Join(RunesMap[start.Item1, start.Item2].transform.DOMove(endPos, GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic))
            .Join(RunesMap[end.Item1, end.Item2].transform.DOMove(startPos, GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic))
            .OnComplete(() =>
            {
                (RunesMap[end.Item1, end.Item2], RunesMap[start.Item1, start.Item2]) = (RunesMap[start.Item1, start.Item2], RunesMap[end.Item1, end.Item2]);
                RunesMap[start.Item1, start.Item2].GetComponent<SpriteRenderer>().sortingOrder = 0;
                int startRow = RunesMap[start.Item1, start.Item2].Row;
                int startCol = RunesMap[start.Item1, start.Item2].Col;
                int endRow = RunesMap[end.Item1, end.Item2].Row;
                int endCol = RunesMap[end.Item1, end.Item2].Col;

                RunesMap[end.Item1, end.Item2].SetRune(startRow, startCol);
                RunesMap[start.Item1, start.Item2].SetRune(endRow, endCol);

            });
    }


    public List<Tuple<int,int>> MatchRune(Tuple<int,int> runeCheckIndex, bool isJustSwapping = false)
    {
        Debug.Log("Rune Check Index" + runeCheckIndex.ToString());
        RuneType runeTypeToCheck = RunesMap[runeCheckIndex.Item1,runeCheckIndex.Item2].Type;

        List<Tuple<int,int>> runeHorizontal = new List<Tuple<int,int>>();
        runeHorizontal.Add(runeCheckIndex);
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
                    runeHorizontal.Insert(0, Tuple.Create(runeCheckIndex.Item1, leftIndex));
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
                    runeHorizontal.Add(Tuple.Create(runeCheckIndex.Item1, rightIndex));
                    rightIndex++;
                }
                else
                {
                    rightIndex = GameManager.Instance.GameManagerSO.WidthRuneMap;
                }

            }
            // Debug.Log("list: " + string.Join(",", runeHorizontal));
        }

        List<Tuple<int,int>> runeVertical = new List<Tuple<int,int>>();
        runeVertical.Add(runeCheckIndex);
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
                    runeVertical.Insert(0, Tuple.Create(bottomIndex,runeCheckIndex.Item2));
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
                    runeVertical.Add(Tuple.Create(topIndex,runeCheckIndex.Item2));
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
            return runeHorizontal;
        }

        if (runeVertical.Count >= 5)
        {
            Debug.Log("5 ô liền");
            return runeVertical;
        }
        if (runeHorizontal.Count >= 3 && runeVertical.Count >= 3)
        {
            Debug.Log("Bomb");
            List<Tuple<int,int>> ans = new List<Tuple<int,int>>();
            ans.AddRange(runeHorizontal);
            ans.AddRange(runeVertical);
            return ans;
        }

        if (runeHorizontal.Count == 4)
        {
            Debug.Log(" line");
            return runeHorizontal;
        }

        if (runeVertical.Count == 4)
        {
            Debug.Log(" line");
            return runeVertical;
        }

        if (runeHorizontal.Count == 3 )
        {
            Debug.Log(" nomal");
            return runeHorizontal;
        }

        if (runeVertical.Count == 3)
        {
            Debug.Log(" nomal");
            return runeVertical;
        }

        return new List<Tuple<int, int>>();
    }


    public void OnRuneChangePostionAction(Tuple<int, int> runeCheckIndex)
    {
        List<Tuple<int,int>> runeMatches = MatchRune(runeCheckIndex);
        foreach (Tuple<int, int> runeMatch in runeMatches)
        {
            RuneObjectPoolManager.ReleaseRune(RunesMap[runeMatch.Item1,runeMatch.Item2].gameObject);
        }
    }

   
}
