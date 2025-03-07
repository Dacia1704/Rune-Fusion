using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "GameManagerSO", order = 0)]
public class GameManagerSO : ScriptableObject
{
        [field: SerializeField] public int WidthRuneMap { get; private set; }
        [field: SerializeField] public int HeightRuneMap { get; private set; }
        [field: SerializeField] public float SwipeThreshold { get; private set; }
        [field: SerializeField] public float DurationSwapRune { get; private set; }
}