using UnityEngine;

public interface IEarlyAwakeableManager
{
    /// <summary>
    /// Call Earlier Interface Functions Than UnityCicle's Awake
    /// </summary>
    public void PreAwake();

}
public class ManagerInitializer : MonoBehaviour
{
    private IEarlyAwakeableManager[] _managers;
    private void Awake()
    {
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        _managers = GetComponentsInChildren<IEarlyAwakeableManager>(true);
        if (_managers == null || _managers.Length == 0) return;

        foreach (var manager in _managers)
        {
            manager.PreAwake();
        }

        Debug.Log($"[ManagerInitializer] PreAwake Run in {_managers.Length} Managers");
    }

}