using UnityEngine;
public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;
    [SerializeField] private Transform orientation;
    private float _xRotation;
    private float _yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //For mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;
        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation,-89f, +89f);
        //for cam and players rotation
        transform.rotation =Quaternion.Euler(_xRotation,_yRotation,0);
        orientation.rotation =Quaternion.Euler(_xRotation,_yRotation,0);
    }
}
