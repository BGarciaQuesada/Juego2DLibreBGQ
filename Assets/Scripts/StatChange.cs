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
    
}