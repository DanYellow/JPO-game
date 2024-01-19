using System;
using System.Collections;

public interface IStunnable
{
    void Stun(float stunTime, Action callback);
}