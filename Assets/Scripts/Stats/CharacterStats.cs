using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public float health = 10;
        public float minDistance = 50;
    }
}
