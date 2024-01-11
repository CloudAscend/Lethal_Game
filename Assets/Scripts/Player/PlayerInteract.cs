using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : PlayerBase
{
    [System.Serializable]
    public struct InteractKeyCode
    {
        public KeyCode grabKey;
        public KeyCode dropKey;
        public KeyCode throwKey;
    }

    public bool grabable;

    [SerializeField] InteractKeyCode _interactKey;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform dropPos;

    [SerializeField] private float detectDistance;
    [SerializeField] private LayerMask whatIsItem;

    private PlayerMovement playerMovement;
    private ItemDetection itemDetection;
    private KeyCodeEvent keyCodeEvent = new();

    private ItemBase detectedItem;
    private SellArea sellArea;

    private void Start()
    {
        TryGetComponent(out playerMovement);
        TryGetComponent(out itemDetection);
        keyCodeEvent.AddKeyEvent(() => Input.GetKeyDown(_interactKey.grabKey), InteractItem);
        keyCodeEvent.AddKeyEvent(() => Input.GetKeyDown(_interactKey.dropKey), Drop);
        keyCodeEvent.AddKeyEvent(() => Input.GetKeyDown(_interactKey.throwKey), ThrowItem);
    }

    private void Update()
    {
        CheckDetectItem();
        //CheckSellArea();
        KeyEvent();
    }

    private void CheckDetectItem()
    {
        detectedItem = itemDetection.DetectItem(detectDistance);
        sellArea = itemDetection.DetectSellArea(detectDistance);
        if (detectedItem != null && !GameManager.instance.HasHoldItem())
        {
            UIManager.Instance.SetInteractText(true,"[ Pick Up ]");
        }
        else if(sellArea != null && GameManager.instance.HasHoldItem())
        {
            UIManager.Instance.SetInteractText(true, "[ Sell Item ]");
        }
        else if(sellArea == null && detectedItem == null && itemDetection.IsDetected(detectDistance,GameManager.instance.callButton))
        {
            UIManager.Instance.SetInteractText(true, "[ Call ]");
        }
        else
        {
            UIManager.Instance.SetInteractText(false);
        }
    }

    private void KeyEvent()
    {
        if (Input.anyKeyDown)
        {
            foreach (var keyEvent in keyCodeEvent.GetKeyEvents())
            {
                if (keyEvent.Key.Invoke())
                {
                    keyEvent.Value.Invoke();
                }
            }
        }
    }

    private void InteractItem()
    {
        var g = GameManager.instance;
        if(g.HasHoldItem() && itemDetection.CheckSellArea(detectDistance) && sellArea != null)
        {
            Debug.Log("Test1");
            SellItem();
            return;
        } 
        if(!g.HasHoldItem() && detectedItem != null)
        {
            Debug.Log("Test2");
            PickUp();
            return;
        }
        if(itemDetection.IsDetected(detectDistance, GameManager.instance.callButton))
        {
            GameManager.instance.SellItems();
            Debug.Log("Test3");
            return;
        }
        Debug.Log("Test4");
        //else if()
    }

    private void SellItem()
    {
        Transform dropItem = GameManager.instance.DropItem();
        detectedItem = null;
        dropItem.position = dropPos.position;
        dropItem.TryGetComponent(out ItemBase item);
        dropItem.TryGetComponent(out Rigidbody rigidbody);
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        item.IsGrabbed = false;
        GameManager.instance.player.GetComponent<PlayerInventory>().RemoveInventory();
        sellArea.ItemSell(item);
    }

    private void PickUp()
    {
        // var detectedItem = ItemDetection.GetDetectedItem();
        if (detectedItem == null)
        {
            return;
        }
        detectedItem.ScanUIOff();
        GameManager.instance.GetItem(detectedItem);
    }

    private void Drop()
    {
        if (GameManager.instance.HasHoldItem())
        {
            Transform dropItem = GameManager.instance.DropItem();
            detectedItem = null;
            dropItem.position = dropPos.position;
            dropItem.TryGetComponent(out ItemBase item);
            item.IsGrabbed = false;
            GameManager.instance.player.GetComponent<PlayerInventory>().RemoveInventory();
            
        }
    }

    private void ThrowItem()
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
            detectedItem = null;
            GameManager.instance.player.GetComponent<PlayerInventory>().RemoveInventory();
        }
    }
}
