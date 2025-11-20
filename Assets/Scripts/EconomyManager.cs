using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    public int Coins { get; private set; }
    public int Crystals { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Load();
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        Save();
    }

    public void AddCrystals(int amount)
    {
        Crystals += amount;
        Save();
    }

    public bool TrySpendCoins(int amount)
    {
        if (Coins < amount) return false;

        Coins -= amount;
        Save();
        return true;
    }

    public bool TrySpendCrystals(int amount)
    {
        if (Crystals < amount) return false;

        Crystals -= amount;
        Save();
        return true;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("coins", Coins);
        PlayerPrefs.SetInt("crystals", Crystals);
    }

    private void Load()
    {
        Coins = PlayerPrefs.GetInt("coins", 0);
        Crystals = PlayerPrefs.GetInt("crystals", 0);
    }
}
