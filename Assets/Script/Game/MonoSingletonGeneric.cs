using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingletonGeneric<T> : MonoBehaviour where T : MonoSingletonGeneric<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
            instance = (T)this;
        else
        {
            Debug.Log("Someone Trying to Duplicate Singleton");
            Destroy(this);
        }

    }


}