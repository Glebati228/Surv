using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStates : MonoBehaviour
{
    public int id;
    public int count;
    private Context context;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

interface IState
{
    void Handle(Context context);
}

class TakingState : IState
{
    public void Handle(Context context)
    {
            
    }
}

class DefaultState : IState
{
    public void Handle(Context context)
    {
        
    }
}

class ItemContext : Context
{
    public IState state;

    public ItemContext(IState state)
    {
        this.state = state;
    }

    public void Request()
    {
        this.state.Handle(this);
    }
}

internal class Context
{

}