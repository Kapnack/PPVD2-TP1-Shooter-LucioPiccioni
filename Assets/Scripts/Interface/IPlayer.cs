using System;
using UnityEngine;

public interface IPlayer
{
    public bool IsDead();

    Vector3 getPos();
    bool IsLookingAt(Vector3 position);
    void ReciveDamage(float amount);
}
