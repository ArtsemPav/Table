using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    public IslandConfig currentIsland;
    public Transform container;
    public LevelButtonUI levelButtonPrefab;

    private void Awake()
    {
        if (SaveManager.Instance.playerData.LastSelectedIsLand != null)
            currentIsland = SaveManager.Instance.playerData.LastSelectedIsLand;
    }

    private void Start()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);

        if (currentIsland == null || currentIsland.levels == null) return;

        foreach (var lvl in currentIsland.levels)
        {
            var btn = Instantiate(levelButtonPrefab, container);
            btn.Setup(lvl);
        }
    }
}
