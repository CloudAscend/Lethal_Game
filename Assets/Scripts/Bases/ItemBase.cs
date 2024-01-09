using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ScanInfo
{
    public Vector3 playerPos;
}

public class ItemBase : MonoBehaviour
{
    public int price;
    public int weight;

    protected virtual void Start()
    {
        //EventManager.Instance.AddListener(Event_Type.EntityScan, this);
    }

    //public void OnNotify(Event_Type type, Component sender, object param = null)
    //{
    //    if(type == Event_Type.EntityScan)
    //    {
    //        Debug.Log("Scanned " + transform.name);
    //        if(!param.Equals(null) && param.GetType().Equals(typeof(ScanInfo)))
    //        {
    //            ScanInfo scanInfo = (ScanInfo)param;
    //            if(Vector3.Distance(scanInfo.playerPos,transform.position) < GameManager.ScanDistance)
    //            {
    //                ScanUI ui = UIManager.Instance.CreateScanUI(transform);
    //                ui.Init(this);
    //            }
    //        }
    //    }
    //}

    public void ScanUIOn()
    {
        ScanUI ui = UIManager.Instance.CreateScanUI(transform);
        ui.Init(this);
    }

    protected virtual void Interact()
    {
        //GameManager.instance.itemShow.
        this.GetComponent<Collider>().enabled = false;
        transform.position = GameManager.instance.rHand.position;
        transform.parent = GameManager.instance.rHand.transform;
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            Interact();
    }
}
