using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject TilePrefab;
    public GameObject[,] Tiles {get;private set;}
    private float sizeTile;
    

    public void GenerateGrid()
    { 
        Tiles = new GameObject[GameManager.Instance.GameManagerSO.Height+2,GameManager.Instance.GameManagerSO.Width+2]; 
        sizeTile = CameraManager.Instance.GetWidthCamera()/ GameManager.Instance.GameManagerSO.Width;
        for(int x=1;x<=GameManager.Instance.GameManagerSO.Width;x++)
        {
            for(int y=1;y<=GameManager.Instance.GameManagerSO.Height;y++)
            {
                GameObject tileObject = Instantiate(TilePrefab,new Vector2(0,0),Quaternion.identity);
                Tile tile = tileObject.GetComponent<Tile>();
                tile.Col = x;
                tile.Row = y;
                tileObject.transform.position = new Vector2( x * sizeTile - sizeTile/2 - GameManager.Instance.GameManagerSO.Width*sizeTile/2,y * sizeTile - sizeTile/2 - GameManager.Instance.GameManagerSO.Height*sizeTile/2 );
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
