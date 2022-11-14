using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;

        [Range(10, 20)]
        public int MaxLoot;
        [Range(1, 10)]
        public int MinLoot;

        [Range(1, 100)]
        public int Hp = 10;

        [Range(1f, 30f)]
        public float Damage = 1f;

        [Range(0.5f, 1f)]
        public float EffectiveDistance = 0.5f;

        [Range(0.5f, 1f)]
        public float Cleavage = 0.5f;

        [Range(1f, 10f)]
        public float MoveSpeed = 1f;
        public GameObject Prefab;
    }
}
