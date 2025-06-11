using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private GameObject bustedText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ShowBustedMessage()
    {
        bustedText.SetActive(true);
    }
}