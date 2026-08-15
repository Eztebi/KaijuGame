using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BodyStateMachineContext 
{
    private InputHandler _handler;
    private Rigidbody[] _rigidbodies;
    private Animator _animator;
    private Transform _bodyPos;
    public BodyStateMachineContext(Transform bodypos,InputHandler handler, Rigidbody[] rbs, Animator animator)
    {
        _handler = handler;
        _rigidbodies = rbs;
        _animator = animator;
        _bodyPos = bodypos;
    }
    public Transform FullBodyPos => _bodyPos;
}
