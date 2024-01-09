using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Objects")]
    public GameObject player;
    public Transform rHand;

    [Header("Bases")]
    public ItemBase itemBase;

    [Header("UI")]
    public UI_ItemShow itemShow;

    private void Awake()
    {
        instance = this;
    }

    public void GetItem(Transform item)
    {
        item.parent = rHand;
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.localPosition = Vector3.zero;
    }

    public Transform DropItem()
    {
        Transform dropItem = rHand.GetChild(0);
        dropItem.parent = null;
        dropItem.GetComponent<Rigidbody>().useGravity = true;
        dropItem.GetComponent<Rigidbody>().isKinematic = false;
        return dropItem;
    }

    public bool HasHoldItem()
    {
        return rHand.childCount > 0;
    }
}
