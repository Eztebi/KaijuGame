using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BodyStateMachineContext 
{
    private InputHandler _handler;
    private Rigidbody[] _rigidbodies;
    private Animator _animator;
    private Transform _bodyPos;
    private StaticBoneFollower[] _bones;

    public BodyStateMachineContext(Transform bodypos, StaticBoneFollower[] bones,InputHandler handler, Rigidbody[] rbs, Animator animator)
    {
        _handler = handler;
        _rigidbodies = rbs;
        _animator = animator;
        _bodyPos = bodypos;
        _bones = bones;
    }
    public void SetBoneStiffness(float spring, float damper)
    {
        if (_bones == null) return;

        foreach (var muscle in _bones)
        {
            muscle.SetMuscleStrength(spring, damper);
        }
    }
    public Transform FullBodyPos => _bodyPos;
    public InputHandler Handler => _handler;
}
