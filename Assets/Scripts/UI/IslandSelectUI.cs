using UnityEngine;
using Game.Data;

public class IslandSelectUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform _container;           // родитель для кнопок (Grid/Horizontal Layout Group)
    [SerializeField] private IslandButtonUI _islandButtonPrefab;

    private IslandConfig[] _islands;

    private void Start()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        _islands = GameManager.Instance.IslandsList;
        if (_islands == null || _islands.Length == 0)
        {
            Debug.LogWarning("IslandSelectUI: no islands assigned.");
            return;
        }

        // очистим старые кнопки
        foreach (Transform child in _container)
            Destroy(child.gameObject);

        foreach (var island in _islands)
        {
            var btn = Instantiate(_islandButtonPrefab, _container);
            btn.Setup(island);
        }
    }
}
