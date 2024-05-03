using UnityEngine;

namespace Assets.Visitor
{
    public class VisitorBootstrap : MonoBehaviour
    {
        [SerializeField] private Spawner _enemySpawner;
        [SerializeField] private EnemyConfig _enemyConfig;

        private Score _score;

        private void Awake()
        {
            _score = new Score(_enemySpawner, _enemyConfig);
            _enemySpawner.Initialize(_enemyConfig);
            _enemySpawner.StartWork();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
                _enemySpawner.KillRandomEnemy();
        }
    }
}

