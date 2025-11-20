using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        MushroomHealth enemy = other.GetComponent<MushroomHealth>();
        if (enemy != null)
        {
            int dmg = StatManager.Instance.GetStrength();
            enemy.TakeDamage(dmg);
        }
    }
}
