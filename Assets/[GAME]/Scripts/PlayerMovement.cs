using UnityEngine;
using UnityEngine.Android;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed;
    [SerializeField] private float groundDrag;
    
    [Header("Ground Check")] [SerializeField]
    private float playerHeight;
    [SerializeField] private LayerMask isGround;
    private bool _grounded;
    [SerializeField] private Transform orientation;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private Rigidbody _myBody;

    private void Start()
    {
        _myBody = GetComponent<Rigidbody>();
        _myBody.freezeRotation = true;
    }

    private void Update()
    {
        GroundRayCheck();
        MyInput();
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
    }

    private void MovePlayer()
    {
        // orientation yerine oyuncunun modelini referans alarak hem hesaplayıp hemde oyuncuyu dondurebilirsin.
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        _myBody.AddForce(_moveDirection.normalized * (movementSpeed * 10f), ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_myBody.velocity.x, 0, _myBody.velocity.z);
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
        _myBody.drag = _grounded ? groundDrag:0;
    }
    //TODO: jumping and airControl
}
