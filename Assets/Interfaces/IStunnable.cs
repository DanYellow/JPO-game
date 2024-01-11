using System;
using System.Collections;

public interface IStunnable
{
    IEnumerator Stun(float stunTime, Action callback);
}