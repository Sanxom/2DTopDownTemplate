using Unity.Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _mapBoundary;
    [SerializeField] private Direction _direction;
    [SerializeField] private float _amountToMove;

    private CinemachineConfiner2D _confiner;

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    private void Awake()
    {
        _confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
        {
            _confiner.BoundingShape2D = _mapBoundary;
            UpdatePlayerPosition(player.gameObject);

            MapControllerManual.Instance?.HighlightArea(_mapBoundary.name);
            MapControllerDynamic.Instance?.UpdateCurrentArea(_mapBoundary.name);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (_direction)
        {
            case Direction.Up:
                newPos.y += _amountToMove;
                break;
            case Direction.Down:
                newPos.y -= _amountToMove;
                break;
            case Direction.Left:
                newPos.x += _amountToMove;
                break;
            case Direction.Right:
                newPos.x -= _amountToMove;
                break;
            default:
                break;
        }

        player.transform.position = newPos;
    }
}