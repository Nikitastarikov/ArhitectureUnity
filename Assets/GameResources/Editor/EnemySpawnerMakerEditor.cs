using CodeBase.Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EnemySpawnMarker))]
    public class EnemySpawnerMakerEditor : UnityEditor.Editor
    {
        private const float RADIUS = 0.5f;

        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(EnemySpawnMarker spawner, GizmoType gizmo)
        {
            CircleGizmo(spawner.transform, RADIUS, Color.red);
        }

        private static void CircleGizmo(Transform transform, float radius, Color color)
        {
            Vector3 pos = transform.position;
            Gizmos.color = color;
            Gizmos.DrawSphere(pos, radius);
        }
    }
}
