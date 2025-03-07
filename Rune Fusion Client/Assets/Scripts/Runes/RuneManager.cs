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
    public GameObject[,] RunesMap {get;private set;} // list : start from bottom to top and from left to right;
    private float sizeTile;

    public void GenerateGrid(List<List<int>> typesList)
    { 
        GameManager.Instance.CheckIfMainThread();
        RunesMap = new GameObject[GameManager.Instance.GameManagerSO.HeightRuneMap,GameManager.Instance.GameManagerSO.WidthRuneMap]; 
        sizeTile = CameraManager.Instance.GetWidthCamera()/ GameManager.Instance.GameManagerSO.WidthRuneMap;
        for(int x=0;x<GameManager.Instance.GameManagerSO.WidthRuneMap;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.HeightRuneMap;y++)
            {
                GameObject tileObject = Instantiate(RuneManagerSO.SingleRuneList[typesList[y][x]],new Vector2(0,0),Quaternion.identity);
                Rune rune = tileObject.GetComponent<Rune>();
                rune.Col = x;
                rune.Row = y;
                rune.TextPos.text = $"{y} {x}";
                tileObject.transform.position = new Vector2( (x+1) * sizeTile - sizeTile/2 - GameManager.Instance.GameManagerSO.WidthRuneMap*sizeTile/2,
                    (y+1) * sizeTile - sizeTile/2 - GameManager.Instance.GameManagerSO.HeightRuneMap*sizeTile/2 );
                tileObject.transform.parent = transform;
                tileObject.name = $"tile {y}-{x}";
                RunesMap[y,x] = tileObject;
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
    
    
    


   
}
