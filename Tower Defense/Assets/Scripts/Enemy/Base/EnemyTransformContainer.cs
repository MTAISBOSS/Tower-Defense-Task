using System.Collections.Generic;
using Service_Locator;
using UnityEngine;
using ZLinq;

namespace Enemy.Base
{
    public class EnemyTransformContainer : ITransformContainer , IService
    {
        private readonly List<Transform> _transforms = new List<Transform>();
        public void Add(Transform transform)
        {
            if (_transforms.Contains(transform))
            {
                return;
            }
            _transforms.Add(transform);
        }

        public void Remove(Transform transform)
        {
            if (_transforms.Contains(transform))
            {
                _transforms.Remove(transform);
            }
        }

        public Transform Get(Transform thisTransform)
        {
            if (_transforms.Count == 0)
            {
                return null;
            }
            var result = _transforms.AsValueEnumerable()
                .OrderBy(o => Vector3.Distance(thisTransform.position, o.position));
            return result.ToArray()[0];
        }
    }
}