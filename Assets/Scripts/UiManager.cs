using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    private void Awake()
    {
        instance = this;
    }

    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI splitTime;
}
