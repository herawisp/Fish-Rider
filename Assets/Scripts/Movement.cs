using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {

    public float MaxSpeed;
    public float Force;
    public float Gravity;

    Vector2 _forceDirection = Vector2.zero;
    Vector2 _movementDirection = Vector2.zero;
    bool _isHoldingInput = false;

    Rigidbody2D _rigidbody;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (_isHoldingInput) {
            _forceDirection = new(0, Force);
            _movementDirection += _forceDirection;
            _movementDirection = Vector2.ClampMagnitude(_movementDirection, MaxSpeed);
        }    

        _movementDirection -= new Vector2(0, Gravity);
        _rigidbody.linearVelocity = _movementDirection;
    }

    void OnFly(InputValue inputValue) {
        _isHoldingInput = inputValue.isPressed;
    }
}
