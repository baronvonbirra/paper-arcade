using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    public class CharacterController : MonoBehaviour
    {
        public static CharacterController Instance { get; private set; }

        [SerializeField] private CharacterVisuals visuals;
        [SerializeField] private CharacterAnimator animator;

        private CurrentOutfit currentOutfit = new CurrentOutfit();
        private Dictionary<ClothingType, ClothingItemData> equippedClothing = new Dictionary<ClothingType, ClothingItemData>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (visuals == null) visuals = GetComponent<CharacterVisuals>();
            if (animator == null) animator = GetComponent<CharacterAnimator>();
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            PlayerSaveData saveData = SaveManager.Instance.LoadGame();
            if (saveData != null && saveData.currentOutfit != null)
            {
                currentOutfit = saveData.currentOutfit;
            }
            if (visuals != null)
            {
                visuals.InitializeVisuals();
            }
        }

        public CurrentOutfit GetCurrentOutfit()
        {
            return currentOutfit;
        }

        public void SetOutfitPiece(ClothingType type, ClothingItemData item)
        {
            equippedClothing[type] = item;
            switch (type)
            {
                case ClothingType.Hat:
                    currentOutfit.hatId = item.id;
                    break;
                case ClothingType.Head:
                    currentOutfit.headId = item.id;
                    break;
                case ClothingType.Glasses:
                    currentOutfit.glassesId = item.id;
                    break;
                case ClothingType.Suit:
                    currentOutfit.suitId = item.id;
                    break;
                case ClothingType.Jacket:
                    currentOutfit.jacketId = item.id;
                    break;
                case ClothingType.Cape:
                    currentOutfit.capeId = item.id;
                    break;
                case ClothingType.Accessory:
                    if (!currentOutfit.accessoryIds.Contains(item.id))
                    {
                        currentOutfit.accessoryIds.Add(item.id);
                    }
                    break;
            }

            if (visuals != null)
            {
                visuals.ApplyClothingVisual(type, item);
            }

            PlayerSaveData saveData = SaveManager.Instance.LoadGame();
            saveData.currentOutfit = currentOutfit;
            SaveManager.Instance.SaveGame(saveData);

            if (animator != null)
            {
                animator.PlayAnimation("happy");
            }
        }

        public void RemoveOutfitPiece(ClothingType type)
        {
            if (equippedClothing.ContainsKey(type))
            {
                equippedClothing.Remove(type);
            }

            switch (type)
            {
                case ClothingType.Hat:
                    currentOutfit.hatId = null;
                    break;
                case ClothingType.Head:
                    currentOutfit.headId = null;
                    break;
                case ClothingType.Glasses:
                    currentOutfit.glassesId = null;
                    break;
                case ClothingType.Suit:
                    currentOutfit.suitId = null;
                    break;
                case ClothingType.Jacket:
                    currentOutfit.jacketId = null;
                    break;
                case ClothingType.Cape:
                    currentOutfit.capeId = null;
                    break;
                case ClothingType.Accessory:
                    currentOutfit.accessoryIds.Clear();
                    break;
            }

            if (visuals != null)
            {
                visuals.RemoveClothingVisual(type);
            }

            PlayerSaveData saveData = SaveManager.Instance.LoadGame();
            saveData.currentOutfit = currentOutfit;
            SaveManager.Instance.SaveGame(saveData);
        }

        public void PlayAnimation(string animationType)
        {
            if (animator != null)
            {
                animator.PlayAnimation(animationType);
            }
        }

        public Vector3 GetCharacterSpritePosition()
        {
            return transform.position;
        }

        public Dictionary<ClothingType, ClothingItemData> GetEquippedClothing()
        {
            return equippedClothing;
        }
    }
}
