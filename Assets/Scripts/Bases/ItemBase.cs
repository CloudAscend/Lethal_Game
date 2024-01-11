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

public enum ItemInteractType
{
    PickUp,
    Drop,
    Throw,
    Use,
    Sell,
}

public class ItemBase : MonoBehaviour
{
    public string id;
    
    public int price;
    public int weight;

    public ItemType itemType = ItemType.Normal;
    public ItemGrabType grabType = ItemGrabType.Small;

    protected ScanUI ui;

    protected Renderer render;

    protected Rigidbody rigid;

    protected bool isGrabbed = false;
    protected bool isThrew = false;
    protected bool isReadyToSell = false;

    protected IItemInteractable interactable;

    public bool IsGrabbed
    {
        get
        {
            if(IsOnGround() && isGrabbed)
            {
                isGrabbed = false;
            } 
            return isGrabbed;
        }
        set
        {
            isGrabbed = value;
        }
    }

    public bool IsThrew
    {
        get
        {
            if (IsOnGround() && isThrew)
            {
                isThrew = false;
            }
            return isThrew;
        }
        set
        {
            isThrew = value;
        }
    }

    public bool IsReadyToSell
    {
        get
        {
            return isReadyToSell;
        }

        set
        {
            isReadyToSell = value;
        }
        
    }

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
        Init(); 
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

    private void Update()
    {
        if (isGrabbed && IsOnGround())
        {
            isGrabbed = false;
            isThrew = false;
        }
        
    }

    protected void Init()
    {
        if(TryGetComponent(out Rigidbody rigid) == false)
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    public bool CheckPickUpItem()
    {
        Debug.Log(isGrabbed + " " + isReadyToSell + " " + IsOnGround());
        if(isGrabbed) return false;
        //if(!IsOnGround()) return false;  
        if(IsReadyToSell) return false;
        return true;
    }

    public bool IsOnGround()
    {
        float dist = transform.localScale.y / 2 + 0.25f;
        Debug.DrawRay(transform.position, Vector3.down, Color.green, 1);
        return Physics.Raycast(transform.position, -Vector3.down,dist);
    }

    public void ScanUIOn()
    {
        ScanUI ui = UIManager.Instance.CreateScanUI(transform);
        ui.Init(this);
        this.ui = ui;
    }

    public void ScanUIOff()
    {
       if(ui != null)
        {
            Destroy(ui.gameObject);
        }
    }

    public virtual void Interact(IItemInteractable interactable)
    {
        this.interactable = interactable;
        if(interactable != null)
        {
            this.interactable.Interact(this);
        }
    }


}
