using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Data;
using UnityEngine;
using System;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(UniqueId))]
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [SerializeField]
        private MonsterTypeId MonsterTypeId;

        [SerializeField]
        private bool _slain;

        private string _id;

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
            {
                _slain = true;
            }
            else
            {
                Spawn();
            }
        }

        private void Spawn()
        {
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
                progress.KillData.ClearedSpawners.Add(_id);
        }

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
        }
    }
}
