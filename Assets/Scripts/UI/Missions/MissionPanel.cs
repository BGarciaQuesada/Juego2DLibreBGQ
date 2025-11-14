using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissionPanel : MonoBehaviour
{
    public static MissionPanel Instance;

    [SerializeField] private GameObject missionPanel;

    private bool playerNearby = false;
    private bool holdingInteract = false;

    private void Awake()
    {
        // Aunque lo dejo desactivado por defecto en el editor, por si acaso
        missionPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerNearby = true;
        InteractionHint.Instance.ShowHint("Mantén <b>[E]</b> para ver misiones");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerNearby = false;
        holdingInteract = false;

        ClosePanel();
        InteractionHint.Instance.HideHint();
    }

    public void OnInteract(InputValue value)
    {
        if (!playerNearby) return;
        missionPanel.SetActive(true);
    }

    public void OpenPanel()
    {
        missionPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        missionPanel.SetActive(false);
    }
}