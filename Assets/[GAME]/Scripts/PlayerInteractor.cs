using System;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform interactorSource; //player camera
    [SerializeField] private float interactRange;
    private Interactable _currentInteractable;
    
    [Header("Keybinds")] [SerializeField] private KeyCode interactKey = KeyCode.E;

    private void Update()
    {
        CheckInteraction();
        if (Input.GetKey(interactKey))
        {
           _currentInteractable?.Interact();
        }
    }

    private void CheckInteraction() // burayı kontrol et optimize edebilirmisin diye
    {
        Ray ray = new Ray(interactorSource.transform.position, interactorSource.transform.forward);

        if (Physics.Raycast(ray,out var hit,interactRange))
        {
            if (hit.transform.TryGetComponent(out Interactable target))
            {
                if (_currentInteractable && target != _currentInteractable)
                {
                    DisableCurrentInteractable();
                }

                if (!target.enabled || _currentInteractable) return;
                _currentInteractable = target;
                target.EnableOutline();
                HUDController.Instance.EnableInteractionText(_currentInteractable.message);
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }
    
    private void DisableCurrentInteractable()
    {
        if (!_currentInteractable) return;
        HUDController.Instance.DisableInteractionText();
        _currentInteractable.DisableOutline();
        _currentInteractable = null;
    }
    
}
