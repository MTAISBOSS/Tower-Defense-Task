using UnityEditor;
using UnityEngine;

namespace Level_Design
{
    [ExecuteAlways]
    public class PlacementService : MonoBehaviour
    {
        [SerializeField] private float sphereRadius = 1f;
        [SerializeField] private float lineLength = 1f;
        [SerializeField] private GameObject prefab;
        [SerializeField] private float offset;

        private bool _isEditing;
        private Vector3 _intersectedPoint;
        private Vector3 _sceneCameraPosition;
        private Vector3 _surfaceNormal;

#if UNITY_EDITOR
        private void OnEnable()
        {
            SceneView.duringSceneGui += SceneViewOnduringSceneGui;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= SceneViewOnduringSceneGui;
        }

        private void SceneViewOnduringSceneGui(SceneView sceneView)
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftControl)
            {
                _isEditing = !_isEditing;
                e.Use();
            }

            if (!_isEditing)
            {
                return;
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                PlaceObject();
                e.Use();
            }

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            bool hasIntersection = Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);
            _intersectedPoint = hasIntersection ? hitInfo.point : Vector3.zero;
            _surfaceNormal = hitInfo.normal;
            SceneView.RepaintAll();
        }

        private void PlaceObject()
        {
            GameObject instantiatePrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instantiatePrefab.transform.position = _intersectedPoint + offset * _surfaceNormal;
            instantiatePrefab.transform.up = _surfaceNormal;
            Undo.RegisterCreatedObjectUndo(instantiatePrefab,"Place Object");
        }
#endif
        private void OnDrawGizmos()
        {
            if (_isEditing)
            {
                Gizmos.color = Color.green;
                float distance = Vector3.Distance(_sceneCameraPosition, _intersectedPoint);
                Gizmos.DrawSphere(_intersectedPoint, distance * sphereRadius);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(_intersectedPoint, _intersectedPoint + (_surfaceNormal) * distance * lineLength);
            }
        }
    }
}