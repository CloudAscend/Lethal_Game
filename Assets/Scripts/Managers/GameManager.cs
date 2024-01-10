using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public readonly static int ScanDistance = 15;

    private int curHand = 0;

    [Header("Systems")]
    [SerializeField] Storage storage;

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

    public void GetItem(ItemBase item)
    {
        if(item.grabType == ItemGrabType.Big)
        {
            item.transform.parent = handPosArray[1];
            curHand = 1;
        } else
        {
            item.transform.parent = handPosArray[0];
            curHand = 0;
        }
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localPosition = Vector3.zero;

        player.GetComponent<PlayerInventory>().AddInventory(item);
    }

    public Transform DropItem()
    {
        Transform dropItem = handPosArray[curHand].GetChild(0);
        dropItem.parent = null;
        dropItem.GetComponent<Rigidbody>().useGravity = true;
        dropItem.GetComponent<Rigidbody>().isKinematic = false;
        return dropItem;
    }

    public bool HasHoldItem()
    {
        return handPosArray[curHand].childCount > 0;
    }

    public void UpdateStorageItems()
    {
        storage.CheckItemsInStorageArea();
    }
}
