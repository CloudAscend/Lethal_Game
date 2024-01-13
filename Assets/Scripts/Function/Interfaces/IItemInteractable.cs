using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemInteractable
{
    public void Init(object param = null);
    
    public void Interact(ItemBase item);
}
