using TMPro;
using UnityEngine;

public class HUDController : Singleton<HUDController>
{
    [SerializeField] private TMP_Text interactionText;
    
    public void EnableInteractionText(string text)
    {
        interactionText.text = text;
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
