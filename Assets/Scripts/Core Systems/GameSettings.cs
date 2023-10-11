using UnityEngine;
using UnityEngine.InputSystem;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance {get; private set;}
    [SerializeField] private InputActions _KeyInput;
    [SerializeField] private LayerMask _InteractableLayers;
    [SerializeField] private LayerMask _UILayer;
    [SerializeField] private LayerMask _GroundLayer;
    [SerializeField] private LayerMask _RopeLayer;
    [SerializeField] private LayerMask _CharacterLayer;
    [SerializeField] private LayerMask _EnemyLayer;
    [SerializeField] private PhysicsMaterial2D _FrictionIdle;
    [SerializeField] private PhysicsMaterial2D _FrictionMove;
    [SerializeField] private float _SkillQueueTime;
    [SerializeField] private float _CameraDampening;
    [SerializeField] private int _WanderValue;
    [SerializeField] private int _StillValue;
    [SerializeField] private float _RespawnTime;
    [SerializeField] private Vector2 _SpawnLocation;

    private string _SavePath;

    public static InputActions KeyInput => Instance._KeyInput;
    public static LayerMask InteractableLayers => Instance._InteractableLayers;
    public static LayerMask UILayer => Instance._UILayer;
    public static LayerMask GroundLayer => Instance._GroundLayer;
    public static LayerMask RopeLayer => Instance._RopeLayer;
    public static LayerMask CharacterLayer => Instance._CharacterLayer;
    public static LayerMask EnemyLayer => Instance._EnemyLayer;
    public static PhysicsMaterial2D FrictionIdle => Instance._FrictionIdle;
    public static PhysicsMaterial2D FrictionMove => Instance._FrictionMove;
    public static float SkillQueueTime => Instance._SkillQueueTime;
    public static int WanderValue => Instance._WanderValue;
    public static int StillValue => Instance._StillValue;
    public static float RespawnTime => Instance._RespawnTime;
    public static Vector2 SpawnLocation => Instance._SpawnLocation;
    public static string SavePath => Instance._SavePath;


    // Camera Settings
    public static float CameraMinSize = 3.5f;
    public static float CameraMaxSize = 7f;
    public static float CameraDampening = 0.25f;
    public static Vector2 CameraOffSet = new Vector2(0f, 1.5f);

    public static int MesoStartAmount = 1000;
    public static int InventoryStartSize = 2;
    

    private void Awake ()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);

        _KeyInput = new InputActions();
        _KeyInput.Enable();
    }

    public void SetSavePath (string newSavePath) => _SavePath = newSavePath;
}