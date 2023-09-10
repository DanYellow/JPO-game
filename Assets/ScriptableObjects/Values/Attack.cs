using UnityEngine;

namespace MyScriptableObject
{
    [CreateAssetMenu(fileName = "New AttackValue", menuName = "ScriptableObjects/Values/AttackValue", order = 0)]
    public class Attack : ScriptableObject
    {
        public int damage;
        public int knockbackForce = 0;

        public int rate = 0;

#pragma warning disable 0414
        [Multiline, SerializeField]
        private string DeveloperDescription = "";
#pragma warning restore 0414
    }
}
