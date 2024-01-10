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
            if(item.IsGrabbed) return null;
            return item;
        }
        else
        {
            return null;
        }
    }
}
