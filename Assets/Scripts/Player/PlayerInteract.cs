using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : PlayerBase
{
    public bool grabable;

    [SerializeField] private KeyCode grabKey;
    [SerializeField] private KeyCode dropKey;
    //[SerializeField] private 
    private void Update()
    {
        /*
        if (Input.GetKey(grabKey) && grabable)
        */

        //RaycastHit rayHit = Physics.Raycast(transform.position, transform.forward, out rayHit);
    }
}
