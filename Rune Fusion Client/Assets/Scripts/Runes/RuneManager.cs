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
    public GameObject[,] Tiles {get;private set;} // list : start from bottom to top and from left to right;
    private float sizeTile;

    public void GenerateGrid(List<List<int>> typesList)
    { 
        GameManager.Instance.CheckIfMainThread();
        Tiles = new GameObject[GameManager.Instance.GameManagerSO.Height+2,GameManager.Instance.GameManagerSO.Width+2]; 
        sizeTile = CameraManager.Instance.GetWidthCamera()/ GameManager.Instance.GameManagerSO.Width;
        for(int x=0;x<GameManager.Instance.GameManagerSO.Width;x++)
        {
            for(int y=0;y<GameManager.Instance.GameManagerSO.Height;y++)
            {
                GameObject tileObject = Instantiate(RuneManagerSO.SingleRuneList[typesList[y][x]],new Vector2(0,0),Quaternion.identity);
                Rune rune = tileObject.GetComponent<Rune>();
                rune.Col = x;
                rune.Row = y;
                tileObject.transform.position = new Vector2( (x+1) * sizeTile - sizeTile/2 - GameManager.Instance.GameManagerSO.Width*sizeTile/2,
                    (y+1) * sizeTile - sizeTile/2 - GameManager.Instance.GameManagerSO.Height*sizeTile/2 );
                tileObject.transform.parent = transform;
                tileObject.name = $"tile {y}-{x}";
                Tiles[y,x] = tileObject;
                tileObject.transform.localScale = new Vector3(sizeTile,sizeTile,sizeTile);
            }
        }
        
        
    }

    public float GetHeightTiles()
    {
        sizeTile = CameraManager.Instance.GetWidthCamera()/ GameManager.Instance.GameManagerSO.Width;
        return GameManager.Instance.GameManagerSO.Height * sizeTile;
    }
    
    
    
    


   
}
