using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;


public class TurnManager : MonoBehaviour {
    private SqlAccess sql;
    private LinkedList<UnitAttribute> unit=new LinkedList<UnitAttribute>();
    private GameManager manager;
    private bool gameBegin = true;
    private bool turnFlag = true;
    private UnitAttribute turnUnit;

    private void Awake()
    {
        /*manager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        sql = new SqlAccess("localhost", "chessdata", "chessclient", "mychessdata", "3306");

        DataSet ds;
        UnitAttribute unitAttribute;
        ds =sql.Select("unit", new string[] { "*" }, new string[] { "id" }, new string[] { "=" }, new string[] { "1" });
        if(ds!=null)
        {
            unitAttribute = new UnitAttribute(ds);
            unitAttribute.Ascription = UnitAttribute.UnitAscription.red;
            unitAttribute.UV = new Pair(2, 0);
            unitAttribute.Higth = 0.5f;
            manager.SetUnit(unitAttribute);
            InserUnit(unitAttribute);
        }
        ds = sql.Select("unit", new string[] { "*" }, new string[] { "id" }, new string[] { "=" }, new string[] { "1" });
        if (ds != null)
        {
            unitAttribute = new UnitAttribute(ds);
            unitAttribute.Ascription = UnitAttribute.UnitAscription.blue;
            unitAttribute.UV = new Pair(2, 9);
            unitAttribute.Higth = 0.5f;
            manager.SetUnit(unitAttribute);
            InserUnit(unitAttribute);
        }*/
        /*unitAttribute = new UnitAttribute();
        unitAttribute.ResourceName = "Prefabs/Long Range Magic";
        unitAttribute.Ascription = UnitAttribute.UnitAscription.blue;
        unitAttribute.UV = new Pair(6, 9);
        unitAttribute.Higth = 0.5f;
        chessUnit = (GameObject)Resources.Load("Prefabs/Long Range Magic");
        unitAttribute.HP = chessUnit.GetComponent<UnitControler>().attribute.HP;
        unitAttribute.speed = chessUnit.GetComponent<UnitControler>().attribute.speed;
        unitAttribute.attackStandard = chessUnit.GetComponent<UnitControler>().attribute.attackStandard;
        unitAttribute.attackDeviation = chessUnit.GetComponent<UnitControler>().attribute.attackDeviation;
        unitAttribute.longRange = chessUnit.GetComponent<UnitControler>().attribute.longRange;
        unitAttribute.longRangeStandard = chessUnit.GetComponent<UnitControler>().attribute.longRangeStandard;
        unitAttribute.longRangeDeviation= chessUnit.GetComponent<UnitControler>().attribute.longRangeDeviation;
        unitAttribute.attackTypeName = chessUnit.GetComponent<UnitControler>().attribute.attackTypeName;
        unitAttribute.luck = chessUnit.GetComponent<UnitControler>().attribute.luck;
        unitAttribute.defense = chessUnit.GetComponent<UnitControler>().attribute.defense;
        unitAttribute.magicDefense = chessUnit.GetComponent<UnitControler>().attribute.magicDefense;
        manager.SetUnit(unitAttribute);
        InserUnit(unitAttribute);
        unitAttribute = new UnitAttribute();
        unitAttribute.ResourceName = "Prefabs/Long Range Magic";
        unitAttribute.Ascription = UnitAttribute.UnitAscription.red;
        unitAttribute.UV = new Pair(6, 0);
        unitAttribute.Higth = 0.5f;
        chessUnit = (GameObject)Resources.Load("Prefabs/Long Range Magic");
        unitAttribute.HP = chessUnit.GetComponent<UnitControler>().attribute.HP;
        unitAttribute.speed = chessUnit.GetComponent<UnitControler>().attribute.speed;
        unitAttribute.attackStandard = chessUnit.GetComponent<UnitControler>().attribute.attackStandard;
        unitAttribute.attackDeviation = chessUnit.GetComponent<UnitControler>().attribute.attackDeviation;
        unitAttribute.longRange = chessUnit.GetComponent<UnitControler>().attribute.longRange;
        unitAttribute.longRangeStandard = chessUnit.GetComponent<UnitControler>().attribute.longRangeStandard;
        unitAttribute.longRangeDeviation = chessUnit.GetComponent<UnitControler>().attribute.longRangeDeviation;
        unitAttribute.attackTypeName = chessUnit.GetComponent<UnitControler>().attribute.attackTypeName;
        unitAttribute.luck = chessUnit.GetComponent<UnitControler>().attribute.luck;
        unitAttribute.defense = chessUnit.GetComponent<UnitControler>().attribute.defense;
        unitAttribute.magicDefense = chessUnit.GetComponent<UnitControler>().attribute.magicDefense;
        manager.SetUnit(unitAttribute);
        InserUnit(unitAttribute);*/
    }

