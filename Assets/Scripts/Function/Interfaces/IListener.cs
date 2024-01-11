using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event_Type
{
    //�÷��̾� �̺�Ʈ
    PlayerSpawn=0,
    PlayerDeath=1,
    PlayerDamaged=2,
    PlayerItemPickUp=3,
    PlayerItemPickDown=4,
    //��Ÿ �̺�Ʈ
    EntityScan=100,
    ItemSold=101,

}

public interface IListener
{
    public void OnNotify(Event_Type type, Component sender, object param = null);
}
