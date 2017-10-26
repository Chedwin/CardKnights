using System;
using System.Collections;
using UnityEngine;

public class Singleton<TYPE> : MonoBehaviour where TYPE : Singleton<TYPE>
{
    public static TYPE Instance { get; protected set; }

    public bool isPersistant = false;

    public virtual void Awake()
    {
        if (isPersistant)
        {
            if (!Instance)
                Instance = this as TYPE;
            else
                DestroyObject(gameObject);

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance = this as TYPE;
        }
    }

} // end class Singleton<TYPE>