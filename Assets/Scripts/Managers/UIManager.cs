using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//public class YieldReturnVariables
//{
//    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new();
    
//}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] Canvas scanCanvas;

    [SerializeField] private Text pickUpText;
    [SerializeField] private ScanUI scanUI;
    [SerializeField] private StorageUI storageUI;
    [SerializeField] private Transform storagePanel;

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

    public void SetPickUpText(bool isActive)
    {
        pickUpText.gameObject.SetActive(isActive);
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
