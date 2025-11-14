using UnityEngine;

public class MissionManager : MonoBehaviour
{
    // Esta clase se dedica a guardar exclusivamente el estado de las misiones (completada/no completada)

    // Misión 0 - desbloqueada al inicio
    // Misión 1 - requiere completar 0
    // Misión 2 - requiere completar 1

    public bool[] completed = new bool[3];

    public bool IsUnlocked(int index)
    {
        if (index == 0) return true; // primera siempre desbloqueada

        return completed[index - 1];
    }

    public void MarkCompleted(int index)
    {
        completed[index] = true;
    }
}
