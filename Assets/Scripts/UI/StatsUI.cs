using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("Textos UI")]
    [SerializeField] private TextMeshProUGUI numEnergy;
    [SerializeField] private TextMeshProUGUI numMoney;
    [SerializeField] private TextMeshProUGUI numStrength;
    [SerializeField] private TextMeshProUGUI numDefense;

    public static StatsUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (StatManager.Instance == null) return;

        numEnergy.text = StatManager.Instance.GetEnergy().ToString();
        numStrength.text = StatManager.Instance.GetStrength().ToString();
        numDefense.text = StatManager.Instance.GetDefense().ToString();
        numMoney.text = StatManager.Instance.GetMoney().ToString();
    }
}
