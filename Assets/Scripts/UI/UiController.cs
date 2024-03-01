using System.Collections;
using Frolicode;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UiController : Singleton<UiController>
    {
        [SerializeField] private TMP_Text jackpotResultTxt;
        
        [SerializeField] private TMP_Text textOne;
        [SerializeField] private TMP_Text textTwo;
        [SerializeField] private TMP_Text textThree;

        
        private void OnEnable()
        {
            Dice.onDiceResult += SetDiceResult;
        }

        private void OnDisable()
        {
            Dice.onDiceResult -= SetDiceResult;
        }
        
        private void SetDiceResult(int diceIndex, int diceResult)
        {
            switch (diceIndex)
            {
                case 0:
                    textOne.SetText(diceResult.ToString());
                    break;
                case 1:
                    textTwo.SetText(diceResult.ToString());
                    break;
                case 2:
                    textThree.SetText(diceResult.ToString());
                    break;
            }
        }

        public void ShowJackpotWonBanner()
        {
            jackpotResultTxt.SetText("You won the jackpot.");
            StartCoroutine(ActivateAndDeactivateCoroutine(jackpotResultTxt.gameObject, 2f));
        }
        
        public void ShowJackpotLoseBanner()
        {
            jackpotResultTxt.SetText("You lose the jackpot, Better luck next time!");
            StartCoroutine(ActivateAndDeactivateCoroutine(jackpotResultTxt.gameObject, 2f));
        }
        
        // Coroutine for activating the GameObject and then deactivating it after a delay
        private IEnumerator ActivateAndDeactivateCoroutine(GameObject objectToToggle, float delay)
        {
            objectToToggle.SetActive(true); // Activate the GameObject

            // Wait for 'delay' seconds without blocking the game
            yield return new WaitForSeconds(delay);

            objectToToggle.SetActive(false); // Deactivate the GameObject
        }
    }
}