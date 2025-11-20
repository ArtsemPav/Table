using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoader : MonoBehaviour
{
    [Header("Addressable References")]
    public AssetReferenceSprite powerUpIconRef;
    public AssetReference bossPrefabRef;

    [Header("UI")]
    public UnityEngine.UI.Image powerUpImageSlot;

    private GameObject _spawnedBoss;

    private void Start()
    {
        LoadPowerUpIcon();
        SpawnBoss();
    }

    private void LoadPowerUpIcon()
    {
        if (powerUpIconRef == null) return;

        powerUpIconRef.LoadAssetAsync<Sprite>().Completed += OnPowerUpIconLoaded;
    }

    private void OnPowerUpIconLoaded(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            powerUpImageSlot.sprite = handle.Result;
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefabRef == null) return;

        bossPrefabRef.InstantiateAsync(transform.position, Quaternion.identity).Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                _spawnedBoss = op.Result;
            }
        };
    }
}
