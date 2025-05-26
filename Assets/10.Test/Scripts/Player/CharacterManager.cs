using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CharacterManager>();
                if (_instance == null)
                {
                    var go = new GameObject("CharacterManager");
                    _instance = go.AddComponent<CharacterManager>();
                }
            }
            return _instance;
        }
    }

    // 플레이어 참조
    public Player Player;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
