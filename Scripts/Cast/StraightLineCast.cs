﻿namespace VRTK.Core.Cast
{
    using UnityEngine;
    using System.Linq;

    /// <summary>
    /// Casts a straight line and creates points at the origin and target.
    /// </summary>
    public class StraightLineCast : PointsCast
    {
        /// <summary>
        /// The maximum length to cast.
        /// </summary>
        [Tooltip("The maximum length to cast.")]
        public float maximumLength = 100f;

        /// <inheritdoc />
        public override void CastPoints()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitData;
            bool hasCollided = PhysicsCast.Raycast(physicsCast, ray, out hitData, maximumLength, Physics.IgnoreRaycastLayer);
            TargetHit = (hasCollided ? hitData : (RaycastHit?)null);

            points[0] = transform.position;
            points[1] = (hasCollided ? hitData.point : transform.position + transform.forward * maximumLength);

            ResultsChanged?.Invoke(eventData.Set(TargetHit, Points));
        }

        protected virtual void OnEnable()
        {
            points.AddRange(Enumerable.Repeat(Vector3.zero, 2));
        }

        protected virtual void OnDisable()
        {
            points.Clear();
        }
    }
}