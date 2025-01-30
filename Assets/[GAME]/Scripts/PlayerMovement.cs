using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float _movementSpeed;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool _readyToJump;
    
    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    
    [Header("Ground Check")] 
    [SerializeField] private float playerHeight;
    [SerializeField]private bool grounded;
    [SerializeField] private bool onSlope;
    private bool _exitingSlope;
    [SerializeField] private Transform orientation;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private Vector3 _slopeMoveDirection;
    private Rigidbody _myBody;

    [SerializeField] private MovementState currentMovementState;
    
    private enum MovementState
    {
        Walking,
        Sprinting,
        OnAir,
    }

    private void MoveStateHandler()
    {
        switch (grounded)
        {
            case true when Input.GetKey(sprintKey):
                currentMovementState = MovementState.Sprinting;
                _movementSpeed = sprintSpeed;
                break;
            case true:
                currentMovementState = MovementState.Walking;
                _movementSpeed = walkSpeed;
                break;
            default:
                currentMovementState = MovementState.OnAir;
                break;
        }
    }

    private void Start()
    {
        _myBody = GetComponent<Rigidbody>();
        _myBody.freezeRotation = true;
        _readyToJump = true;
    }

    private void Update()
    {
        GroundRayCheck();
        MyInput();
        _myBody.drag = grounded ? groundDrag:0;
        SpeedControl();
        MoveStateHandler();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        //Jump
        if (Input.GetKey(jumpKey) && _readyToJump && grounded)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump),jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // move direction
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        _moveDirection = new Vector3(_moveDirection.x, 0f, _moveDirection.z);
        // on slope
        if (onSlope && !_exitingSlope)
        {
            _myBody.AddForce(_slopeMoveDirection.normalized * (_movementSpeed * 20f),ForceMode.Force);
            if (_myBody.velocity.y > 0)
            {
                _myBody.AddForce(Vector3.down * 80f,ForceMode.Force);
            }
        }
        //on floor
        else if (grounded)
        {
            _myBody.AddForce(_moveDirection.normalized * (_movementSpeed * 10f), ForceMode.Force);
        }
        // on air
        else
        {
            _myBody.AddForce(_moveDirection.normalized * (_movementSpeed * 10f * airMultiplier), ForceMode.Force);
        }

        _myBody.useGravity = !onSlope;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_myBody.velocity.x, 0f, _myBody.velocity.z);
        if (onSlope)
        {
            if (_myBody.velocity.magnitude > _movementSpeed)
            {
                _myBody.velocity = _myBody.velocity.normalized * _movementSpeed;
            }
        }
        //Limit velocity
        else
        {
            if (!(flatVel.magnitude > _movementSpeed)) return;
            Vector3 limitedVel = flatVel.normalized * _movementSpeed;
            _myBody.velocity = new Vector3(limitedVel.x, _myBody.velocity.y, limitedVel.z);
        }
    }
    
    private void GroundRayCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down,out RaycastHit hit, playerHeight * 0.5f + 0.2f))
        {
            grounded = true;
            var groundType = hit.transform.GetComponent<Ground>().groundType;
            if (groundType == Ground.GroundType.Sloped)
            {
                onSlope = true;
                _slopeMoveDirection = Vector3.ProjectOnPlane(_moveDirection, hit.normal);
            }
            if (groundType == Ground.GroundType.Floor)
            {
                onSlope = false;
            }
        }
        else
        {
            onSlope = false;
            grounded = false;
        }
        
    }

    private void Jump()
    {
        _exitingSlope = true;
        _myBody.velocity = new Vector3(_myBody.velocity.x, 0f, _myBody.velocity.z);
        _myBody.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }
    
    private void ResetJump()
    {
        _readyToJump = true;
        _exitingSlope = false;
    }
}
