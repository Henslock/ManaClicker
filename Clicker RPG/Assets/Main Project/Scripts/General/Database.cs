using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(Database)) as Database;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static Database instance;

    [Serializable]
    protected struct unitNameLookupStruct
    {
        public string name;
        public GameObject unitObj;
    }

    [SerializeField]
    protected unitNameLookupStruct[] UnitLookupStruct;

    public Dictionary<string, GameObject> units = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach(unitNameLookupStruct uStruct in UnitLookupStruct)
        {
            units.Add(uStruct.name, uStruct.unitObj);
        }
    }
}
