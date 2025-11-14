using UnityEngine;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
    public int missionIndex;
    public Button button;
    public MissionManager missionManager;

    public void Refresh()
    {
        // ¿La misión está desbloqueada? Habilitar/deshabilitar botón
        bool unlocked = missionManager.IsUnlocked(missionIndex);
        button.interactable = unlocked;
    }

    public void SelectMission()
    {
        if (!missionManager.IsUnlocked(missionIndex))
            return;

        // Aquí cargara escena
        Debug.Log("Misión " + missionIndex + " seleccionada");
    }
}
