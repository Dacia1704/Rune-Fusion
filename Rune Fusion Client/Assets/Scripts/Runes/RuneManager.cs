using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using DG.Tweening;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

/*Rune flow
 * Swap Rune
 * call CheckMatches để check ô combo
 * check match rune xóa ô, call hàm update state rune
 * đẩy các ô vào khoảng trống bên dưới (count số animation đẩy xuống dưới)
 * bao giờ animation đẩy xuống dưới hết thì gọi hàm gen new rune (có count số animation đẩy xuống)
 * bao giờ animation đẩy xuống dưới hết thì call hàm CheckMatches ở các ô vừa rơi xuống (bước 2)
 */


public class RuneManager : MonoBehaviour
{
    // [field: SerializeField] public RuneManagerSO RuneManagerSO { get; private set; }
    
    public RuneObjectPoolManager RuneObjectPoolManager { get; private set; }
    
    public Rune[,] RunesMap {get;private set;} // list : start from bottom to top and from left to right;
    public Vector3[,] RunesPositionMap {get;private set;} // list : start from bottom to top and from left to right;
    
    public Vector3[,] NewRunesPositionMap {get;private set;}
    private float sizeTile;

    public Action<Tuple<int, int>> OnRuneChangePosition;

    private int countCurrentUpdateRuneStateAnimation;
    private int countCurrentLoadNewRuneAnimation;
    private bool canChangeTurn;
    private bool hasNewRunesToGen;
    private void Awake()
    {
        RuneObjectPoolManager = FindFirstObjectByType<RuneObjectPoolManager>();
    }

    private void Start()
    {
        countCurrentUpdateRuneStateAnimation = -1;
        countCurrentLoadNewRuneAnimation = -1;
        canChangeTurn = false;
        hasNewRunesToGen = true;
        OnRuneChangePosition += OnRuneChangePostionAction;

        StartCoroutine(GenNewRuneAfterUpdateRuneStateCoroutine());
        StartCoroutine(MatchesRuneAfterGenNewRuneCoroutine());
        StartCoroutine(CheckChangeTurn()); 
    }

    public void GenerateRunesMap(List<List<int>> typesList)
    { 
        // GameManager.Instance.CheckIfMainThread();
        RunesMap = new Rune[GameManager.Instance.GameManagerSO.HeightRuneMap,GameManager.Instance.GameManagerSO.WidthRuneMap]; 
        RunesPositionMap = new Vector3[GameManager.Instance.GameManagerSO.HeightRuneMap,GameManager.Instance.GameManagerSO.WidthRuneMap]; 
        NewRunesPositionMap = new Vector3[GameManager.Instance.GameManagerSO.HeightRuneMap,GameManager.Instance.GameManagerSO.WidthRuneMap]; 
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
        UpdateRunesPostionMap();
    }

    public void GenerateNewRune(List<List<int>> typesList)
    {
        bool hasNewRune = false;
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            bool newRune = false;
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                if (typesList[y][x] != -1)
                {
                    newRune = true;
                    break;
                }
            }

