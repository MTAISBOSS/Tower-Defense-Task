using UnityEditor;
using UnityEngine.UIElements;

namespace Wave
{
    [CustomEditor(typeof(Wave))]
    public class WaveEditor : Editor
    {
        public VisualTreeAsset visualTreeAsset;
        public override VisualElement CreatePreview(VisualElement inspectorPreviewWindow)
        {
            VisualElement root = new VisualElement();
            visualTreeAsset.CloneTree(root);
            return root;
        }
    }
}