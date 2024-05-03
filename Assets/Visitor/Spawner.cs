using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Visitor
{
    public class Spawner : MonoBehaviour, IEnemyDeathNotifier
    {
        [SerializeField] private float _spawnCooldown;
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private EnemyFactory _enemyFactory;

        private List<Enemy> _spawnedEnemies = new List<Enemy>();
        private Coroutine _spawn;
        private EnemyWeightVisitor _enemyWeightVisitor;
        private EnemyConfig _enemyConfig;

        public event Action<Enemy> Notified;

        public void Initialize(EnemyConfig enemyConfig)
        {
            _enemyConfig = enemyConfig;
            _enemyWeightVisitor = new EnemyWeightVisitor(enemyConfig);
        }

        public void StartWork()
        {
            StopWork();

            _spawn = StartCoroutine(Spawn());
        }

        public void StopWork()
        {
            if (_spawn != null)
                StopCoroutine(_spawn);
        }

        public void KillRandomEnemy()
        {
            if (_spawnedEnemies.Count == 0)
                return;

            _spawnedEnemies[UnityEngine.Random.Range(0, _spawnedEnemies.Count)].Kill();
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                if (_enemyConfig.maxWeight > _enemyWeightVisitor.Weight)
                {
                    EnemyType randomEnemyType = (EnemyType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EnemyType)).Length);
                    Enemy enemy = _enemyFactory.Get(randomEnemyType);
                    enemy.MoveTo(_spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position);
                    enemy.Died += OnEnemyDied;
                    _spawnedEnemies.Add(enemy);
                    _enemyWeightVisitor.Visit(enemy);
                }
                else
                {
                    Debug.Log("Достигнут лимит врагов!");
                    Debug.Log(_enemyWeightVisitor.Weight);
                }

                yield return new WaitForSeconds(_spawnCooldown);
            }
        }

        private void OnEnemyDied(Enemy enemy)
        {
            Notified?.Invoke(enemy);
            enemy.Died -= OnEnemyDied;
            _spawnedEnemies.Remove(enemy);
        }

        private class EnemyWeightVisitor : IEnemyVisitor
        {
            private EnemyConfig _enemyConfig;

            public EnemyWeightVisitor(EnemyConfig enemyConfig)
            {
                _enemyConfig = enemyConfig;
            }

            public int Weight { get; private set; }

            public void Visit(Elf elf) => Weight += _enemyConfig.elfWeight;

            public void Visit(Human human) => Weight += _enemyConfig.humanWeight;

            public void Visit(Ork ork) => Weight += _enemyConfig.orkWeight;

            public void Visit(Robot robot) => Weight += _enemyConfig.robotWeight;

            public void Visit(Enemy enemy) => Visit((dynamic)enemy);
        }
    }
}
