using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutcomeUI : MonoBehaviour
{
    public static OutcomeUI Instance;

    [Header("Elementos UI")]
    [SerializeField] private GameObject panelOutcome;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button returnButton;

    private void Awake()
    {
        Instance = this;
        panelOutcome.SetActive(false);
        // Botón
        returnButton.gameObject.SetActive(false);
        returnButton.onClick.AddListener(ReturnToInbetween);
    }

    public void ShowMessage(string msg)
    {
        messageText.text = msg;
        panelOutcome.SetActive(true);

        // Mostrar botón cuando se muere el jugador o enemigo
        returnButton.gameObject.SetActive(true);
    }

    public void ReturnToInbetween()
    {
        // Resets opcionales
        if (StatManager.Instance.GetDefense() < 0)
            StatManager.Instance.SetDefense(10);

        SceneManager.LoadScene("Inbetween");
    }
}
