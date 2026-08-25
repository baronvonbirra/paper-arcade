using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    public class ClothingManager : MonoBehaviour
    {
        public static ClothingManager Instance { get; private set; }

        private List<ClothingItemData> allClothingData = new List<ClothingItemData>();
        private Dictionary<string, ClothingItemData> clothingMap = new Dictionary<string, ClothingItemData>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadAllClothingData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadAllClothingData()
        {
            allClothingData.Clear();
            clothingMap.Clear();

            TextAsset jsonAsset = Resources.Load<TextAsset>("Data/Clothing/clothing_items");
            if (jsonAsset != null)
            {
                ClothingCollectionData collection = JsonUtility.FromJson<ClothingCollectionData>(jsonAsset.text);
                if (collection != null && collection.items != null)
                {
                    allClothingData = collection.items;
                    foreach (var item in allClothingData)
                    {
                        clothingMap[item.id] = item;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Clothing items JSON resource not found.");
            }
        }

        public ClothingItemData GetClothingItemById(string id)
        {
            if (clothingMap.TryGetValue(id, out ClothingItemData item))
            {
                return item;
            }
            return null;
        }

        public List<ClothingItemData> GetClothingByType(ClothingType type)
        {
            return allClothingData.FindAll(item => item.type == type);
        }

        public List<ClothingItemData> GetAllClothing()
        {
            return allClothingData;
        }

        public void ApplyClothingToCharacter(ClothingItemData itemData)
        {
            if (itemData == null) return;

            if (CharacterController.Instance != null)
            {
                CharacterController.Instance.SetOutfitPiece(itemData.type, itemData);
                PlayClothingSound(itemData.type);
                AnimatePlacingClothing(itemData);
            }
        }

        public void RemoveClothingFromCharacter(ClothingType type)
        {
            if (CharacterController.Instance != null)
            {
                CharacterController.Instance.RemoveOutfitPiece(type);
                PlayClothingSound(type);
            }
        }

        public void PlayClothingSound(ClothingType type)
        {
            AudioManager.Instance?.PlaySFX("pop");
        }

        public void AnimatePlacingClothing(ClothingItemData itemData)
        {
            StartCoroutine(AnimatePlacingCoroutine(itemData));
        }

        private IEnumerator AnimatePlacingCoroutine(ClothingItemData itemData)
        {
            float duration = Constants.CLOTHING_ANIMATION_SPEED;
            yield return new WaitForSeconds(duration);
        }

        public bool DetectClothingPlacement(ClothingItemData itemData, Vector3 dropPosition)
        {
            if (CharacterController.Instance != null)
            {
                Vector3 charPos = CharacterController.Instance.GetCharacterSpritePosition();
                float dist = Vector3.Distance(dropPosition, charPos);
                if (dist <= Constants.CLOTHING_DRAG_SNAP_DISTANCE || dist <= GameConfig.Instance?.clothingSnapDistance)
                {
                    ApplyClothingToCharacter(itemData);
                    return true;
                }
            }
            return false;
        }
    }
}
