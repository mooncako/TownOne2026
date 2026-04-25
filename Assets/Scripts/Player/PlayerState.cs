using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField, BoxGroup("Debug")] public ReadyState ReadyState;
}
