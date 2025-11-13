using UnityEngine;

public class StatManager : MonoBehaviour
{
    // Esta clase se dedica a guardar estadísticas del Player

    public static StatManager Instance;

    [SerializeField] int energy = 100;
    [SerializeField] int strength = 10;
    [SerializeField] int defense = 10;
    [SerializeField] int money = 0;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeStats(int energyChange, int strengthChange, int defenseChange, int moneyChange)
    {
        // --- Validación de recursos ---
        // Si el cambio es negativo (consumo), comprobar si el jugador tiene suficiente -> Sino, return
        // PREGUNTAR SI QUEDA MÁS LIMPIO HACER UN MÉTODO APARTE SOLO PARA ESTO O ES CONSUMO INNECESARIO
        if (energy + energyChange < 0)
        {
            Debug.Log("No hay suficiente energía.");
            return;
        }

        if (money + moneyChange < 0)
        {
            Debug.Log("No hay suficiente dinero.");
            return;
        }

        // Los this los estoy dejando por si luego cambio el nombre, que no haya lio. Luego borrar si eso.
        this.energy += energyChange;
        this.strength += strengthChange;
        this.defense += defenseChange;
        this.money += moneyChange;

        Debug.Log($"Stats -> Energy: {energy}, Strength: {strength}, Defense: {defense}, Money: {money}");
    }
}
