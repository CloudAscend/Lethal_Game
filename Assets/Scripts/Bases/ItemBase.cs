using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ScanInfo
{
    public Vector3 playerPos;
}

public enum ItemGrabType
{
    Small,
    Big
}

public enum ItemType
{
    Normal,
    Trap,
    Weapon,
    Useable
}


public class ItemBase : MonoBehaviour
{
    public int price;
    public int weight;

    public ItemType itemType = ItemType.Normal;
    public ItemGrabType grabType = ItemGrabType.Small;

    private ScanUI ui;

    private Renderer render;

    private Rigidbody rigid;

    public Renderer Render
    {
        get 
        { 
            if (render == null) 
                render = GetComponent<Renderer>(); 
            return render; 
        }
    }

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.mass = weight;

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
        this.ui = ui;
    }

    protected virtual void Interact()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            Interact();
    }


}
