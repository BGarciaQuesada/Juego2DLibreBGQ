using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void Interact()
    {
        string nextLevel = LevelManager.Instance.GetNextLevel();
        if (!string.IsNullOrEmpty(nextLevel))
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            // RECORDATORIO AÑADIR EN EDITOR (Sino petardazo rojo)
            Debug.LogWarning("No hay siguiente nivel definido en LevelManager.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerMovement.Instance?.SetInteractable(this);
        InteractionHint.Instance?.ShowHint("Mantén <b>[E]</b> para entrar al combate.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerMovement.Instance?.SetInteractable(null);
        InteractionHint.Instance?.HideHint();
    }
}

