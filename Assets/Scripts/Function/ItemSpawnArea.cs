using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnArea : MonoBehaviour
{
    [SerializeField] float detectFloorY;
    [SerializeField] int maxItemCount;
    [SerializeField] List<ItemBase> items = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SpawnItem(ItemBase item,Vector3 pos)
    {
        if(items.Count +1 > maxItemCount) return false;
        RaycastHit hit; 
        pos = transform.position + pos;
        if(Physics.Raycast(new Vector3(pos.x,pos.y+detectFloorY,pos.z), Vector3.down, out hit, detectFloorY + 1,LayerMask.GetMask("Ground")))
        {
            var spawnPos = hit.point + (Vector3.up * Mathf.Abs(item.transform.localScale.y));
            var spawnItem = Instantiate(item,spawnPos,item.transform.rotation);
            items.Add(spawnItem);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetMaxItemCount()
    {
        return maxItemCount;
    }

    public int GetCurrentItemCount()
    {
        return items.Count;
    }

    public Vector3 GetAreaSize()
    {
        return transform.localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position - ((Vector3.down * detectFloorY)/2), new Vector3(transform.localScale.x, detectFloorY, transform.localScale.z));
    }
}
