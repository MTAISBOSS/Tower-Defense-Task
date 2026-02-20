using UnityEngine;

namespace Enemy
{
    public interface ITransformContainer
    {
        void Add(Transform transform);
        void Remove(Transform transform);
        Transform Get(Transform thisTransform);
    }
}