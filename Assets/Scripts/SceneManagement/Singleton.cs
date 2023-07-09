using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField] private bool dontDestroyOnLoad = false;
    private static T instance;

    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if(instance != null && this.gameObject != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = (T)this;

        if (dontDestroyOnLoad)
        {
            if (!gameObject.transform.parent)
            {
                DontDestroyOnLoad(gameObject);
            } 
        }
    }

    protected virtual void OnDestroy()
    {
        if(this == instance)
        {
            instance = null;
        }
    }
}
