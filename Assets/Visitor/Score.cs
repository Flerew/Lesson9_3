using System;
using UnityEngine;

namespace Assets.Visitor
{
    public class Score : IDisposable
    {
        public int Value => _enemyVisitor.Score;

        private IEnemyDeathNotifier _enemyDeathNotifier;
        private EnemyVisitor _enemyVisitor;

        public Score(IEnemyDeathNotifier enemyDeathNotifier, EnemyConfig enemyConfig)
        {
            _enemyDeathNotifier = enemyDeathNotifier;
            _enemyDeathNotifier.Notified += OnEnenmyKilled;

            _enemyVisitor = new EnemyVisitor(enemyConfig);
        }

        public void OnEnenmyKilled(Enemy enemy)
        {
            _enemyVisitor.Visit(enemy);
            Debug.Log("Cчет: " + Value);
        }

        public void Dispose()
        {
            _enemyDeathNotifier.Notified -= OnEnenmyKilled;
        }

        private class EnemyVisitor : IEnemyVisitor
        {
            private EnemyConfig _enemyConfig;

            public EnemyVisitor(EnemyConfig enemyConfig)
            {
                _enemyConfig = enemyConfig;
            }

            public int Score { get; private set; }

            public void Visit(Elf elf) => Score += _enemyConfig.elfScore;

            public void Visit(Human human) => Score += _enemyConfig.humanScore;

            public void Visit(Ork ork) => Score += _enemyConfig.orkScore;

            public void Visit(Robot robot) => Score += _enemyConfig.robotScore;

            public void Visit(Enemy enemy) => Visit((dynamic) enemy);
        }
    }
}

