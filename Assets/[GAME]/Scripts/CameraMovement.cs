using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;

    private void Update()
    {
        transform.position = cameraPos.position;
    }
}
