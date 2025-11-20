using UnityEngine;
using UnityEngine.InputSystem;

public class StatChange : MonoBehaviour
{
    // Esta clase se dedica a llamar a los métodos de StatManager para cambiar valores.
    // Se ha hecho aparte para poder reutilizarlo con los distintos entrenamientos.

    public int energyChange;
    public int defenseChange;
    public int strengthChange;
    public int moneyChange;

    public void Interact()
    {
        Debug.Log("El jugador ha usado el objeto.");

        if (StatManager.Instance != null)
        {
            StatManager.Instance.ChangeStats(energyChange, strengthChange, defenseChange, moneyChange);
        }
        else
        {
            Debug.LogWarning("No se encontró instancia de StatManager en la escena.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ¿Está player cerca? Mostrar stats que da/consume
        if (!other.CompareTag("Player")) return;
        CreateHintText();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // ¿Se ha ido player? adiós cartelito
        if (!other.CompareTag("Player")) return;
        InteractionHint.Instance?.HideHint();
    }

    private void CreateHintText()
    {
        // Si no existe cartel, fuera
        if (InteractionHint.Instance == null) return;

        // --- Título (negrita) ---
        string message = "Mantén <b>[E]</b> para interactuar\n";

        // --- Cambios de stats (solo los que se alteran) ---
        if (energyChange != 0)
            message += $"{FormatChange(energyChange, "energía")}\n";
        if (strengthChange != 0)
            message += $"{FormatChange(strengthChange, "fuerza")}\n";
        if (defenseChange != 0)
            message += $"{FormatChange(defenseChange, "defensa")}\n";
        if (moneyChange != 0)
            message += $"{FormatChange(moneyChange, "dinero")}\n";

        InteractionHint.Instance.ShowHint(message.TrimEnd());
    }

    private string FormatChange(int value, string statName)
    {
        // DEFINIR SIGNO
        // Aka. ¿Es un valor positivo? Le añadimos un +. ¿Ya era negativo? Se queda como está
        string sign = value > 0 ? "+" : "";

        // COLOR SEGÚN VALOR
        // Positivo es verde (00FF00) : Negativo es rojo (FF5555)
        string color = value >= 0 ? "#00FF00" : "#FF5555";

        // Hemos definido su color y signo, poner el valor con dichas etiquetas
        return $"<color={color}>{sign}{value}</color> {statName}";
    }


}