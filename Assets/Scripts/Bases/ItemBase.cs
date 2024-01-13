using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

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

    public Vector3 grabRotOffset;
    public Vector3 grabPosOffset;

    protected ScanUI ui;

    protected Renderer render;

    protected Rigidbody rigid;

    protected bool isGrabbed = false;
    protected bool isThrew = false;
    protected bool isReadyToSell = false;

    public Vector3 customRot = Vector3.zero;

    public bool isGrab = false;

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

    protected virtual void Update()
    {
        if (isGrabbed && IsOnGround())
        {
            isGrabbed = false;
            isThrew = false;
        }
        if(isGrab)
        {
            Transform hand = PlayerInventory.Instance.GetHand();
            transform.position = grabPosOffset + hand.position;
            Vector3 rot = GameManager.instance.cam.transform.eulerAngles;
            
            transform.localRotation = Quaternion.Euler(grabRotOffset + rot + customRot);
            Debug.Log(transform.rotation.eulerAngles);
        }
        Interact();
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
        //Debug.Log(isGrabbed + " " + isReadyToSell + " " + IsOnGround());
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

    public virtual void SetInteract(IItemInteractable interactable, object param = null)
    {
        this.interactable = interactable;
        interactable.Init(param);
    }

    protected virtual void Interact()
    {
        if(interactable != null)
        {
            this.interactable.Interact(this);
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color color = Color.green;
       // var view = SceneView.currentDrawingSceneView;
        Vector3 pos = transform.position + grabPosOffset;
        //if (view != null)
        //{
            Gizmos.color = new Color(color.r, color.g, color.b, 0.3f);
            Gizmos.DrawSphere(pos, 0.1f );
            
        //}
       
    }
#endif
}
