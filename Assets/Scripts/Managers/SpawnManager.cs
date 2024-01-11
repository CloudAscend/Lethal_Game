using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] List<ItemBase> spawnItems = new();

    [SerializeField] List<ItemSpawnArea> itemSpawnList = new();

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
        SpawnItems();
    }

    private void SpawnItems()
    {
        foreach(ItemSpawnArea area in itemSpawnList)
        {
            for(int i = 0; i < area.GetMaxItemCount(); i++)
            {
                float x = Random.Range(-area.GetAreaSize().x / 2, area.GetAreaSize().x / 2);
                float z = Random.Range(-area.GetAreaSize().z / 2, area.GetAreaSize().z / 2);

                Vector3 spawnPos = new Vector3(x, 0, z);
                ItemBase spawnItem = spawnItems[Random.Range(0, spawnItems.Count)];
                area.SpawnItem(spawnItem, spawnPos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
