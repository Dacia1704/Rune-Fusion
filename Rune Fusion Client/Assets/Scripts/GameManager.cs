using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        public static GameManager Instance;
        [field: SerializeField] public GameManagerSO GameManagerSO { get; private set; }

        private TileManager tileManager;

        private void Awake()
        {
                Instance = this;
                tileManager = FindObjectOfType<TileManager>();
        }

        private void Start()
        {
                tileManager.GenerateGrid();
                
                SetUpTilePosition(tileManager.transform);
                
        }


        public void SetUpTilePosition(Transform tilesTransform)
        {
                Debug.Log(new Vector2(tilesTransform.position.x,
                        -1 * CameraManager.Instance.GetHeightCamera() / 2 + tileManager.GetHeightTiles()/2));
                tilesTransform.position = new Vector2(tilesTransform.position.x,
                        -1 * CameraManager.Instance.GetHeightCamera() / 2 + tileManager.GetHeightTiles()/2) ;
        }
}