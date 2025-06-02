using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject createObject = new GameObject(typeof(T).Name);
                    _instance = createObject.AddComponent<T>();
                    DontDestroyOnLoad(createObject);
                }
            }

            return _instance;
        }
    }
}
