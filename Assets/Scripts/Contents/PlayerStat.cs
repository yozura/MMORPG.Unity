using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] protected int _exp;
    [SerializeField] protected int _gold;

    public int EXP
    {
        get { return _exp; }
        set
        {
            _exp = value;
            // 레벨업 체크
            int level = Level;
            while(true)
            {
                Data.Stat stat;
                if (!Managers.Data.StatDict.TryGetValue(level + 1, out stat))
                    break;
                if (_exp < stat.totalExp)
                    break;

                level++;
            }

            if (level == Level)
            {
                Debug.Log("Level Up!");
                Level = level;
                SetStat(Level);
            }
        }
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;
        _exp = 0;
        _defense = 5;
        _moveSpeed = 5.0f;
        _gold = 0;

        SetStat(_level);   
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
