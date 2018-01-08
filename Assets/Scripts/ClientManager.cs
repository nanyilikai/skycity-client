using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class ClientManager : Entity
{
    public override void __init__()
    {
        KBEngine.Event.registerIn("SetTurnFlag", this, "SetTurnFlag");
        KBEngine.Event.registerIn("UnitAttack", this, "UnitAttack");
    }

    public void SetAllUnit(Dictionary<string, object> infos)
    {
        KBEngine.Event.fireOut("SetAllUnit", new object[] { infos });
    }

    public void SetTurnUnit(Dictionary<string, object> infos)
    {
        UnitAttribute unitAttribute=new UnitAttribute(infos);
        KBEngine.Event.fireOut("SetTurnUnit", new object[] { unitAttribute });
    }

    public void SetTurnFlag(object first,object second)
    {
        cellCall("SetTurnFlag",new object[] { System.Convert.ToUInt16(first), System.Convert.ToUInt16(second) });
    }

    public void UnitAttack(Pair[] UVStack, bool flag)
    {
        List<object> first = new List<object>();
        List<object> second = new List<object>();
        foreach (Pair p in UVStack)
        {
            first.Add(System.Convert.ToUInt16(p.First));
            second.Add(System.Convert.ToUInt16(p.Second));
        }
        cellCall("UnitAttack", new object[] { first,second,flag});
    }

    public void UnitListUnderAttack(Dictionary<string, object> infos)
    {
        KBEngine.Event.fireOut("UnitListUnderAttack", new object[] { infos });
    }
}
