using UnityEngine;

public class StatManager : MonoBehaviour
{
    // Esta clase se dedica a guardar estadísticas del Player

    public static StatManager Instance;

    [SerializeField] int energy = 100;
    [SerializeField] int strength = 10;
    [SerializeField] int defense = 10;
    [SerializeField] int money = 0;

    [Header("Sonidos Objetos")]
    [SerializeField] AudioClip usageSound;

    public int GetEnergy() => energy;
    public int GetStrength() => strength;
    public int GetDefense() => defense;
    public int GetMoney() => money;

    public void SetDefense(int defense)
    {
        this.defense = defense;

    }

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

        AudioManager.Instance.PlaySFX(usageSound);

        // Los this los estoy dejando por si luego cambio el nombre, que no haya lio. Luego borrar si eso.
        this.energy += energyChange;
        this.strength += strengthChange;
        this.defense += defenseChange;
        this.money += moneyChange;

        Debug.Log($"Stats -> Energy: {energy}, Strength: {strength}, Defense: {defense}, Money: {money}");

        StatsUI.Instance?.Refresh();
    }
}
