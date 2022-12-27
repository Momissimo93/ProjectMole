using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class Hole : MonoBehaviour, IFixable
{
    private enum TypeOfPosition { Left, Right, Center };

    [SerializeField] 
    private TypeOfPosition typeOfPosition;
    [SerializeField] 
    private Enemy [] enemies;
    [SerializeField] 
    private Transform spawnPosition;
    [SerializeField]
    private Debris debris;

    private GameObject enemy;
    private bool canBeFixed;
    private IEnumerator coroutine;
    private float counter = 3;
    public bool isAnActiveHole { get; set; }

    public delegate void HoleFixed();
    public HoleFixed holeFixed;

    private void OnEnable()
    {
        canBeFixed = false;
        SpawnEnemy();

        if(debris)
        {
            debris.PlayAniamtionDebris();
        }
    }

    public void Update()
    {
        if (counter < 0)
        {
            counter = 3;
            if (debris)
            {
                debris.PlayAniamtionDebris();
            }
        }
        else
        {
            counter -= Time.deltaTime;
        }

    }

    void SpawnEnemy()
    {
        int r = Random.Range(0, enemies.Length);
        switch(typeOfPosition)
        {
            case TypeOfPosition.Left:
                enemy = Instantiate(enemies[r].gameObject, spawnPosition.transform.position, Quaternion.Euler(0, 0, 90));
                enemy.GetComponent<SpriteRenderer>().flipY = true;
                break;
            case TypeOfPosition.Right:
                enemy = Instantiate(enemies[r].gameObject, spawnPosition.transform.position, Quaternion.Euler(0, 0, 90));
                break;
            case TypeOfPosition.Center:
                enemy  = Instantiate(enemies[r].gameObject, spawnPosition.transform.position, Quaternion.Euler(0, 0, 90));
                break;
        }
        enemy.GetComponent<Enemy>().enemyDestroyDelegate += OnDestroyEnemy;
        enemy.GetComponent<Enemy>().holeCanBeFixed += OnHoleCanBeFixed;
    }

    void OnHoleCanBeFixed()
    {
        enemy.GetComponent<Enemy>().holeCanBeFixed -= OnHoleCanBeFixed;
        canBeFixed = true;
    }

    void OnDestroyEnemy()
    {
        enemy.GetComponent<Enemy>().enemyDestroyDelegate -= OnDestroyEnemy;
        Destroy(enemy);
    }
    public void Fix()
    {
        if (canBeFixed)
        {
            holeFixed?.Invoke();
            Debug.Log("The hole is fixed fixed");
        }
        else
        {
            Debug.Log("The hole can not be fixed");
        }
    }
    public void ManualEnemyCleaning()
    {
        if(enemy)
        {
            Destroy(enemy);
        }
    }

    public bool IsAnActiveHole () => isAnActiveHole;
}
