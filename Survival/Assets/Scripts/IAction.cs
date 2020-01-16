using UnityEngine;

public interface IAction<T> where T : Component
{
    void DoAction(T param);
}