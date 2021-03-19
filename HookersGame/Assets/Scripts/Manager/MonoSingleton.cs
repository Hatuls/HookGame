using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour ,IInit where T: MonoSingleton<T> 
{
    private static T m_Instance;
    public static T Instance
    {
        get {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (m_Instance == null)
                    Debug.Log("Couldnt Find The Object of type " + typeof(T).ToString());
            }
              return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this as T;
        else if (m_Instance != this)
        {
            Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
            DestroyImmediate(this);
            return;
        }
    }
    public virtual void Init() { }
}
public interface IInit {
     void Init();
}