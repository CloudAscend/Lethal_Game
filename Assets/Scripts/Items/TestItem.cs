using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : ItemBase
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Interact(IItemInteractable item)
    {
        base.Interact(item);
    }
}
