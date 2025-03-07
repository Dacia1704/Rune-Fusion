using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
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

    public Vector2 GetRunesByPosition(Vector2 position)
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
                    return new Vector2(y, x);
                }
            }
        }
        return new Vector2(-1,-1);
    }

    public void SwapWithRightRune(Vector2 start)
    {
        SwapRunes(new Vector2(start.x,start.y), new Vector2(start.x,start.y+1));        
    }
    public void SwapWithLeftRune(Vector2 start)
    {
        SwapRunes(new Vector2(start.x,start.y), new Vector2(start.x,start.y-1));        
    }
    public void SwapWithTopRune(Vector2 start)
    {
        SwapRunes(new Vector2(start.x,start.y), new Vector2(start.x+1,start.y));        
    }
    public void SwapWithBottomRune(Vector2 start)
    {
        SwapRunes(new Vector2(start.x,start.y), new Vector2(start.x-1,start.y));        
    }

    public void SwapRunes(Vector2 start, Vector2 end)
    {
        Vector3 startPos = RunesMap[(int)start.x,(int)start.y].transform.position;
        Vector3 endPos = RunesMap[(int)end.x,(int)end.y].transform.position;
        RunesMap[(int)start.x,(int)start.y].transform.position = endPos;
        RunesMap[(int)end.x,(int)end.y].transform.position = startPos;
        
        // Lưu lại thông tin hàng/cột trước khi đổi
        int startRow = RunesMap[(int)start.x, (int)start.y].Row;
        int startCol = RunesMap[(int)start.x, (int)start.y].Col;
        int endRow = RunesMap[(int)end.x, (int)end.y].Row;
        int endCol = RunesMap[(int)end.x, (int)end.y].Col;
        Debug.Log($"{startRow} {startCol} , {endRow} {endCol}");

        // Swap logic game (Gọi SetRune nếu nó có trách nhiệm cập nhật thông tin)
        RunesMap[(int)end.x, (int)end.y].SetRune(startRow, startCol);
        RunesMap[(int)start.x, (int)start.y].SetRune(endRow, endCol);
        
        (RunesMap[(int)end.x,(int)end.y], RunesMap[(int)start.x,(int)start.y]) = (RunesMap[(int)start.x,(int)start.y], RunesMap[(int)end.x,(int)end.y]);
        // (RunesMap[(int)end.x, (int)end.y].Col, RunesMap[(int)start.x, (int)start.y].Col) = (
        //     RunesMap[(int)start.x, (int)start.y].Col, RunesMap[(int)end.x, (int)end.y].Col);
        // (RunesMap[(int)end.x, (int)end.y].Row, RunesMap[(int)start.x, (int)start.y].Row) = (
        //     RunesMap[(int)start.x, (int)start.y].Row, RunesMap[(int)end.x, (int)end.y].Row);
        
        
    }
    
    
    


   
}
