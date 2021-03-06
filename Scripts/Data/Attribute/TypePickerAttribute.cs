﻿namespace VRTK.Core.Data.Attribute
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Defines the <c>[TypePicker]</c> attribute.
    /// </summary>
    /// <remarks>This attribute is only valid on fields that use <see cref="SerializedType"/>.</remarks>
    public class TypePickerAttribute : PropertyAttribute
    {
        public readonly Type superType;

        public TypePickerAttribute(Type superType)
        {
            this.superType = superType;
        }
    }
}