using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalUI : MonoBehaviour
{
    [SerializeField] Text totalText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTotalText(string text)
    {
        totalText.text = text;
    }
}
