using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform _waypointParent;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _waitTimeDuration = 2f;
    [SerializeField] private bool _loopWaypoints = true;

    private const string IS_WALKING_ANIM_NAME = "isWalking";
    private const string INPUT_X_ANIM_NAME = "inputX";
    private const string INPUT_Y_ANIM_NAME = "inputY";
    private const string LAST_DIRECTION_X_ANIM_NAME = "lastInputX";
    private const string LAST_DIRECTION_Y_ANIM_NAME = "lastInputY";

    private Transform[] _waypointArray;
    private WaitForSeconds _waitTime;

    private Vector2 _direction;
    private float _lastDirectionX;
    private float _lastDirectionY;
    private int _currentWaypointIndex;
    private bool _isWaiting;

    private void Awake()
    {
        _waitTime = new(_waitTimeDuration);
        _waypointArray = new Transform[_waypointParent.childCount];

        for (int i = 0; i < _waypointParent.childCount; i++)
        {
            _waypointArray[i] = _waypointParent.GetChild(i);
        }
    }

    private void Update()
    {
        if (PauseManager.IsGamePaused || _isWaiting)
        {
            _anim.SetBool(IS_WALKING_ANIM_NAME, false);
            _anim.SetFloat(LAST_DIRECTION_X_ANIM_NAME, _lastDirectionX);
            _anim.SetFloat(LAST_DIRECTION_Y_ANIM_NAME, _lastDirectionY);
            return;
        }

        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        Transform target = _waypointArray[_currentWaypointIndex];
        _direction = (target.position - transform.position).normalized;

        if (_direction.magnitude > 0f)
        {
            _lastDirectionX = _direction.x;
            _lastDirectionY = _direction.y;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, _moveSpeed * Time.deltaTime);
        SetAnimation();

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypointCoroutine());
        }
    }

    private IEnumerator WaitAtWaypointCoroutine()
    {
        _isWaiting = true;
        _anim.SetBool(IS_WALKING_ANIM_NAME, false);
        _anim.SetFloat(LAST_DIRECTION_X_ANIM_NAME, _lastDirectionX);
        _anim.SetFloat(LAST_DIRECTION_Y_ANIM_NAME, _lastDirectionY);

        yield return _waitTime;
        _currentWaypointIndex = _loopWaypoints ? (_currentWaypointIndex + 1) % _waypointArray.Length : Mathf.Min(_currentWaypointIndex + 1, _waypointArray.Length - 1);
        
        _isWaiting = false;
    }

    private void SetAnimation()
    {
        _anim.SetFloat(INPUT_X_ANIM_NAME, _direction.x);
        _anim.SetFloat(INPUT_Y_ANIM_NAME, _direction.y);
        _anim.SetBool(IS_WALKING_ANIM_NAME, _direction.magnitude > 0f);
    }
}