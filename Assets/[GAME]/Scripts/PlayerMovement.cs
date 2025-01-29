using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool _readyToJump;
    
    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [Header("Ground Check")] [SerializeField]
    private float playerHeight;
    [SerializeField] private LayerMask isGround;
    [SerializeField]private bool _grounded;
    [SerializeField] private Transform orientation;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private Rigidbody _myBody;

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
        _myBody.drag = _grounded ? groundDrag:0;
        SpeedControl();
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
        if (Input.GetKey(jumpKey) && _readyToJump && _grounded)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump),jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // orientation yerine oyuncunun modelini referans alarak hem hesaplayıp hemde oyuncuyu dondurebilirsin.
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        _moveDirection = new Vector3(_moveDirection.x, 0f, _moveDirection.z);
        _myBody.AddForce(_grounded ? _moveDirection.normalized * (movementSpeed * 10f) :
            _moveDirection.normalized * (movementSpeed * 10f * airMultiplier), ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_myBody.velocity.x, 0f, _myBody.velocity.z);
        //Limit velocity
        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            _myBody.velocity = new Vector3(limitedVel.x, _myBody.velocity.y, limitedVel.z);
        }
    }

    private void GroundRayCheck()
    {
        // player zemin olmayan objelere zıpladığında tırtlar bu(daha iyisini yap)
        _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, isGround);
    }

    private void Jump()
    {
        _myBody.velocity = new Vector3(_myBody.velocity.x, 0f, _myBody.velocity.z);
        _myBody.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }
    
    private void ResetJump()
    {
        _readyToJump = true;
    }
}
