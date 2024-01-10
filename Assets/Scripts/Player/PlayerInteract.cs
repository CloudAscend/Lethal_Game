using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : PlayerBase
{
    public bool grabable;

    [SerializeField] private KeyCode grabKey;
    [SerializeField] private KeyCode dropKey;
    [SerializeField] private KeyCode throwKey;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform dropPos;

    [SerializeField] private float detectDistance;
    [SerializeField] private LayerMask whatIsItem;

    private PlayerMovement playerMovement;

    //[SerializeField] private 
    RaycastHit hit;

    private void Start()
    {
        TryGetComponent(out playerMovement);
    }

    private void Update()
    {
        DetectItem();
        Drop();
        ThrowItem();
    }

    private void DetectItem()
    {
        Debug.DrawRay(cam.position, cam.forward, Color.red);
        if (Physics.Raycast(cam.position, cam.forward, out hit, detectDistance, whatIsItem))
        {
            
            UIManager.Instance.SetPickUpText(true);
            if (GameManager.instance.HasHoldItem()) return;
            ItemBase detectItem = hit.collider.GetComponent<ItemBase>();
            PickUp(detectItem);
        } else
        {
            UIManager.Instance.SetPickUpText(false);
        }
    }

    private void PickUp(ItemBase item)
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

                GameManager.instance.player.GetComponent<PlayerInventory>().RemoveInventory();
            }
        }
    }

    private void ThrowItem()
    {
        if(Input.GetKeyDown(throwKey))
        {
            if (GameManager.instance.HasHoldItem())
            {
                if (!(playerMovement.stamina >= 0.5f)) return;
                playerMovement.SpendStamina(0.5f);
                Transform dropItem = GameManager.instance.DropItem();
                dropItem.position = dropPos.position;
                Rigidbody rigid;
                dropItem.TryGetComponent(out rigid);
                rigid.velocity = cam.forward * 15;

                GameManager.instance.player.GetComponent<PlayerInventory>().RemoveInventory();
            }
        }
    }
}
