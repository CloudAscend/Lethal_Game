using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Objects")]
    public GameObject player;
    public Transform rHand;

    [Header("Bases")]
    public ItemBase itemBase;

    [Header("UI")]
    public UI_ItemShow itemShow;

    public void Awake()
    {
        instance = this;
    }
}
