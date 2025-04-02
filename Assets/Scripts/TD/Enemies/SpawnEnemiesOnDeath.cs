using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    [RequireComponent(typeof(Enemy))]
    public class SpawnEnemiesOnDeath : MonoBehaviour
    {
        public List<EnemySpawnSettings> spawnSettings;

        protected Enemy enemy;
        private void Awake()
        {
            enemy = GetComponent<Enemy>();
        }

        private void OnEnable()
        {
            enemy.OnDeath += Spawn;
        }

        private void OnDisable()
        {
            enemy.OnDeath -= Spawn;
        }

        protected void Spawn(Enemy.OnDeathArgs args)
        {
            if (args.reachedHomeTile)
                return;

            foreach(var s in spawnSettings)
            {
                for(int i = 0; i< s.amount; i++)
                {
                    UnityEngine.Random.Range(-1, 1);
                    Vector3 pos = transform.position + UnityEngine.Random.onUnitSphere * s.distance;
                    var go = Instantiate(s.prefab, pos, Quaternion.identity);
                    go.GetComponent<Enemy>().AddBaseModifiers(enemy.startModifiers);
                }
            }
        }

        [Serializable]
        public class EnemySpawnSettings
        {
            public GameObject prefab;
            public int amount;
            public float distance;
        }
    }
}
