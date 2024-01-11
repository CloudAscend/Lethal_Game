using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class YieldReturnVariables
{
    private static readonly Dictionary<float, WaitForSeconds> _waitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float time)
    {
        WaitForSeconds waitForSeconds;
        if(_waitForSeconds.TryGetValue(time, out waitForSeconds) == false)
        {
            waitForSeconds = new WaitForSeconds(time);
            _waitForSeconds.Add(time, waitForSeconds);  
        }
        return waitForSeconds;
    }

}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] Canvas scanCanvas;

    [SerializeField] private Text pickUpText;
    [SerializeField] private ScanUI scanUI;
    [SerializeField] private StorageUI storageUI;
    [SerializeField] private Transform storagePanel;
    [SerializeField] private TotalUI totalUI;
    [SerializeField] private SellUI sellUI;

    [SerializeField] private float uiSeperation;

    List<StorageUI> storageList = new();
    Queue<IEnumerator> storageUIQueue = new();

    StorageUI curStorageUI;

    bool isStorageListUp = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInteractText(bool isActive, string text = "")
    {
        pickUpText.gameObject.SetActive(isActive);
        if(isActive) 
            pickUpText.text = text;
    }

    public void SetTotalUIText(bool isActive, string text = "")
    {
        totalUI.gameObject.SetActive(isActive);
        if(isActive)
            totalUI.SetTotalText(text);
    }

    public void ShowSellItemUI(List<SellInfo> data,float totalPrice)
    {
        List<SellInfo> list = new List<SellInfo>();
        Dictionary<string, int> itemCounts = new();

        foreach(var item in data)
        {
            
            if(itemCounts.ContainsKey(item.name))
            {
                itemCounts[item.name]++;
            } else
            {
                itemCounts.Add(item.name, 1);
                list.Add(item);
            }
        }
        sellUI.gameObject.SetActive(true);
        foreach(var item in list)
        {
            sellUI.CreateItemPriceText(
                $"{item.name}{(itemCounts[item.name] > 1 ? $"x{itemCounts[item.name]} : {item.price * itemCounts[item.name]}" : $" : {item.price}")}"
            );
        }
        sellUI.SetTotalText(true, $"Total : {totalPrice}");
        StartCoroutine(HideSellItemUI());
    }

    IEnumerator HideSellItemUI()
    {
        yield return YieldReturnVariables.WaitForSeconds(2.5f);
        sellUI.DisableAllText();
        sellUI.SetTotalText(false);

        sellUI.gameObject.SetActive(false);
    }


    private void StartStorageListUp()
    {
        
        if (!isStorageListUp)
            StartCoroutine(StorageListUp(curStorageUI));
        else
            storageUIQueue.Enqueue(StorageListUp(curStorageUI));
    }

    IEnumerator StorageListUp(StorageUI ui)
    {
        isStorageListUp = true;
        for(int i = storageList.Count-1; i > 0; i--)
        {
            storageList[i].TryGetComponent(out RectTransform rectTransform);
            rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + uiSeperation,0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        isStorageListUp = false;
        //if(storageUIQueue.Count > 0) StartCoroutine(storageUIQueue.Dequeue());

    }

    public void RemoveStorageUI(StorageUI ui)
    {
        if(storageList.Contains(ui))
        {
            storageList.Remove(ui);
        }
    }

    public StorageUI CreateStorageUI(ItemBase item)
    {
        if(GameManager.instance.storage.CheckDupeItem(item)) return null;
        var ui = Instantiate(storageUI, storagePanel);
        ui.transform.localPosition = Vector3.zero;
        ui.Init(item);
        curStorageUI = ui;
        storageList.Add(ui);
        StartStorageListUp();
        return ui;
    }

    public ScanUI CreateScanUI(Transform target)
    {
        var ui = Instantiate(scanUI, scanCanvas.transform);
        ui.transform.position = target.position;
        return ui;
    }


}
