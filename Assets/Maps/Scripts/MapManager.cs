using UnityEngine;
using CoreSystems;

namespace MapSystem
{
    public class MapManager : MonoBehaviour
    {
        // Memebers
        public static MapManager Instance {get; private set;}

        [SerializeField] private EdgeCollider2D _MapBounds;

        private void Awake ()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(this);
        }

        private void Start ()
        {
            CameraManager.Instance?.SetMapBounds(_MapBounds.bounds);
        }
    }
}