using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Gameplay/Level", order = 1)]

public class LevelScriptable : ScriptableObject
{
    [field: SerializeField] public int MaxFloors { get; private set; }
    [field: SerializeField] public float CraneAngle { get; private set; }
    [field: SerializeField] public int CraneSpeed { get; private set; }
    [field: SerializeField] public int MaxScorePerLand { get; private set; }
}
