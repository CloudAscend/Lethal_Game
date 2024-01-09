using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] Canvas scanCanvas;

    [SerializeField] private Text pickUpText;
    [SerializeField] private ScanUI scanUI;

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

    public ScanUI CreateScanUI(Transform target)
    {
        var ui = Instantiate(scanUI, scanCanvas.transform);
        ui.transform.position = target.position;
        return ui;
    }
}
