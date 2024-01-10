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
    private readonly Dictionary<Func<bool>, System.Action> _onKeyEvent = new();

    private ItemBase detectedItem;

    private void Start()
    {
        TryGetComponent(out playerMovement);

        _onKeyEvent.Add(() => Input.GetKeyDown(_interactKey.grabKey), PickUp);
        _onKeyEvent.Add(() => Input.GetKeyDown(_interactKey.dropKey), Drop);
        _onKeyEvent.Add(() => Input.GetKeyDown(_interactKey.throwKey), ThrowItem);
    }

    private void Update()
    {
        DetectItem();
        KeyEvent();
    }

    private void KeyEvent()
    {
        if (Input.anyKeyDown)
        {
            foreach (var keyEvent in _onKeyEvent)
            {
                if (keyEvent.Key.Invoke())
                {
                    keyEvent.Value.Invoke();
                }
            }
        }
    }

    private void DetectItem()
    {
        Debug.DrawRay(cam.position, cam.forward, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, detectDistance, whatIsItem))
        {
            UIManager.Instance.SetPickUpText(true);
            if (GameManager.instance.HasHoldItem()) return;

            detectedItem = hit.collider.GetComponent<ItemBase>();
        } else
        {
            UIManager.Instance.SetPickUpText(false);
        }
    }

    private void PickUp()
    {
        // var detectedItem = ItemDetection.GetDetectedItem();
        if (detectedItem == null)
        {
            return;
        }

        GameManager.instance.GetItem(detectedItem);
    }

    private void Drop()
    {
        if (GameManager.instance.HasHoldItem())
        {
            Transform dropItem = GameManager.instance.DropItem();
            dropItem.position = dropPos.position;
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
        }
    }
}
