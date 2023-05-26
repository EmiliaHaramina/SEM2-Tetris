using System;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private float _threshold = .1f;
    [SerializeField] private float _deadZone = .025f;

    public static Action<bool> onButtonPressed;

    public UnityEvent onPressed, onReleased;

    private bool _isPressed;
    private Vector3 _startPosition;
    private ConfigurableJoint _joint;

    private void OnEnable()
    {
        GameManager.onGameStarted += SetPressedStatus;
        GameManager.onGameStopped += SetPressedStatus;
    }

    private void OnDisable()
    {
        GameManager.onGameStarted -= SetPressedStatus;
        GameManager.onGameStopped -= SetPressedStatus;
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPressed && GetValue() + _threshold >= 0.19)
        {
            Pressed();
        }
        if (_isPressed && GetValue() - _threshold <= -0.07)
        {
            Released();
        }
    }

    private float GetValue()
    {
        float value = Vector3.Distance(_startPosition, transform.localPosition) / _joint.linearLimit.limit;
        if (Mathf.Abs(value) < _deadZone)
        {
            value = 0;
        }
        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
    }

    private void SetPressedStatus(bool status)
    {
        onButtonPressed?.Invoke(status);
    }
}
