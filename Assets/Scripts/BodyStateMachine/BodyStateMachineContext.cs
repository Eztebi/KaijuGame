using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BodyStateMachineContext 
{
    private InputHandler _handler;
    private Rigidbody[] _rigidbodies;
    private Animator _animator;
    private Transform _bodyPos;
    private StaticBoneFollower[] _bones;
    public Rigidbody HipsRB { get; private set; }
    public Rigidbody SpineRB { get; private set; }
    public Rigidbody HeadRB { get; private set; }

    public ConfigurableJoint HipsJoint { get; private set; } // El que NO tiene Connected Body
    public ConfigurableJoint SpineJoint { get; private set; }
    public ConfigurableJoint HeadJoint { get; private set; }
    public BodyStateMachineContext(Transform bodypos, StaticBoneFollower[] bones,InputHandler handler, Rigidbody[] rbs, Animator animator,
        Rigidbody hipsRB, Rigidbody spineRB, Rigidbody headRB,
        ConfigurableJoint hipsJoint, ConfigurableJoint spineJoint, ConfigurableJoint headJoint)
    {
        _handler = handler;
        _rigidbodies = rbs;
        _animator = animator;
        _bodyPos = bodypos;
        _bones = bones;
        HipsRB = hipsRB;
        SpineRB = spineRB;
        HeadRB = headRB;
        HipsJoint = hipsJoint;
        SpineJoint = spineJoint;
        HeadJoint = headJoint;
    }
    public void SetBodyToIdle()
    {
        // 1. Activar el Equilibrio de la Cadera (Angular Drives en XYZ)
        SetHipsBalance(15000f, 500f);

        // 2. Músculos del torso firmes para mantener la postura
        SetJointSlerp(SpineJoint, 4000f, 150f);
        SetJointSlerp(HeadJoint, 1000f, 40f);
    }

    public void SetBodyToRagdoll()
    {
        // 1. APAGAR el Equilibrio de la Cadera (Deja de flotar)
        SetHipsBalance(0f, 0f);

        // 2. APAGAR los músculos del torso (Se vuelve un fideo)
        SetJointSlerp(SpineJoint, 0f, 0f);
        SetJointSlerp(HeadJoint, 0f, 0f);
    }
    private void SetJointSlerp(ConfigurableJoint joint, float spring, float damper)
    {
        if (joint == null) return;
        JointDrive drive = joint.slerpDrive;
        drive.positionSpring = spring;
        drive.positionDamper = damper;
        joint.slerpDrive = drive;
    }

    private void SetHipsBalance(float spring, float damper)
    {
        if (HipsJoint == null) return;

        // Eje X (Adelante / Atrás)
        JointDrive driveX = HipsJoint.angularXDrive;
        driveX.positionSpring = spring;
        driveX.positionDamper = damper;
        HipsJoint.angularXDrive = driveX;

        // Eje YZ (Lados y Giro)
        JointDrive driveYZ = HipsJoint.angularYZDrive;
        driveYZ.positionSpring = spring;
        driveYZ.positionDamper = damper;
        HipsJoint.angularYZDrive = driveYZ;
    }
public void SetBodyToReset()
    {
        // El Reset es cuando se está levantando del suelo.
        // Necesitamos fuerza, pero con un Damper (freno) ALTO para que no "salte" violentamente al ponerse de pie.

        SetHipsBalance(10000f, 800f); // Damper alto para que suba suave
        SetJointSlerp(SpineJoint, 3000f, 200f);
        SetJointSlerp(HeadJoint, 800f, 60f);
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
