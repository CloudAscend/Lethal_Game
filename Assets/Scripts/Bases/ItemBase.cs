using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public int price;
    public int weight;
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
