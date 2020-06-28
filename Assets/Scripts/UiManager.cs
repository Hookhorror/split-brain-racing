using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager uiManager;
    private void Awake()
    {
        uiManager = this;
    }

    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI splitTime;
}
