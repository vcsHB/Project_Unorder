using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public interface IEarlyAwakeableManager
{
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

        // 2. 순회하며 PreAwake 실행
        foreach (var manager in _managers)
        {
            manager.PreAwake();
        }

        Debug.Log($"[ManagerInitializer] {_managers.Length}개의 매니저 초기화 완료.");
    }

}