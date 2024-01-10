using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    // Start is called before the first frame update
    public List<ItemBase> StorageItems 
    { 
        get 
        { 
            return curStorageItems; 
        } 
    }
         
    private List<ItemBase> curStorageItems = new();


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }

    private void DetectItemInStorageArea(ItemBase item)
    {
        if (!curStorageItems.Contains(item))
        {
            UIManager.Instance.CreateStorageUI(item);
            curStorageItems.Add(item);
        }
    }

    public void CheckItemsInStorageArea()
    {
        List<ItemBase> removeList = new List<ItemBase>();
        foreach(ItemBase item in curStorageItems)
        {
            if(item.Equals(null) || !item.gameObject.activeSelf)
            {
                removeList.Add(item);
            }
        }
        foreach(ItemBase item in removeList)
        {
            curStorageItems.Remove(item);
        }
    }

    public bool CheckDupeItem(ItemBase item)
    {
        if (StorageItems.Contains(item))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (LayerMask.LayerToName(other.gameObject.layer) == "Item")
        {
            ItemBase item;
            other.TryGetComponent(out item);
            if (CheckDupeItem(item)) return;    
            CheckItemsInStorageArea();
            DetectItemInStorageArea(item);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Item")
        {
            ItemBase item;
            other.TryGetComponent(out item);
            if(!CheckDupeItem(item)) return;
            curStorageItems.Remove(item);
            CheckItemsInStorageArea();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0, 0.3f);
        try
        {
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
        catch
        {
            return;
        }
    }
}
