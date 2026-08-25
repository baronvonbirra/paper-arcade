using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    public class MemoryCard
    {
        public int id;
        public int pairId;
        public bool isFlipped;
        public bool isMatched;
    }

    public class MemoryMinigame : MinigameBase
    {
        [Header("Memory Settings")]
        [SerializeField] private int gridRows = 4;
        [SerializeField] private int gridCols = 4;

        private List<MemoryCard> cards = new List<MemoryCard>();
        private MemoryCard firstSelectedCard = null;
        private MemoryCard secondSelectedCard = null;
        private bool isCheckingPair = false;

        private void Awake()
        {
            minigameId = "memory";
            minigameType = MinigameType.Memory;
            musicKey = "Minigame_Music";
        }

        public override void Initialize(int difficulty)
        {
            base.Initialize(difficulty);
            switch (difficulty)
            {
                case 1: gridRows = 4; gridCols = 4; break;
                case 2: gridRows = 4; gridCols = 6; break;
                case 3: gridRows = 6; gridCols = 6; break;
                default: gridRows = 6; gridCols = 8; break;
            }
            GenerateCards();
        }

        private void GenerateCards()
        {
            cards.Clear();
            int totalPairs = (gridRows * gridCols) / 2;
            for (int i = 0; i < totalPairs; i++)
            {
                cards.Add(new MemoryCard { id = cards.Count, pairId = i });
                cards.Add(new MemoryCard { id = cards.Count, pairId = i });
            }
            // Shuffle
            for (int i = 0; i < cards.Count; i++)
            {
                var temp = cards[i];
                int randomIndex = Random.Range(i, cards.Count);
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }

        public void SelectCard(int index)
        {
            if (!isPlaying || isPaused || isCheckingPair) return;
            if (index < 0 || index >= cards.Count) return;

            MemoryCard card = cards[index];
            if (card.isFlipped || card.isMatched) return;

            card.isFlipped = true;
            AudioManager.Instance?.PlaySFX("click");

            if (firstSelectedCard == null)
            {
                firstSelectedCard = card;
            }
            else
            {
                secondSelectedCard = card;
                StartCoroutine(CheckMatchCoroutine());
            }
        }

        private IEnumerator CheckMatchCoroutine()
        {
            isCheckingPair = true;
            yield return new WaitForSeconds(0.8f);

            if (firstSelectedCard.pairId == secondSelectedCard.pairId)
            {
                firstSelectedCard.isMatched = true;
                secondSelectedCard.isMatched = true;
                AddScore(10);
                AudioManager.Instance?.PlaySFX("pop");

                if (CheckAllMatched())
                {
                    EndGame();
                }
            }
            else
            {
                firstSelectedCard.isFlipped = false;
                secondSelectedCard.isFlipped = false;
            }

            firstSelectedCard = null;
            secondSelectedCard = null;
            isCheckingPair = false;
        }

        private bool CheckAllMatched()
        {
            return cards.TrueForAll(c => c.isMatched);
        }
    }
}
