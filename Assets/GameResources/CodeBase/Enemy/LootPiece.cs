using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Data;
using System.Collections;
using TMPro;
using UnityEngine;
using CodeBase.Logic;
using System;
using UnityEngine.SceneManagement;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(UniqueId))]
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        private const float TIME_DELAY = 1.5f;

        [SerializeField]
        private GameObject _skull;
        [SerializeField]
        private GameObject _pickupFxPrefab;
        [SerializeField]
        private GameObject _pickupPopup;

        [SerializeField]
        private TextMeshPro _lootText;

        private Loot _loot;

        private string _id = string.Empty;
        private bool _isGet = false;
        private WorldData _worldData;

        public void Constructor(WorldData worldData) => 
            _worldData = worldData;

        public void Initialize(Loot loot) => 
            _loot = loot;

        public void UpdateProgress(PlayerProgress progress) => 
            progress.KnokedOutLoot.NotCollectedLoot.Add(new LootOnLevel(_id, CreatePositionOnLevel(CurrentLevel(), transform.position.AsVectorData()), _loot));

        public void LoadProgress(PlayerProgress progress)
        {
            LootOnLevel lootOnLevel = progress.KnokedOutLoot.NotCollectedLoot.Find(loot => loot.Id == _id);
            if (lootOnLevel != null)
            {
                _loot = lootOnLevel.Loot;
                transform.position = lootOnLevel.PositionOnLevel.Position.AsUnityVector();
            }
        }

        private PositionOnLevel CreatePositionOnLevel(string id, Vector3Data vector3Data) =>
            new PositionOnLevel(id, vector3Data);

        private string CurrentLevel() => 
            SceneManager.GetActiveScene().name;

        private void Awake() => 
            _id = GetComponent<UniqueId>().Id;

        private void OnTriggerEnter(Collider other) => Pickup();

        private void Pickup()
        {
            if (_isGet)
                return;

            _isGet = true;

            UpdateWorldData();
            HideSkull();
            PickupFX();
            ShowText();
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideSkull() => 
            _skull.SetActive(false);

        private void PickupFX() => 
            Instantiate(_pickupFxPrefab, transform.position, Quaternion.identity);

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(TIME_DELAY);
            Destroy(gameObject);
        }

        private void ShowText()
        {
            _lootText.text = $"{_loot.Value}";
            _pickupPopup.SetActive(true);
        }
    }
}
