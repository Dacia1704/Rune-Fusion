using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "GameManagerSO", order = 0)]
public class GameManagerSO : ScriptableObject
{
        [field: SerializeField] public int WidthRuneMap { get; private set; }
        [field: SerializeField] public int HeightRuneMap { get; private set; }
        [field: SerializeField] public float SwipeThreshold { get; private set; }
        [field: SerializeField] public float DurationSwapRune { get; private set; }
        [field: SerializeField] public float DurationDeleteRune { get; private set; }
        [field: SerializeField] public float TimePlayerTurn { get; private set; }
        [field: SerializeField] public float TimeMonsterTurn { get; private set; }
        
        public void SetWidthRuneMap(int widthRuneMap)
        {
                this.WidthRuneMap = widthRuneMap;
        }

        public void SetHeightRuneMap(int heightRuneMap)
        {
                this.HeightRuneMap = heightRuneMap;
        }
}