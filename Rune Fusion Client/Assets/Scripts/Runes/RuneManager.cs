using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
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
    public RuneObjectPoolManager RuneObjectPoolManager { get; private set; }
    
    public Rune[,] RunesMap {get;private set;} // list : start from bottom to top and from left to right;
    public Vector3[,] RunesPositionMap {get;private set;} // list : start from bottom to top and from left to right;
    
    public Vector3[,] NewRunesPositionMap {get;private set;}
    private float sizeTile;

    [SerializeField] private bool canChangeTurn;
    private bool hasNewRunesToGen;
    
    [SerializeField] private int countRuneSequences;


    private void Awake()
    {
        RuneObjectPoolManager = FindFirstObjectByType<RuneObjectPoolManager>();
    }

    private void Start()
    {
        canChangeTurn = false;
        hasNewRunesToGen = true;
        countRuneSequences = -1;
    }
    #region Map Core
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
    public void GenerateRunesMap(List<List<string>> typesList)
    { 
        RunesMap = new Rune[GameManager.Instance.GameManagerSO.HeightRuneMap,GameManager.Instance.GameManagerSO.WidthRuneMap]; 
        RunesPositionMap = new Vector3[GameManager.Instance.GameManagerSO.HeightRuneMap,GameManager.Instance.GameManagerSO.WidthRuneMap]; 
        NewRunesPositionMap = new Vector3[GameManager.Instance.GameManagerSO.HeightRuneMap,GameManager.Instance.GameManagerSO.WidthRuneMap]; 
        sizeTile = CameraManager.Instance.GetWidthCamera()/ GameManager.Instance.GameManagerSO.WidthRuneMap;
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                int isProtected = (int)Char.GetNumericValue(typesList[y][x][0]);
                int type = (int)Char.GetNumericValue(typesList[y][x][1]);
                GameObject tileObject = RuneObjectPoolManager.GetBasicRuneObjectFromIndex(type);
                Rune rune = tileObject.GetComponent<Rune>();
                if (isProtected == 1)
                {
                    rune.ProtectRune();
                }
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
        StartCoroutine(CheckChangeTurn()); 
    }
    public void GenerateNewRune(List<List<string>> typesList)
    {
        bool hasNewRune = false;
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            bool newRune = false;
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                int type = int.Parse(typesList[y][x].Substring(1));
                if (type != -1)
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

        DOTween.CompleteAll();
        Sequence sequence = DOTween.Sequence();
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            int count = 0;
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                int type = int.Parse(typesList[y][x].Substring(1));
                if (type != -1)
                {
                    count++;
                }
            }
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                int isProtected = (int)Char.GetNumericValue(typesList[y][x][0]);
                int type = int.Parse(typesList[y][x].Substring(1));
                if (type != -1)
                {
                    GameObject tileObject = RuneObjectPoolManager.GetBasicRuneObjectFromIndex(type);
                    Rune rune = tileObject.GetComponent<Rune>();
                    if(isProtected==1) rune.ProtectRune();
                    tileObject.transform.position = new Vector2( NewRunesPositionMap[y,x].x, NewRunesPositionMap[y,x].y);
                    tileObject.transform.parent = transform;
                    tileObject.transform.localScale = new Vector3(sizeTile,sizeTile,sizeTile);

                    rune.SetRune(y + (GameManager.Instance.GameManagerSO.HeightRuneMap - count),x);
                    RunesMap[y + (GameManager.Instance.GameManagerSO.HeightRuneMap - count), x] = rune;
                    sequence.Join(tileObject.transform.DOMove(
                        RunesPositionMap[y + (GameManager.Instance.GameManagerSO.HeightRuneMap - count), x],
                        GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic));
                }
            }
        }
        
        sequence.OnComplete( () =>
        {
            MatchesRuneAfterGenNewRune();
        });
    }
    private void MatchesRuneAfterGenNewRune()
    {
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                if (RunesMap[y, x] != null && !RunesMap[y,x].IsChecked )
                {
                    RunesMap[y,x].CheckMatches();
                }
            }
        }
        StartCoroutine(UpdateRuneStateAfterCheck());
    }
    private IEnumerator UpdateRuneStateAfterCheck()
    {
        yield return new WaitUntil(() => countRuneSequences == 0);
        countRuneSequences = -1;
        UpdateRuneState();
    }
    public void UpdateRuneState()
    {
        DOTween.CompleteAll();
        Sequence sequence = DOTween.Sequence();
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            List<Tuple<int,int>> runeLeftList = new List<Tuple<int,int>>();
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                if (RunesMap[y, x] != null)
                {
                    runeLeftList.Add(Tuple.Create(y,x));
                }
            }
            if (runeLeftList.Count == GameManager.Instance.GameManagerSO.HeightRuneMap) continue;
            for (int y = 0; y < runeLeftList.Count; y++)
            {
                Tuple<int, int> start = runeLeftList[y];
                Tuple<int, int> end = Tuple.Create(y,x);
                if (start.Item1 == end.Item1) continue;
                RunesMap[end.Item1,end.Item2] = RunesMap[start.Item1,start.Item2];
                sequence.Join(RunesMap[end.Item1, end.Item2].transform.DOMove(
                    RunesPositionMap[end.Item1, end.Item2],
                    GameManager.Instance.GameManagerSO.DurationSwapRune).SetEase(Ease.InOutCubic));
                RunesMap[end.Item1,end.Item2].SetRune(end.Item1,end.Item2);
                RunesMap[start.Item1,start.Item2] = null;
            }
        }

        sequence.OnComplete(() =>
        {
            GenNewRuneAfterUpdateRuneState();
        });
    }
    private void GenNewRuneAfterUpdateRuneState()
    {
        if (GameManager.Instance.BattleManager.TurnManager.isPlayerTurn)
        {
            SocketManager.Instance.RequestNewRune(ConvertRunesMapToServerData());
        }
    }
    #endregion
    
    #region Swap Runes
    public void SwapWithRightRune(Tuple<int,int> start)
    {
        StartCoroutine(UpdateRuneStateAfterCheck());
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1,start.Item2+1),SwapType.Horizontal);    
        canChangeTurn = true;
        SocketManager.Instance.SwapRune(new Vector2(start.Item1, start.Item2), new Vector2(start.Item1,start.Item2+1));
    }
    public void SwapWithLeftRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1,start.Item2-1),SwapType.Horizontal);    
        canChangeTurn = true;
        SocketManager.Instance.SwapRune(new Vector2(start.Item1, start.Item2), new Vector2(start.Item1,start.Item2-1));
    }
    public void SwapWithTopRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1+1,start.Item2),SwapType.Vertical);    
        canChangeTurn = true;
        SocketManager.Instance.SwapRune(new Vector2(start.Item1, start.Item2), new Vector2(start.Item1+1,start.Item2));
    }
    public void SwapWithBottomRune(Tuple<int,int> start)
    {
        SwapRunes(Tuple.Create(start.Item1,start.Item2), Tuple.Create(start.Item1-1,start.Item2),SwapType.Vertical);      
        canChangeTurn = true;
        SocketManager.Instance.SwapRune(new Vector2(start.Item1, start.Item2), new Vector2(start.Item1-1,start.Item2));
    }
    
    /// <summary>
    /// Swap start rune with end rune
    /// </summary>
    /// <param name="start">start is index of rune start in RunesMap</param>
    /// <param name="end">end is index of end in RunesMap</param>
    /// <param name="swapType">Swap Type</param>
    public void SwapRunes(Tuple<int, int> start, Tuple<int, int> end,SwapType swapType)
    {
        StartCoroutine(UpdateRuneStateAfterCheck());
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
                RunesMap[start.Item1, start.Item2].CheckMatches(swapType);
                RunesMap[end.Item1, end.Item2].CheckMatches(swapType);
            });
    }
    #endregion
    
    #region Release rune
    public void OnRuneChangePostionAction(Tuple<int, int> runeCheckIndex, SwapType swapType = SwapType.None)
    {
        if(RunesMap[runeCheckIndex.Item1, runeCheckIndex.Item2] == null) return;
        Tuple<List<Tuple<int, int>>, RuneForm> check = MatchRune(runeCheckIndex, swapType);
        List<Tuple<int,int>> runeMatches = check.Item1;
        RuneType runeType = RuneType.PhysicAttack;
        if (runeMatches.Count > 0)
        {
            runeType = RunesMap[runeMatches[0].Item1, runeMatches[0].Item2].Type;
        }
        foreach (Tuple<int, int> runeMatch in runeMatches)
        {
            if (RunesMap[runeMatch.Item1, runeMatch.Item2] != null)
            {
                RunesMap[runeMatch.Item1, runeMatch.Item2].SetIsChecked(true);
                RunesMap[runeMatch.Item1, runeMatch.Item2].TextPos.text = "C";
            }
        }
        //check protect
        bool hasProtectRune = false;
        foreach (Tuple<int, int> runeMatch in runeMatches)
        {
            if (RunesMap[runeMatch.Item1, runeMatch.Item2] != null)
            {
                if (RunesMap[runeMatch.Item1, runeMatch.Item2].IsProtected)
                {
                    hasProtectRune = true;
                    break;
                }
            }
        }
        HashSet<Tuple<int,int>> runeReleaseSet = new HashSet<Tuple<int,int>>();
        foreach (Tuple<int, int> runeMatch in runeMatches)
        {
            if (RunesMap[runeMatch.Item1, runeMatch.Item2] != null)
            {
                if (RunesMap[runeMatch.Item1, runeMatch.Item2].Form == RuneForm.Horizontal)
                {
                    runeReleaseSet.UnionWith(ReleaseRowSequence(runeMatch.Item1, runeMatch.Item2));
                }else if (RunesMap[runeMatch.Item1, runeMatch.Item2].Form == RuneForm.Vertical)
                {
                    runeReleaseSet.UnionWith(ReleaseColSequence(runeMatch.Item2, runeMatch.Item1));
                } else if (RunesMap[runeMatch.Item1, runeMatch.Item2].Form == RuneForm.Explosive)
                {
                    runeReleaseSet.UnionWith(ReleaseExplosiveRuneSequence(Tuple.Create(runeMatch.Item1, runeMatch.Item2)));
                }
                
                if (!RunesMap[runeMatch.Item1, runeMatch.Item2].IsProtected)
                {
                    runeReleaseSet.Add(Tuple.Create(runeMatch.Item1, runeMatch.Item2));
                }
                else
                {
                    RunesMap[runeMatch.Item1, runeMatch.Item2].BreakProtectLayer();
                }
            }
        }
        if (countRuneSequences == -1)
        {
            countRuneSequences = 0;
        }
        if (runeReleaseSet.Count <= 0)
        {
            return;
        }
        Sequence sequence = DOTween.Sequence();
        countRuneSequences++;
        List<GameObject> runeReleaseList = new List<GameObject>();
        foreach (Tuple<int, int> index in runeReleaseSet)
        {
            GameObject runeObj = RunesMap[index.Item1, index.Item2].gameObject;
            runeReleaseList.Add(runeObj);
            runeObj.GetComponent<SpriteRenderer>().sortingOrder = 1;
            sequence.Join(runeObj.transform.DOScale(
                new Vector3(sizeTile * 1.3f, sizeTile * 1.3f, sizeTile * 1.3f),
                GameManager.Instance.GameManagerSO.DurationDeleteRune));
            RunesMap[index.Item1, index.Item2] = null;
            if (Equals(index, runeCheckIndex) && !hasProtectRune)
            {
                if (check.Item2 == RuneForm.Horizontal)
                {
                    GameObject newRune = RuneObjectPoolManager.GetHorizontalRuneObjectFromIndex((int)runeType);
                    Rune rune = newRune.GetComponent<Rune>();
                    rune.SetRune(index.Item1, index.Item2);
                    newRune.transform.position = RunesPositionMap[index.Item1, index.Item2];
                    newRune.transform.parent = transform;
                    RunesMap[index.Item1, index.Item2] = rune;
                    newRune.transform.localScale = new Vector3(sizeTile,sizeTile,sizeTile);
                }else if (check.Item2 == RuneForm.Vertical)
                {
                    GameObject newRune = RuneObjectPoolManager.GetVerticalRuneObjectFromIndex((int)runeType);
                    Rune rune = newRune.GetComponent<Rune>();
                    rune.SetRune(index.Item1, index.Item2);
                    newRune.transform.position = RunesPositionMap[index.Item1, index.Item2];
                    newRune.transform.parent = transform;
                    RunesMap[index.Item1, index.Item2] = rune;
                    newRune.transform.localScale = new Vector3(sizeTile,sizeTile,sizeTile);
                }else if (check.Item2 == RuneForm.Special)
                {
                    GameObject newRune = RuneObjectPoolManager.GetSpecialRuneObjectFromIndex();
                    Rune rune = newRune.GetComponent<Rune>();
                    rune.SetRune(index.Item1, index.Item2);
                    newRune.transform.position = RunesPositionMap[index.Item1, index.Item2];
                    newRune.transform.parent = transform;
                    RunesMap[index.Item1, index.Item2] = rune;
                    newRune.transform.localScale = new Vector3(sizeTile,sizeTile,sizeTile);
                } else if (check.Item2 == RuneForm.Explosive)
                {
                    GameObject newRune = RuneObjectPoolManager.GetExplosiveRuneObjectFromIndex((int)runeType);
                    Rune rune = newRune.GetComponent<Rune>();
                    rune.SetRune(index.Item1, index.Item2);
                    newRune.transform.position = RunesPositionMap[index.Item1, index.Item2];
                    newRune.transform.parent = transform; 
                    RunesMap[index.Item1, index.Item2] = rune;
                    newRune.transform.localScale = new Vector3(sizeTile,sizeTile,sizeTile);
                }
            }
        }
        sequence.OnComplete(() =>
        {
            foreach (GameObject runeObj in runeReleaseList)
            {
                runeObj.GetComponent<SpriteRenderer>().sortingOrder = 0;
                RuneObjectPoolManager.ReleaseRune(runeObj);
            }
            countRuneSequences--;
        });
    }
    
    public bool ReleaseUniqueRune()
    {
        List<Rune> specialRuneList = new List<Rune>();
        List<Rune> horizontalRuneList = new List<Rune>();
        List<Rune> verticalRuneList = new List<Rune>();
        List<Rune> explosiveRuneList = new List<Rune>();
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                if (RunesMap[y, x] != null)
                {
                    if(RunesMap[y,x].Form == RuneForm.Special) specialRuneList.Add(RunesMap[y, x]);
                    if(RunesMap[y,x].Form == RuneForm.Horizontal) horizontalRuneList.Add(RunesMap[y, x]);
                    if(RunesMap[y,x].Form == RuneForm.Vertical) verticalRuneList.Add(RunesMap[y, x]);
                    if(RunesMap[y,x].Form == RuneForm.Explosive) explosiveRuneList.Add(RunesMap[y, x]);
                }
            }
        }
        Debug.Log($"SpecialList:{specialRuneList.Count} HorizontalList:{horizontalRuneList.Count} VerticalList:{verticalRuneList.Count} ExplosiveList:{explosiveRuneList.Count}");
        if (specialRuneList.Count == 0 && horizontalRuneList.Count == 0 && verticalRuneList.Count == 0 && explosiveRuneList.Count==0) return false;
        
        DOTween.CompleteAll();
        Sequence sequence = DOTween.Sequence();
        HashSet<Tuple<int,int>> runeReleaseSet = new HashSet<Tuple<int,int>>();
        if (horizontalRuneList.Count > 0 || verticalRuneList.Count > 0)
        {
            foreach(Rune rune in horizontalRuneList)
            {
                runeReleaseSet.UnionWith(ReleaseRowSequence(rune.Row));
            }
            foreach (Rune rune in verticalRuneList)
            {
                runeReleaseSet.UnionWith(ReleaseColSequence(rune.Col));
            }
        }else if (explosiveRuneList.Count>0)
        {
            foreach (Rune rune in explosiveRuneList)
            {
                runeReleaseSet.UnionWith(ReleaseExplosiveRuneSequence(Tuple.Create(rune.Row,rune.Col)));
            }
        }else if (specialRuneList.Count > 0)
        {
            runeReleaseSet.UnionWith(ReleaseAllRuneSequence());
        }
        List<GameObject> runeReleaseList = new List<GameObject>();
        foreach (Tuple<int, int> index in runeReleaseSet)
        {
            if (RunesMap[index.Item1, index.Item2] != null)
            {
                GameObject runeObj = RunesMap[index.Item1, index.Item2].gameObject;
                runeReleaseList.Add(runeObj);
                runeObj.GetComponent<SpriteRenderer>().sortingOrder = 1;
                sequence.Join(runeObj.transform.DOScale(
                    new Vector3(sizeTile * 1.3f, sizeTile * 1.3f, sizeTile * 1.3f),
                    GameManager.Instance.GameManagerSO.DurationDeleteRune));
                RunesMap[index.Item1, index.Item2] = null;
            }
        }
        sequence.OnComplete(() =>
        {
            foreach (GameObject runeObj in runeReleaseList)
            {
                runeObj.GetComponent<SpriteRenderer>().sortingOrder = 0;
                RuneObjectPoolManager.ReleaseRune(runeObj);
            }
            UpdateRuneState();
        });
        return true;
    }
    
    public List<Tuple<int,int>> ReleaseRowSequence(int rowIndex, int exceptCol = -1)
    {
        List<Tuple<int,int>> runeReleaseList = new List<Tuple<int,int>>();
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            if (RunesMap[rowIndex, x] != null && x!=exceptCol)
            {
                if (!RunesMap[rowIndex, x].IsProtected)
                {
                    runeReleaseList.Add(Tuple.Create(rowIndex,x));
                }
                else
                {
                    RunesMap[rowIndex, x].BreakProtectLayer();
                }
            }
        }
        return runeReleaseList;
    }
    public List<Tuple<int,int>> ReleaseColSequence(int colIndex,int exceptRow=-1)
    {
        List<Tuple<int,int>> runeReleaseList = new List<Tuple<int,int>>();
        for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
        {
            if (RunesMap[y, colIndex] != null && y!=exceptRow)
            {
                if (!RunesMap[y, colIndex].IsProtected)
                {
                    runeReleaseList.Add(Tuple.Create(y, colIndex));
                }
                else
                {
                    RunesMap[y, colIndex].BreakProtectLayer();
                }
            }
        }

        return runeReleaseList;
    }
    public List<Tuple<int,int>> ReleaseExplosiveRuneSequence(Tuple<int, int> runeIndex)
    {
        List<Tuple<int, int>> explosiveList = new List<Tuple<int, int>>
        {
            Tuple.Create(runeIndex.Item1 - 1, runeIndex.Item2 - 1),
            Tuple.Create(runeIndex.Item1, runeIndex.Item2 - 1),
            Tuple.Create(runeIndex.Item1+1, runeIndex.Item2-1),
            Tuple.Create(runeIndex.Item1-1, runeIndex.Item2),
            Tuple.Create(runeIndex.Item1, runeIndex.Item2),
            Tuple.Create(runeIndex.Item1+1, runeIndex.Item2),
            Tuple.Create(runeIndex.Item1-1, runeIndex.Item2+1),
            Tuple.Create(runeIndex.Item1, runeIndex.Item2+1),
            Tuple.Create(runeIndex.Item1+1, runeIndex.Item2+1)
        };
        List<Tuple<int,int>> runeReleaseList = new List<Tuple<int,int>>();
        foreach (Tuple<int,int> index in explosiveList)
        {
            if (index.Item1 < GameManager.Instance.GameManagerSO.HeightRuneMap && index.Item1 >= 0 &&
                index.Item2 >= 0 && index.Item2 < GameManager.Instance.GameManagerSO.WidthRuneMap)
            {
                if (RunesMap[index.Item1, index.Item2] != null)
                {
                    if (!RunesMap[index.Item1, index.Item2].IsProtected)
                    {
                        runeReleaseList.Add(Tuple.Create(index.Item1, index.Item2));
                    }
                    else
                    {
                        RunesMap[index.Item1, index.Item2].BreakProtectLayer();
                    }
                }
            }
        }
        return runeReleaseList;
    }
    public List<Tuple<int,int>> ReleaseAllRuneSequence() {
    {
        List<Tuple<int,int>> runeReleaseList = new List<Tuple<int,int>>();
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                if (RunesMap[y, x] != null)
                {
                    if (!RunesMap[y, x].IsProtected)
                    {
                        runeReleaseList.Add(Tuple.Create(y,x));
                    }
                    else
                    {
                        RunesMap[y, x].BreakProtectLayer();
                    }
                }
            }
        }
        return runeReleaseList;
    }}

    #endregion
    
    #region Check condition
    public Tuple<List<Tuple<int,int>>,RuneForm> MatchRune(Tuple<int,int> runeCheckIndex,SwapType swapType)
    {
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
        
        if (runeHorizontal.Count >=5)
        {
            Debug.Log($"5 ô liền: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return Tuple.Create(runeHorizontal,RuneForm.Special);
        }

        if (runeVertical.Count >= 5)
        {
            Debug.Log($"5 ô liền: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return Tuple.Create(runeVertical,RuneForm.Special);
        }
        if (runeHorizontal.Count >= 3 && runeVertical.Count >= 3)
        {
            Debug.Log($"Bomb: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            List<Tuple<int,int>> ans = new List<Tuple<int,int>>();
            ans.AddRange(runeHorizontal);
            ans.AddRange(runeVertical);
            return Tuple.Create(ans,RuneForm.Explosive);
        }

        if (runeHorizontal.Count == 4)
        {
            Debug.Log($"Line: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            if (swapType == SwapType.Vertical)
            {
                return Tuple.Create(runeHorizontal,RuneForm.Vertical);
            }
            if(swapType == SwapType.Horizontal)
            {
                return Tuple.Create(runeHorizontal,RuneForm.Horizontal);
            }
            return Tuple.Create(runeHorizontal,RuneForm.Horizontal);
        }

        if (runeVertical.Count == 4)
        {
            Debug.Log($"Line: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            if (swapType == SwapType.Vertical)
            {
                return Tuple.Create(runeVertical,RuneForm.Vertical);
            }
            if(swapType == SwapType.Horizontal)
            {
                return Tuple.Create(runeVertical,RuneForm.Horizontal);
            }
            return Tuple.Create(runeVertical,RuneForm.Vertical);
        }

        if (runeHorizontal.Count == 3 )
        {
            Debug.Log($"Nomal: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return Tuple.Create(runeHorizontal,RuneForm.Base);
        }

        if (runeVertical.Count == 3)
        {
            Debug.Log($"Nomal: runeHorizontal {runeHorizontal.Count} + runeVertical {runeVertical.Count}");
            return Tuple.Create(runeVertical,RuneForm.Base);
        }
        return Tuple.Create(new List<Tuple<int, int>>(),RuneForm.Base);
    }
    private IEnumerator CheckChangeTurn()
    {
        while (true)
        {
            yield return new WaitUntil(() => !hasNewRunesToGen);
            hasNewRunesToGen = true;
            if (ReleaseUniqueRune()) continue;
            if (!GameManager.Instance.BattleManager.TurnManager.isPlayerTurn || !canChangeTurn) continue;
            canChangeTurn = false;
            GameManager.Instance.BattleManager.TurnManager.OnEndTurn?.Invoke();
        }
    }
    #endregion
    
    #region other
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
    #endregion
}
