using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SellArea : MonoBehaviour
{
    [SerializeField] List<ItemBase> sellItems = new();

    private List<SellInfo> sellItemInfoLists = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SellAllItems()
    {
        
        sellItemInfoLists.Clear();
        int price = 0;
        foreach(ItemBase item in sellItems)
        {
            if(item != null)
            {
                price += item.price;
                sellItemInfoLists.Add(
                    new SellInfo { name = item.id, price = item.price }    
                );
                Destroy(item.gameObject);
            }
        }
        GameManager.SetMoney(GameManager.Money + price);
        UIManager.Instance.ShowSellItemUI(sellItemInfoLists,price);
        sellItems.Clear();

    }

    public void ItemSell(ItemBase item)
    {
        float x, y, z;
        x = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
        y = Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2);
        z = Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2);

        Vector3 pos = transform.position + new Vector3(x, y, z);
        Vector3 rot = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        item.IsReadyToSell = true;
        item.transform.DORotate(rot, 0.12f).SetEase(Ease.Linear);
        item.transform.DOMove(pos,0.12f).SetEase(Ease.Linear);
        sellItems.Add(item);
    }
}
