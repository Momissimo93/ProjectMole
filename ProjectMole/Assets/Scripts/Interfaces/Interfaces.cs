using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void GetDamage();
}

public interface IMarkable
{
    void ActivateMarker();
    void DeactivateMarker();
}
public interface IFixable
{
    void Fix();
}
