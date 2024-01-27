using UnityEngine;

namespace MyScriptableObject
{
    [CreateAssetMenu(fileName = "New Attack Value", menuName = "ScriptableObjects/Values/AttackValue", order = 0)]
    public class Attack : ScriptableObject
    {
        public int damage;
        public Vector2 knockbackForce = Vector2.zero;
        public bool isKnockingSelf = false;
        public float stunTime;
        public float recoveryTime;

        public CameraShakeTypeValue cameraShake;

#pragma warning disable 0414
        [Multiline, SerializeField]
        private string DeveloperDescription = "";
#pragma warning restore 0414
    }
}
