using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpotController : MonoBehaviour
{
    public List<GameObject> spots = new List<GameObject>();

    public GameObject prefabEnemy;

    private void Start()
    {
        StartCoroutine("EnemyCreator");
    }

    private IEnumerator EnemyCreator()
    {
        yield return new WaitForSeconds(2);

        while (true)
        {
            yield return null;

            int numEnemysToCreate = Random.Range(0, spots.Count);

            for (int i = 0; i < numEnemysToCreate; i++)
            {
                GameObject spot = spots[Random.Range(0, spots.Count)];

                if (spot.transform.childCount > 1) continue;

                GameObject enemy = Instantiate(prefabEnemy, spot.transform);
                enemy.transform.DOLocalMoveY(1, 1);
            }

            yield return new WaitForSeconds(5);
        }
    }
}
