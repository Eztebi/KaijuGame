using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelStartEvent : MonoBehaviour
{
    [SerializeField] private float delayBeforeEvent = 0f;

    public UnityEvent OnLevelStart;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delayBeforeEvent);
        OnLevelStart.Invoke();
    }
}
