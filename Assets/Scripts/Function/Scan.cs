using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Scan : MonoBehaviour
{
    [SerializeField] Transform scanArea;

    private bool isScanning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UseScan();
    }

    private void UseScan()
    {
        if(Input.GetMouseButton(1) && !isScanning)
        {
            GameManager.instance.UpdateStorageItems();
            StartCoroutine(ScanEntity(0.5f));

        }
    }

    IEnumerator ScanEntity(float time)
    {
        isScanning = true;
        //scanArea.position = transform.position;
        float elapsedTime = 0;
        List<ItemBase> items = new List<ItemBase>();
        Collider[] hits;
        float radius = 0;
        while (elapsedTime <= time)
        {
            scanArea.localScale = Vector3.Lerp(scanArea.localScale,GameManager.ScanDistance * Vector3.one * 2,(elapsedTime/time));
            radius = Mathf.Lerp(radius,GameManager.ScanDistance, (elapsedTime / time));
            hits = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Item"));
            foreach(var hit in hits)
            {
                var item = hit.GetComponent<ItemBase>();
                if (!item.Render.isVisible) continue;
                if(!items.Contains(item))
                {
                    items.Add(item);
                    item.ScanUIOn();
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        EventManager.Instance.PostNotification(Event_Type.EntityScan, this, new ScanInfo
        {
            playerPos = GameManager.instance.player.transform.position
        });
        DisableScanArea();
    }

    private async void DisableScanArea()
    {
        await Task.Delay(500);
        scanArea.localScale = Vector3.zero;
        isScanning = false;

    }
}
