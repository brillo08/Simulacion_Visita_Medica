using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(ComponentsManager.Instance.mainCamera.transform, -Vector3.up);
    }
}
