using UnityEngine;

public class ComponentsManager : MonoBehaviour
{
    public static ComponentsManager Instance;

    public Camera mainCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        else
            Instance = this;
    }
}
