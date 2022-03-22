using UnityEngine;

public class DontDestroyInstance : MonoBehaviour
{

    public static DontDestroyInstance instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
