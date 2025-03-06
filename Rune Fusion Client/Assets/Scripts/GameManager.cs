using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        public static GameManager Instance;
        [field: SerializeField] public GameManagerSO GameManagerSO { get; private set; }

        private RuneManager runeManager;

        private void Awake()
        {
                Instance = this;
                runeManager = FindObjectOfType<RuneManager>();
        }

        private void Start()
        {
                runeManager.GenerateGrid();
                
                SetUpTilePosition(runeManager.transform);
                
        }


        public void SetUpTilePosition(Transform tilesTransform)
        {
                Debug.Log(new Vector2(tilesTransform.position.x,
                        -1 * CameraManager.Instance.GetHeightCamera() / 2 + runeManager.GetHeightTiles()/2));
                tilesTransform.position = new Vector2(tilesTransform.position.x,
                        -1 * CameraManager.Instance.GetHeightCamera() / 2 + runeManager.GetHeightTiles()/2) ;
        }
}