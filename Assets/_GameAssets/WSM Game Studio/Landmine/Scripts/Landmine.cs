using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSMGameStudio.Bombs
{
    public class Landmine : MonoBehaviour
    {
        public GameObject explosionPrefab;
        [SerializeField] Vector3 offsetPos;
        [SerializeField] Vector3 offsetSize;


        private void Update()
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + offsetPos, offsetSize,Quaternion.identity);
            foreach (Collider collider in colliders)
            {
                string layerName = LayerMask.LayerToName(collider.gameObject.layer);
                if (layerName == "Item" || layerName == "Player")
                {
                    if (collider.gameObject == gameObject) break;
                    Explode();
                    break;
                }
            }
        }

        private void Explode()
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    } 
}
