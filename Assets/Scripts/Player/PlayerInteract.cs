using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : PlayerBase
{
    public bool grabable;

    [SerializeField] private KeyCode grabKey;
    [SerializeField] private KeyCode dropKey;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform dropPos;

    [SerializeField] private float detectDistance;
    [SerializeField] private LayerMask whatIsItem;

    //[SerializeField] private 
    RaycastHit hit;

    private void Update()
    {
        DetectItem();
        Drop();
    }

    private void DetectItem()
    {
        Debug.DrawRay(cam.position, cam.forward, Color.red);
        if (Physics.Raycast(cam.position, cam.forward, out hit, detectDistance, whatIsItem))
        {
            
            UIManager.Instance.SetPickUpText(true);
            if (GameManager.instance.HasHoldItem()) return;
            Transform detectItem = hit.collider.transform;
            PickUp(detectItem);
        } else
        {
            UIManager.Instance.SetPickUpText(false);
        }
    }

    private void PickUp(Transform item)
    {
       if(Input.GetKeyDown(grabKey))
       {
            GameManager.instance.GetItem(item);
       }
    }

    private void Drop()
    {
        if(Input.GetKeyDown(dropKey))
        {
            if(GameManager.instance.HasHoldItem())
            {
                Transform dropItem = GameManager.instance.DropItem();
                dropItem.position = dropPos.position;
            }
        }
    }
}
