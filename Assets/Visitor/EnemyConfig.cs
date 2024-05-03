using UnityEngine;

namespace Assets.Visitor
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        // Must be property
        public int maxWeight;

        [Header("Elf")]
        public int elfScore;
        public int elfWeight;

        [Header("Human")]
        public int humanScore;
        public int humanWeight;

        [Header("Ork")]
        public int orkScore;
        public int orkWeight;

        [Header("Robot")]
        public int robotScore;
        public int robotWeight;
    }
}