using UnityEngine;
using MonsterSystem;

namespace MapSystem
{
    [CreateAssetMenu(fileName = "Map", menuName = "Map/Default")]
    public class Map : ScriptableObject
    {
        // Memebers 
        [SerializeField] private string _Name;
        [SerializeField] private AudioClip _BGM;
        [SerializeField] private Vector2[] _SpawnLocations;
        [SerializeField] private MonsterBase[] _SpawnableMobs;

        public string Name => _Name;
        public AudioClip BGM => _BGM;
        public Vector2[] SpawnLocations => _SpawnLocations;
        public MonsterBase[] SpwanableMobs => _SpawnableMobs;
    }
}