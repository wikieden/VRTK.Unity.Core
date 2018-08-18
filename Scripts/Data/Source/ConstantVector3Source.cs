namespace VRTK.Core.Data.Source
{
    using UnityEngine;

    public class ConstantVector3Source : MonoBehaviour, IVector3Source
    {
        public Vector3 vector;

        public Vector3 Vector => vector;
    }
}