using MoreMountains.Tools;
using UnityEngine;

public class SoundManager : MMSingleton<SoundManager>
{

    protected override void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    

}
