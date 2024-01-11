using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearTrash : ItemBase
{
    public GameObject explosionPrefab;

    protected override void Start()
    {
        base.Start();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isThrew)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
