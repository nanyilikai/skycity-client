using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

[System.Serializable]
public class UnitAttribute {
    public enum UnitAscription
    {
        red = 0,
        blue
    }

    public enum AttackType
    {
        melee=0,
        longRange
    }

    private string resourceName;
    private UnitAscription ascription;
    private Pair uv;
    private float higth;

    public int maxHP;
    public int speed;
    public int moveLenth;

    [SerializeField,HeaderAttribute("AttackType and value")]
    public AttackType attackType;
    public int attackStandard;
    public int attackDeviation;
    public int longRange;
    public int longRangeStandard;
    public int longRangeDeviation;
    public string attackTypeName;

    public int defense;
    public int magicDefense;
    public int luck;

    public int HP;

    public UnitAttribute(DataSet ds)
    {
        DataRow row = ds.Tables[0].Rows[0];
        resourceName = row[1].ToString();
        maxHP = (int)row[2];
        speed = (int)row[3];
        moveLenth = (int)row[4];
        attackType = (AttackType)row[5];
        attackStandard = (int)row[6];
        attackDeviation = (int)row[7];
        if (attackType == AttackType.longRange)
        {
            longRange = (int)row[8];
            longRangeStandard = (int)row[9];
            longRangeDeviation = (int)row[10];
            attackTypeName = row[11].ToString();
        }
        defense = (int)row[12];
        magicDefense = (int)row[13];
        luck = (int)row[14];
        HP = maxHP;
    }

    public UnitAttribute(Dictionary<string, object> ds)
    {
        resourceName = ds["resourceName"].ToString();
        uv = new Pair(System.Convert.ToInt32(ds["UValue"]), System.Convert.ToInt32(ds["VValue"])); 
        ascription = (UnitAscription)System.Convert.ToInt32(ds["ascription"]);
        higth = (float)ds["higth"];
        maxHP = System.Convert.ToInt32(ds["maxHP"]);
        speed = System.Convert.ToInt32(ds["speed"]);
        moveLenth = System.Convert.ToInt32(ds["moveLenth"]);
        attackType = (AttackType)System.Convert.ToInt32(ds["attackType"]);
        attackStandard = System.Convert.ToInt32(ds["attackStandard"]);
        attackDeviation = System.Convert.ToInt32(ds["attackDeviation"]);
        if (attackType == AttackType.longRange)
        {
            longRange = System.Convert.ToInt32(ds["longRange"]);
            longRangeStandard = System.Convert.ToInt32(ds["longRangeStandard"]);
            longRangeDeviation = System.Convert.ToInt32(ds["longRangeDeviation"]);
            attackTypeName = ds["attackTypeName"].ToString();
        }
        defense = System.Convert.ToInt32(ds["defense"]);
        magicDefense = System.Convert.ToInt32(ds["magicDefense"]);
        luck = System.Convert.ToInt32(ds["luck"]);
        HP = maxHP;
    }

    private float speedCount=0;
    public void AddSpeedCount()
    {
        speedCount += 100 / (float)speed;
    }

    public float SpeedCount
    {
        get { return speedCount; }
    }

    public string ResourceName
    {
        set { resourceName = value; }
        get { return resourceName; }
    }

    public UnitAscription Ascription
    {
        set { ascription = value; }
        get { return ascription; }
    }

    public float Higth
    {
        set { higth = value; }
        get { return higth; }
    }

    public Pair UV
    {
        set { uv = value; }
        get { return uv; }
    }
}
