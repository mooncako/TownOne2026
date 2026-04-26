using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MinionSpawnPoint : MonoBehaviour
{
    public bool IsOccupied = false;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, new Vector3(.5f, 1, .5f));

        Handles.Label(transform.position + Vector3.up, "Minion Spawn Point", new GUIStyle()
        {
            normal = new GUIStyleState() { textColor = Color.green },
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        });
    }
#endif
}
