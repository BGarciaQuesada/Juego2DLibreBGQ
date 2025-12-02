using TMPro;
using UnityEngine;

public class OutcomeUI : MonoBehaviour
{
    public static OutcomeUI Instance;

    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text messageText;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowMessage(string msg)
    {
        messageText.text = msg;
        panel.SetActive(true);
    }
}
