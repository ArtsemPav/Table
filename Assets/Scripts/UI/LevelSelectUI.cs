using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private LevelButtonUI _levelButtonPrefab;

    private IslandConfig _currentIsland;

    private void Start()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        _currentIsland = GameManager.Instance.GetLastIslandConfig();
        foreach (Transform child in _container)
            Destroy(child.gameObject);

        if (_currentIsland == null)
        {
            Debug.LogWarning("LevelSelectUI: no island assigned.");
            return;
        } else if (_currentIsland.levels == null)
        {
            Debug.LogWarning("LevelSelectUI: no levels assigned.");
            return;
        }
        foreach (var lvl in _currentIsland.levels)
        {
            var btn = Instantiate(_levelButtonPrefab, _container);
            btn.Setup(lvl);
        }
    }
}