    // Use this for initialization
    void Start () {
        gameBegin = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (turnFlag)
        {
            turnFlag = false;
            if (unit.First != null)
            {
                turnUnit = unit.First.Value;
                manager.SetTurnUnit(turnUnit);
                unit.RemoveFirst();
            }
        }
	}

    //棋子插入到时间排序链表
    private void InserUnit(UnitAttribute newUnit)
    {
        newUnit.AddSpeedCount();
        LinkedListNode<UnitAttribute> node = unit.First;
        while (node != null)
        {
            if (newUnit.SpeedCount < node.Value.SpeedCount)
            {
                unit.AddBefore(node, newUnit);
                break;
            }
            else if(gameBegin && newUnit.SpeedCount== node.Value.SpeedCount)  //开始时
            {
                if (Random.Range(0, 2) == 0)
                {
                    unit.AddBefore(node, newUnit);
                    break;
                }
            }
            node = node.Next;
        }
        if (node == null)
            unit.AddLast(newUnit);
    }

    //回合结束，处理本回合的棋子，允许运行下回合
    public void SetTurnFlag(Pair UV)
    {
        turnFlag = true;

        turnUnit.UV = UV;
        InserUnit(turnUnit);
    }

    //处理棋子攻击
    public void UnitAttack(Pair[] UVStack, bool flag)
    {
        bool first = true;
        LinkedListNode<UnitAttribute> node;
        int damage;
        bool deadFlag;
        foreach(Pair UV in UVStack)
        {
            deadFlag = false;
            node = unit.First;
            while (node != null)
            {
                if (node.Value.UV.Equals(UV))
                    break;
                node = node.Next;
            }
            UnitAttribute attribute = node.Value;
            if (flag)
            {
                damage = Random.Range(turnUnit.attackStandard - turnUnit.attackDeviation, turnUnit.attackStandard + turnUnit.attackDeviation + 1);
                damage = subDefense(damage, attribute.defense);
            }
            else
            {
                damage = Random.Range(turnUnit.longRangeStandard - turnUnit.longRangeDeviation, turnUnit.longRangeStandard + turnUnit.longRangeDeviation + 1);
                if (turnUnit.attackTypeName.Equals("LongRangeAttack"))
                {
                    int distance = turnUnit.UV.GetDistance(UV);
                    if (distance > turnUnit.longRange)
                    {
                        if (distance > 2 * turnUnit.longRange)
                            damage *= 3 / 10;
                        else
                            damage = (int)(damage*(1.7f-distance * 7 / 10 / turnUnit.longRange));
                        damage = subDefense(damage,attribute.defense);
                    }
                }
                else if (turnUnit.attackTypeName.Equals("MagicAttack"))
                {
                    if (first)
                    {
                        damage++;
                        first = false;
                        damage = subMagicDefense(damage, attribute.magicDefense);
                    }
                }
            }
            damage = addLuckyDamage(damage);
            attribute.HP -= damage;
            if (attribute.HP <= 0)
            {
                unit.Remove(node);
                deadFlag = true;
            }
            else
                node.Value = attribute;   
            manager.UnitUnderAttack(damage,deadFlag);
        }      
    }

    private int subDefense(int damage,int defense)
    {
        return damage - defense;
    }

    private int subMagicDefense(int damage,int magicDefense)
    {
        return damage * (10 - magicDefense) / 10;
    }

    private int addLuckyDamage(int damage)
    {
        return Random.Range(0,10)>=turnUnit.luck?damage:(int)(damage*1.5f);
    }
}
