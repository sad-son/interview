using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 30f;

    private List<Enemy> _allEnemies = new List<Enemy>();
    private Dictionary<int, Enemy> _enemyById = new Dictionary<int, Enemy>();
    private List<int> _visibleEnemyIds = new List<int>();

    void Update()
    {
        ScanEnemies();
        UpdateTargets();
    }

    private void ScanEnemies()
    {
        _visibleEnemyIds.Clear();

        foreach (var enemy in _allEnemies)
        {
            if (enemy == null)
                continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= _scanRadius)
            {
                _visibleEnemyIds.Add(enemy.Id);
            }
        }
    }

    private void UpdateTargets()
    {
        for (int i = 0; i < _visibleEnemyIds.Count; i++)
        {
            int id = _visibleEnemyIds[i];

            if (_enemyById.ContainsKey(id))
            {
                Enemy enemy = _enemyById[id];
                enemy.MarkAsVisible();
            }
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        _allEnemies.Add(enemy);
        _enemyById.Add(enemy.Id, enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        _allEnemies.Remove(enemy);
        _enemyById.Remove(enemy.Id);
    }
}

public class Enemy : MonoBehaviour
{
    public int Id;

    public void MarkAsVisible()
    {
        // update ui / ai state
    }
}
