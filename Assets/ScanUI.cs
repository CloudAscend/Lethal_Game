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

    // Start is called before the first frame update
    void Start()
    {
        Scaned();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GameManager.instance.cam.transform.position);
        transform.position = item.transform.position;
    }

    public void Init(ItemBase item)
    {
        this.item = item;
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
