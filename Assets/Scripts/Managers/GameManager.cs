using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public readonly static int ScanDistance = 15;

    public static int Money = 0;


    private int curHand = 0;
    
    public static int curInventory = 0;

    [Header("Systems")]
    public Storage storage;

    [Header("Objects")]
    public GameObject player;
    public Transform[] handPosArray;
    public Camera cam;

    [Header("Bases")]
    public ItemBase itemBase;

    [Header("UI")]
    public UI_ItemShow itemShow;

    private void Awake()
    {
        instance = this;
    }
    
    public static void SetMoney(int value)
    {
        Money = value;
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
            item.transform.parent = handPosArray[1];
            curHand = 1;
        } else
        {
            item.transform.parent = handPosArray[0];
            curHand = 0;
        }
        item.IsGrabbed = true;
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = Vector3.zero;

        player.GetComponent<PlayerInventory>().AddInventory(item);
    }

    public Transform DropItem()
    {

        //Transform dropItem = handPosArray[curHand].GetChild(0);
        Transform dropItem = PlayerInventory.inventory[curInventory].transform;

        dropItem.parent = null;
        dropItem.GetComponent<Rigidbody>().useGravity = true;
        dropItem.GetComponent<Rigidbody>().isKinematic = false;
        return dropItem;
    }

    public bool HasHoldItem()
    {
        return PlayerInventory.inventory[curInventory] != null;
        //return handPosArray[curHand].childCount > 0;
    }

    public void UpdateStorageItems()
    {
        storage.CheckItemsInStorageArea();
    }
}
