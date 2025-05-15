using UnityEngine;
using UnityEngine.UI;

public class PlanetButton : MonoBehaviour
{
    [Header("Display Settings")]
    public string displayName;
    public ButtonDisplay displayController;
    
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => {
                if (displayController != null)
                    displayController.ShowButtonName(displayName);
            });
        }
    }
    
    public void OnButtonClick()
    {
        if (displayController != null)
            displayController.ShowButtonName(displayName);
    }
}