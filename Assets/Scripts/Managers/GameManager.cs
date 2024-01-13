using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public readonly static int ScanDistance = 15;

    public static int Money = 0;

    [Header("Systems")]
    public Storage storage;
    public SellArea sellArea;

    [Header("Objects")]
    public GameObject player;
    public Camera cam;
    public GameObject callButton;

    [Header("Bases")]
    public ItemBase itemBase;

    [Header("UI")]
    public UI_ItemShow itemShow;

    private void Awake()
    {
        instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public static void SetMoney(int value)
    {
        Money = value;
    }

    public void SellItems()
    {
        sellArea.SellAllItems();
    }
         
    public void UpdateStorageItems()
    {
        storage.CheckItemsInStorageArea();
    }
}
