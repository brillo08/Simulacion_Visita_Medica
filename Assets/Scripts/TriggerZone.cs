using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent OnEnterEvent;
    public UnityEvent OnExitEvent;
    public string[] triggeredBy;

    
    private void OnTriggerEnter(Collider o)
    {
        Debug.Log(o.gameObject.name);

        if (triggeredBy.ToList().Contains(o.tag))
            OnEnterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider o)
    {
        if (triggeredBy.ToList().Contains(o.tag))
            OnExitEvent?.Invoke();
    }
}
