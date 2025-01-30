using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Outline outline;
    public string message;
    
    void Start()
    {
        DisableOutline();
    }

    public abstract void Interact();

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }
}
