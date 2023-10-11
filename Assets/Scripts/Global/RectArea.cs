using UnityEngine;

public class RectArea : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 _OffSet;
    [SerializeField] private Vector2 _Size;
    [SerializeField] private bool _Draw;

    [Header("Run Time")]
    [SerializeField] private Vector2 _Center;
    [SerializeField] private Vector2 _PointA;
    [SerializeField] private Vector2 _PointB;

    public Vector2 Center => _Center;
    public Vector2 PointA => _PointA;
    public Vector2 PointB => _PointB;         

    private void OnDrawGizmos()
    {
        if (_Draw) Gizmos.DrawWireCube(_Center, _Size);
    }

    private void Awake ()
    {
        transform.position = new Vector3(transform.position.x + _OffSet.x, transform.position.y + _OffSet.y, transform.position.z); 
    }

    private void FixedUpdate ()
    {
        _Center = transform.position;

        _PointA = _Center;
        _PointA.x -= _Size.x / 2;
        _PointA.y -= _Size.y / 2;
        
        _PointB = _PointA;
        _PointB.x += _Size.x;
        _PointB.y += _Size.y;
    }

    public void Init (Vector2 offset, Vector2 size)
    {
        _OffSet = offset;
        _Size = size;

        transform.position = new Vector3(transform.position.x + _OffSet.x, transform.position.y + _OffSet.y, transform.position.z);
    }
}