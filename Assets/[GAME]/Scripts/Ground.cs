using UnityEngine;
public class Ground : MonoBehaviour
{
    public GroundType groundType;

    public enum GroundType
    {
        Floor,
        Sloped,
    }
}
