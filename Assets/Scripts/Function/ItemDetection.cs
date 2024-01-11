using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetection : MonoBehaviour
{
    [SerializeField] private Transform cam;

    public ItemBase DetectItem(float detectDistance)
    {
        Debug.DrawRay(cam.position, cam.forward, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, detectDistance, LayerMask.GetMask("Item")))
        {
            if(hit.collider.TryGetComponent(out ItemBase item) == false)
            {
                return null;
            }
            if(!item.CheckPickUpItem()) return null;
            return item;
        }
        else
        {
            return null;
        }
    }

    public bool IsDetected(float detectDistance,LayerMask layerMask)
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, detectDistance, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsDetected(float detectDistance, GameObject target)
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, detectDistance))
        {
            if(target != null && hit.collider.gameObject.Equals(target))
                return true;
            return false;
        }
        else
        {
            return false;
        }
    }

    public bool CheckSellArea(float detectDistance)
    {
        Debug.DrawRay(cam.position, cam.forward, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, detectDistance, LayerMask.GetMask("SellArea")))
        {
            if (hit.collider.TryGetComponent(out SellArea area) == false)
            {
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public SellArea DetectSellArea(float detectDistance)
    {
        Debug.DrawRay(cam.position, cam.forward, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, detectDistance, LayerMask.GetMask("SellArea")))
        {
            if (hit.collider.TryGetComponent(out SellArea area) == false)
            {
                return null;
            }
            return area;
        }
        else
        {
            return null;
        }
    }
}
