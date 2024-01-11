using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct SellInfo
{
    public string name;
    public int price;
}

public class SellUI : MonoBehaviour
{
    [SerializeField] Transform itemSellUI;
    
    [SerializeField] Text itemSellText;
    [SerializeField] Text totalPriceText;

    List<GameObject> textList = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableAllText()
    {
        for(int i = 0; i < textList.Count; i++)
        {
            Destroy(textList[i].gameObject);
        }
    }

    public void CreateItemPriceText(string text)
    {
        var ui = Instantiate(itemSellText,itemSellUI);
        ui.text = text;
        textList.Add(ui.gameObject);
    }

    public void SetTotalText(bool isActive, string text = "")
    {
        totalPriceText.gameObject.SetActive(isActive);
        if(isActive)
            totalPriceText.text = text;
    }
}
