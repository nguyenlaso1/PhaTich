using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Player : MonoBehaviour
{
    private string name;
    private int map;
    private long level;

    public Player() { }

    public Player(string name, int map, long level)
    {
        this.name = name;
        this.map = map;
        this.level = level;
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Map
    {
        get { return map; }
        set { map = value; }
    }

    public long Level
    {
        get { return level; }
        set { level = value; }
    }
}

