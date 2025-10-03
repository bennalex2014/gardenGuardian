using UnityEngine;

public class RapidFireTurret : TurretScript
{
    // All values inherited from base class
    // Set them in Unity Inspector, not in code!

    protected override void Start()
    {
        base.Start();
    }

    void OnDrawGizmosSelected()
    {
        if (showRangeGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}