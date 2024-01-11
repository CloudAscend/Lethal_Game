using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class ScanUI : MonoBehaviour
{
    [SerializeField] Transform nameInfo;
    [SerializeField] Transform priceInfo;

    [SerializeField] Text nameText;
    [SerializeField] Text priceText;

    ItemBase item;

    ItemTypeEvents typeEvents = new();

    // Start is called before the first frame update
    void Start()
    {
        Scaned();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = GameManager.instance.cam.transform.rotation;
        transform.position = item.transform.position;
    }

    public void Init(ItemBase item)
    {
        this.item = item;
        if(TryGetComponent(out Image img1) == false)
        {
            img1 = gameObject.AddComponent<Image>();
        }
        if(nameInfo.TryGetComponent(out Image img2) == false)
        {
            img2 = nameInfo.gameObject.AddComponent<Image>();
        }
        if (priceInfo.TryGetComponent(out Image img3) == false)
        {
            img3 = priceInfo.gameObject.AddComponent<Image>();
        }
        Color color = typeEvents.GetItemScanColor(item.itemType);
        img1.color = color;
        img2.color = color;
        img3.color = color;
    }

    private void Scaned()
    {
        transform.localScale = Vector3.zero;
        nameInfo.gameObject.SetActive(false);
        priceInfo.gameObject.SetActive(false);
        transform.DOScale(1, 0.2f).OnComplete(() => SetUI());
    }

    private void SetUI()
    {
        nameInfo.gameObject.SetActive(true);
        priceInfo.gameObject.SetActive(true);
        nameText.text = item.name;
        priceText.text = $"АЁАн : {item.price}";  
        DisableUI();
    }

    private void DisableUI()
    {
        Destroy(gameObject, 3f);
    }
    
} 
