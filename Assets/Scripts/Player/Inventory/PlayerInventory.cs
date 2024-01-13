using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInventory : PlayerBase
{
    public static PlayerInventory Instance { get; private set; }
    
    public Sprite inventoryImage;
    public int price;
    public int weight;

    public static ItemBase[] inventory;
    [SerializeField] private Image[] inventoryTrans;
    public static int invenValue;

    private int curHand = 0;
    [SerializeField] Transform[] handPosPivot;
    public Transform[] handPosArray;
    public static int curInventory = 0;
    private GameObject player;

    private float size;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
        player = GameManager.instance.player;
    }

    private void Awake()
    {
        inventory = new ItemBase[4];
        size = 1;
    }

    private void Update()
    {
        ChooseInventory();
        //for(int i = 0; i < handPosArray.Length; i++)
        //{
        //    handPosArray[i].position = handPosPivot[i].position;
        //    handPosArray[i].rotation = GameManager.instance.cam.transform.rotation;
        //}
    }

    private void ChooseInventory()
    {
        if      (Input.GetKey(KeyCode.Alpha1)) invenValue = 0;
        else if (Input.GetKey(KeyCode.Alpha2)) invenValue = 1;
        else if (Input.GetKey(KeyCode.Alpha3)) invenValue = 2;
        else if (Input.GetKey(KeyCode.Alpha4)) invenValue = 3;

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        invenValue %= 4;

        if (wheelInput > 0)
        {
            invenValue++;
        }
        else if (wheelInput < 0)
        {
            invenValue--;
            if (invenValue < 0) invenValue = 3;
        }
        else
        {
            invenValue %= 4;
        }

        invenValue %= 4;
        
        for (int i = 0; i < 4; i++)
        {
            Transform t = inventoryTrans[i].transform.parent;
            if (i == invenValue)
            {
                t.DOScale(1.25f, 0.1f);
            }
            else
            {
                t.DOScale(1f, 0.1f);
            }    
        }
        ChangeInventory(invenValue);
    }

    public void AddInventory(ItemBase item)
    {
        if (HasHoldItem())
        {
            return;
        }

        if (inventory[invenValue] == null)
        {
            inventoryTrans[invenValue].gameObject.SetActive(true);
            inventoryTrans[invenValue].sprite = inventoryImage;
        }
        inventory[invenValue] = item;

        for (int index = 0; index < inventoryTrans.Length; index++)
        {
            if (index != invenValue && inventory[index] != null)
                inventory[index].gameObject.SetActive(false);
        }

        GameManager.instance.player.GetComponent<PlayerMovement>().AddInventoryWeight(item.weight);
    }

    public void RemoveInventory()
    {

        GameManager.instance.player.GetComponent<PlayerMovement>().RemoveInventoryWeight(inventory[invenValue].weight);
        if (inventory[invenValue] != null)
        {
            inventoryTrans[invenValue].gameObject.SetActive(false);
            inventoryTrans[invenValue].sprite = null;
        }
        inventory[invenValue] = null;
    }

    public bool IsInventoryFull()
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] == null) return false;
        }
        return true;    
    }

    private void ChangeInventory(int inventoryValue)
    {
        curInventory = inventoryValue;

        for (int index = 0; index < inventoryTrans.Length; index++)
        {
            bool objectActive = index == inventoryValue == true;
            if (inventory[index] != null)
            {
                if (objectActive)
                {
                    if (inventory[index].grabType == ItemGrabType.Big)
                    {
                        //item.transform.parent = handPosArray[1];
                        curHand = 1;
                    }
                    else
                    {
                        //item.transform.parent = handPosArray[0];
                        curHand = 0;
                    }
                }
                
                inventory[index].gameObject.SetActive(objectActive);

            }
        }
    }

    private bool CheckInventory(int value)
    {
        return inventory[value] == null == true;
    }

    public Transform GetHand()
    {
        return handPosArray[curHand];
    }

    public ItemBase GetHeldItem()
    {
        handPosArray[curHand].GetChild(0).TryGetComponent(out ItemBase item);
        return item;
    }

    public void GetItem(ItemBase item)
    {
        if (HasHoldItem())
        {
            return;
        }

        if (item.grabType == ItemGrabType.Big)
        {
            //item.transform.parent = handPosArray[1];
            curHand = 1;
        }
        else
        {
            //item.transform.parent = handPosArray[0];
            curHand = 0;
        }
        item.isGrab = true;
        item.IsGrabbed = true;
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = Vector3.zero;

        player.GetComponent<PlayerInventory>().AddInventory(item);
    }

    public ItemBase DropItem()
    {

        //Transform dropItem = handPosArray[curHand].GetChild(0);
        Transform dropItem = inventory[curInventory].transform;

        //dropItem.parent = null;
        dropItem.GetComponent<Rigidbody>().useGravity = true;
        dropItem.GetComponent<Rigidbody>().isKinematic = false;
        dropItem.TryGetComponent(out ItemBase item);
        item.isGrab = false;
        return item;
    }

    public bool HasHoldItem()
    {
        return inventory[curInventory] != null;
        //return handPosArray[curHand].childCount > 0;
    }

}
