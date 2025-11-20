using System.Collections;
using System.Collections.Generic;

public interface IHurtable
{
    void GetHurt(Damage damage);
    void Die();
}
