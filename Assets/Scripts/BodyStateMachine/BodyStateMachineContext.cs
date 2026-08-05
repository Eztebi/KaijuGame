using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BodyStateMachineContext : MonoBehaviour
{
    private InputHandler _handler;
    private Rigidbody[] _rigidbodies;
    private Animator _animator;
    public BodyStateMachineContext(InputHandler handler, Rigidbody[] rbs, Animator animator)
    {
        _handler = handler;
        _rigidbodies = rbs;
        _animator = animator;

    }

}