            if (newRune)
            {
                hasNewRune = true;
                break;
            }
        }

        if (!hasNewRune)
        {
            hasNewRunesToGen = false;
            return;
        }
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            int count = 0;
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                if (typesList[y][x] != -1)
                {
                    count++;
                }
            }
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                if (typesList[y][x] != -1)
                {
                    GameObject tileObject = RuneObjectPoolManager.GetBasicRuneObjectFromIndex(typesList[y][x]);
                    Rune rune = tileObject.GetComponent<Rune>();
                    tileObject.transform.position = new Vector2( NewRunesPositionMap[y,x].x, NewRunesPositionMap[y,x].y);
                    tileObject.transform.parent = transform;
                    tileObject.transform.localScale = new Vector3(sizeTile,sizeTile,sizeTile);

                    rune.SetRune(y + (GameManager.Instance.GameManagerSO.HeightRuneMap - count),x);
                    RunesMap[y + (GameManager.Instance.GameManagerSO.HeightRuneMap - count), x] = rune;
                    if (countCurrentLoadNewRuneAnimation == -1)
                    {
                        countCurrentLoadNewRuneAnimation = 0;
                    }
                    countCurrentLoadNewRuneAnimation++;
                    tileObject.transform.DOMove(RunesPositionMap[y+ (GameManager.Instance.GameManagerSO.HeightRuneMap - count),x],
                        GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic).onComplete += (
                        () =>
                        {
                            countCurrentLoadNewRuneAnimation--;
                        });
                }
            }
        }
    }

    private IEnumerator MatchesRuneAfterGenNewRuneCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => countCurrentLoadNewRuneAnimation == 0);
            for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
            {
                for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
                {
                    if (RunesMap[y, x] != null)
                    {
                        RunesMap[y,x].CheckMatches();
                    }
                }
            }

            countCurrentLoadNewRuneAnimation = -1;
        }
    }
    public void UpdateRunesPostionMap()
    {
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                RunesPositionMap[y,x] = RunesMap[y, x].transform.position;
                NewRunesPositionMap[y,x] = new Vector3(RunesPositionMap[y,x].x,RunesPositionMap[y,x].y + GetHeightRunesMap(),0);
            }
        }
    }

    public void UpdateRuneState()
    {
        if (countCurrentUpdateRuneStateAnimation == -1)
        {
            countCurrentUpdateRuneStateAnimation= 0;
        }
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                if (RunesMap[y, x] == null)
                {
                    for (int z = y + 1; z < GameManager.Instance.GameManagerSO.HeightRuneMap; z++)
                    {
                        if (RunesMap[z, x] != null)
                        {
                            for (int t = z; t < GameManager.Instance.GameManagerSO.HeightRuneMap; t++)
                            {
                                if (RunesMap[t, x] != null)
                                {
                                    
                                    Tuple<int, int> start = Tuple.Create<int, int>(t,x);
                                    Tuple<int, int> end = Tuple.Create<int, int>(y+t-z,x);
                                    RunesMap[end.Item1,end.Item2] = RunesMap[start.Item1,start.Item2];
                                    countCurrentUpdateRuneStateAnimation++;
                                    RunesMap[end.Item1,end.Item2].transform.DOMove(RunesPositionMap[end.Item1,end.Item2],
                                        GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic).onComplete += (
                                        () =>
                                        {
                                            countCurrentUpdateRuneStateAnimation--;
                                        });
                                    RunesMap[end.Item1,end.Item2].SetRune(end.Item1,end.Item2);
                                    RunesMap[start.Item1,start.Item2] = null;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }

    private IEnumerator GenNewRuneAfterUpdateRuneStateCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => countCurrentUpdateRuneStateAnimation == 0);
            if (GameManager.Instance.BattleManager.TurnManager.isPlayerTurn)
            {
                SocketManager.Instance.RequestNewRune(ConvertRunesMapToServerData());
            }
            countCurrentUpdateRuneStateAnimation = -1;
        }
    }

    public float GetHeightRunesMap()
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
                if (RunesMap[y, x] != null)
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
        }
        return Tuple.Create(-1,-1);
    }
    public void SwapWithRightRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1,start.Item2+1));    
        canChangeTurn = true;
        SocketManager.Instance.SwapRune(new Vector2(start.Item1, start.Item2), new Vector2(start.Item1,start.Item2+1));
    }
    public void SwapWithLeftRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1,start.Item2-1));    
        canChangeTurn = true;
        SocketManager.Instance.SwapRune(new Vector2(start.Item1, start.Item2), new Vector2(start.Item1,start.Item2-1));
    }
    public void SwapWithTopRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1+1,start.Item2));    
        canChangeTurn = true;
        SocketManager.Instance.SwapRune(new Vector2(start.Item1, start.Item2), new Vector2(start.Item1+1,start.Item2));
    }
    public void SwapWithBottomRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1-1,start.Item2));      
        canChangeTurn = true;
        SocketManager.Instance.SwapRune(new Vector2(start.Item1, start.Item2), new Vector2(start.Item1-1,start.Item2));
    }
    
    /// <summary>
    /// Swap start rune with end rune
    /// </summary>
    /// <param name="start">start is index of rune start in RunesMap</param>
    /// <param name="end">end is index of end in RunesMap</param>
    public void SwapRunes(Tuple<int, int> start, Tuple<int, int> end)
    {
        if (RunesMap[start.Item1, start.Item2] == null || RunesMap[end.Item1, end.Item2] == null) return;
        Vector3 startPos = RunesPositionMap[start.Item1, start.Item2];
        Vector3 endPos = RunesPositionMap[end.Item1, end.Item2];
        RunesMap[start.Item1, start.Item2].GetComponent<SpriteRenderer>().sortingOrder = 1;
        Sequence swapSequence = DOTween.Sequence();
        swapSequence
            .Join(RunesMap[start.Item1, start.Item2].transform.DOMove(endPos, GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic))
            .Join(RunesMap[end.Item1, end.Item2].transform.DOMove(startPos, GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic))
            .OnComplete(() =>
            {
                RunesMap[start.Item1, start.Item2].GetComponent<SpriteRenderer>().sortingOrder = 0;
                (RunesMap[end.Item1, end.Item2], RunesMap[start.Item1, start.Item2]) = (RunesMap[start.Item1, start.Item2], RunesMap[end.Item1, end.Item2]);
                int startRow = RunesMap[start.Item1, start.Item2].Row;
                int startCol = RunesMap[start.Item1, start.Item2].Col;
                int endRow = RunesMap[end.Item1, end.Item2].Row;
                int endCol = RunesMap[end.Item1, end.Item2].Col;
                RunesMap[end.Item1, end.Item2].SetRune(startRow, startCol);
                RunesMap[start.Item1, start.Item2].SetRune(endRow, endCol);
                RunesMap[start.Item1, start.Item2].CheckMatches();
                RunesMap[end.Item1, end.Item2].CheckMatches();
            });
    }
    
    public List<Tuple<int,int>> MatchRune(Tuple<int,int> runeCheckIndex, bool isJustSwapping = false)
    {
        // Debug.Log("Rune Check Index" + runeCheckIndex.ToString());
        RuneType runeTypeToCheck = RunesMap[runeCheckIndex.Item1,runeCheckIndex.Item2].Type;

        List<Tuple<int,int>> runeHorizontal = new List<Tuple<int,int>>();
        runeHorizontal.Add(runeCheckIndex);
        
        int leftIndex = runeCheckIndex.Item2 -1;
        int rightIndex = runeCheckIndex.Item2 +1;
        while (leftIndex >= 0 || rightIndex < GameManager.Instance.GameManagerSO.WidthRuneMap)
        {
            if (leftIndex >= 0)
            {
                if (RunesMap[runeCheckIndex.Item1, leftIndex]!= null && RunesMap[runeCheckIndex.Item1, leftIndex].Type == runeTypeToCheck)
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
                if (RunesMap[runeCheckIndex.Item1, rightIndex]!= null &&RunesMap[runeCheckIndex.Item1, rightIndex].Type == runeTypeToCheck)
                {
                    runeHorizontal.Add(Tuple.Create(runeCheckIndex.Item1, rightIndex));
                    rightIndex++;
                }
                else
                {
                    rightIndex = GameManager.Instance.GameManagerSO.WidthRuneMap;
                }

            }
        }

        List<Tuple<int,int>> runeVertical = new List<Tuple<int,int>>();
        runeVertical.Add(runeCheckIndex);
        int bottomIndex = runeCheckIndex.Item1 -1;
        int topIndex = runeCheckIndex.Item1 +1;
        while (bottomIndex >= 0 || topIndex < GameManager.Instance.GameManagerSO.HeightRuneMap)
        {
            if (bottomIndex >= 0)
            {
                if (RunesMap[bottomIndex,runeCheckIndex.Item2]!= null && RunesMap[bottomIndex,runeCheckIndex.Item2].Type == runeTypeToCheck)
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
                if (RunesMap[topIndex,runeCheckIndex.Item2]!= null && RunesMap[topIndex,runeCheckIndex.Item2].Type == runeTypeToCheck)
                {
                    runeVertical.Add(Tuple.Create(topIndex,runeCheckIndex.Item2));
                    topIndex++;
                }
                else
                {
                    topIndex = GameManager.Instance.GameManagerSO.HeightRuneMap;
                }

            }
        }
        // Debug.Log($"runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
        
        if (runeHorizontal.Count >=5)
        {
            Debug.Log($"5 ô liền: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return runeHorizontal;
        }

        if (runeVertical.Count >= 5)
        {
            Debug.Log($"5 ô liền: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return runeVertical;
        }
        if (runeHorizontal.Count >= 3 && runeVertical.Count >= 3)
        {
            Debug.Log($"Bomb: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            List<Tuple<int,int>> ans = new List<Tuple<int,int>>();
            ans.AddRange(runeHorizontal);
            ans.AddRange(runeVertical);
            return ans;
        }

        if (runeHorizontal.Count == 4)
        {
            Debug.Log($"Line: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return runeHorizontal;
        }

        if (runeVertical.Count == 4)
        {
            Debug.Log($"Line: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return runeVertical;
        }

        if (runeHorizontal.Count == 3 )
        {
            Debug.Log($"Nomal: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return runeHorizontal;
        }

        if (runeVertical.Count == 3)
        {
            Debug.Log($"Nomal: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return runeVertical;
        }
        return new List<Tuple<int, int>>();
    }
    
    public void OnRuneChangePostionAction(Tuple<int, int> runeCheckIndex)
    {
        List<Tuple<int,int>> runeMatches = MatchRune(runeCheckIndex);
        foreach (Tuple<int, int> runeMatch in runeMatches)
        {
            if (RunesMap[runeMatch.Item1, runeMatch.Item2] != null)
            {
                GameObject runeObj = RunesMap[runeMatch.Item1, runeMatch.Item2].gameObject;
                runeObj.GetComponent<SpriteRenderer>().sortingOrder = 1;
                runeObj.transform.DOScale(new Vector3(sizeTile * 1.2f, sizeTile * 1.2f, sizeTile*1.2f ), 0.2f)
                    .OnComplete(() =>
                    {
                        runeObj.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        RuneObjectPoolManager.ReleaseRune(runeObj);
                    });
                
                RunesMap[runeMatch.Item1, runeMatch.Item2] = null;
            }
        }

        UpdateRuneState();
    }

    private IEnumerator CheckChangeTurn()
    {
        while (true)
        {
            yield return new WaitUntil(() => canChangeTurn && countCurrentLoadNewRuneAnimation == -1 &&
                                             countCurrentUpdateRuneStateAnimation == -1 && GameManager.Instance.BattleManager.TurnManager.isPlayerTurn && !hasNewRunesToGen);
            
            canChangeTurn = false;
            hasNewRunesToGen = true;
            // SocketManager.Instance.TurnRequest();
            GameManager.Instance.BattleManager.TurnManager.OnEndTurn?.Invoke();
        }
    }
    public string ConvertRunesMapToServerData()
    {
        int[][] data = new int[GameManager.Instance.GameManagerSO.HeightRuneMap][];
        for (int y = 0; y < GameManager.Instance.GameManagerSO.HeightRuneMap; y++)
        {
            data[y] = new int[GameManager.Instance.GameManagerSO.WidthRuneMap];
            for (int x = 0; x < GameManager.Instance.GameManagerSO.WidthRuneMap; x++)
            {
                if (RunesMap[y, x] != null)
                {
                    data[y][x] = (int)RunesMap[y, x].Type;
                }
                else
                {
                    data[y][x] = -1;
                }
            }
        }

        return JsonConvert.SerializeObject(data);
    }
}
