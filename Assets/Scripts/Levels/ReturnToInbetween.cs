using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToInbetween : MonoBehaviour
{
    [SerializeField] float holdTime = 2f;
    [SerializeField] string inbetweenScene = "Inbetween";

    private float holdCounter = 0f;
    private bool interacting = false;

    public void Interact()
    {
        interacting = true;
    }

    void Update()
    {
        if (!interacting)
        {
            holdCounter = 0f;
            return;
        }

        holdCounter += Time.deltaTime;

        if (holdCounter >= holdTime)
        {
            SceneManager.LoadScene(inbetweenScene);
        }

        interacting = false;
    }

    // Originalmente había un cartelito, ya no, hay que cambiarlo
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement.Instance?.SetInteractable(this);
        InteractionHint.Instance?.ShowHint("Mantén <b>[E]</b> para volver a la base.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement.Instance?.SetInteractable(null);
        InteractionHint.Instance?.HideHint();
    }
}
