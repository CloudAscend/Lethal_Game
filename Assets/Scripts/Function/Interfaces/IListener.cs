using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event_Type
{
    //플레이어 이벤트
    PlayerSpawn=0,
    PlayerDeath=1,
    PlayerDamaged=2,
    PlayerItemPickUp=3,
    PlayerItemPickDown=4,
    //기타 이벤트
    EntityScan=100,
    ItemSold=101,

}

public interface IListener
{
    public void OnNotify(Event_Type type, Component sender, object param = null);
}
