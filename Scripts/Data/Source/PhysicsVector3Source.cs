namespace VRTK.Core.Data.Source
{
    using UnityEngine;

    public class PhysicsVector3Source : MonoBehaviour, IVector3Source
    {
        public Vector3 Vector => Physics.gravity;
    }
}