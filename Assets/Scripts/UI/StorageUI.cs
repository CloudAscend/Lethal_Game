using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StorageUI : MonoBehaviour
{
    [SerializeField] float livingTime = 3;

    [SerializeField] Text nameText;
    [SerializeField] Text priceText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(ItemBase item)
    {
        nameText.text = item.name;
        priceText.text = $"{item.price}$";
        Invoke("DisableUI", livingTime);
    }

    private void DisableUI()
    {
        UIManager.Instance.RemoveStorageUI(this);
        Destroy(gameObject);
    }
}
