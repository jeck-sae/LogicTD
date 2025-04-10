using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace TowerDefense
{
    public class SpawnRandomTowers : MonoBehaviour
    {
        [SerializeField] int spawnChance = 30;
        IEnumerator Start()
        {
            yield return null;
            var allTowers = Resources.LoadAll("Prefabs/Towers/");
            var towers = allTowers.Where(x => x.GetComponent<Tower>() != null).ToList();

            var tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
            foreach(var t in tiles)
            {
                if (t.CanBuildOver)
                {
                    if(Random.Range(0, 100) < spawnChance)
                    {
                        var tower = Instantiate((GameObject)towers[Random.Range(0, towers.Count() - 1)]);
                        t.PlaceTower(tower.GetComponent<Tower>());
                    }
                }
            }
        }
    }
}
