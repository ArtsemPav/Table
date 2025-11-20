using UnityEngine;

public class IslandSelectUI : MonoBehaviour
{
    [Header("Data")]
    public IslandConfig[] islands;        // задать в инспекторе

    [Header("UI")]
    public Transform container;           // родитель для кнопок (Grid/Horizontal Layout Group)
    public IslandButtonUI islandButtonPrefab;

    private void Start()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        if (islands == null || islands.Length == 0)
        {
            Debug.LogWarning("IslandSelectUI: no islands assigned.");
            return;
        }

        // очистим старые кнопки
        foreach (Transform child in container)
            Destroy(child.gameObject);

        foreach (var island in islands)
        {
            var btn = Instantiate(islandButtonPrefab, container);
            btn.Setup(island);
        }
    }
}
