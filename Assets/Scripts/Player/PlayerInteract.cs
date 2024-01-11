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

    private void Start()
    {
        TryGetComponent(out playerMovement);
        TryGetComponent(out itemDetection);
        keyCodeEvent.AddKeyEvent(() => Input.GetKeyDown(_interactKey.grabKey), PickUp);
        keyCodeEvent.AddKeyEvent(() => Input.GetKeyDown(_interactKey.dropKey), Drop);
        keyCodeEvent.AddKeyEvent(() => Input.GetKeyDown(_interactKey.throwKey), ThrowItem);
    }

    private void Update()
    {
        CheckDetectItem();
        KeyEvent();
    }

    private void CheckDetectItem()
    {
        detectedItem = itemDetection.DetectItem(detectDistance);

        if (detectedItem != null)
        {
            if (GameManager.instance.HasHoldItem())
            {
                UIManager.Instance.SetPickUpText(false);
                return;
            }
            UIManager.Instance.SetPickUpText(true);
        }
        else
        {
            UIManager.Instance.SetPickUpText(false);
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
