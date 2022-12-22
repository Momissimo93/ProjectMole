using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public delegate void EnemyDelegate();
    public EnemyDelegate enemyDelegate;
    public void GetDamage()
    {
        enemyDelegate?.Invoke();
    }
}
