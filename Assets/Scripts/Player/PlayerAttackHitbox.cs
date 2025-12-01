using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    // -> Actualizado para ser generalizado
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("CHOCÓ CON: " + other.name);

        // Busca health en el objeto
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy == null)
        {
            Debug.Log("No se encontró MushroomHealth en " + other.name);
            return;
        }

        if (StatManager.Instance == null)
        {
            Debug.LogError("StatManager.Instance es null");
        }

        int dmg = StatManager.Instance.GetStrength();
        Debug.Log("Aplicando daño a " + enemy.name);

        enemy.EnemyTakeDamage(dmg);
    }
}