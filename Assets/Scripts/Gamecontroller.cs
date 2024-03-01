using System.Collections.Generic;
using UI;
using UnityEngine;



public class Gamecontroller : MonoBehaviour
{
    private void OnEnable()
    {
        DiceThrower.diceRolled += CheckJackpot;
    }
    private void OnDisable()
    {
        DiceThrower.diceRolled -= CheckJackpot;
    }
   
    
    private void CheckJackpot(Dictionary<int,int> dices)
    {
        if (dices == null || dices.Count == 0)
        {
            // Return false or handle empty list case as needed
            return;
        }

        // Get the value of the first dice to compare with others
        int firstDiceResult = dices[0]; // Assuming 'Value' is the property that stores the dice's value

        foreach (int diceResult in dices.Values)
        {
            if (diceResult != firstDiceResult)
            {
                // Found a dice that doesn't match the first one's value, so not all dice have the same value
                JackpotLose();
                return ;
            }
        }

        // If the loop completes without finding any differing values, all dice have the same value
        JackpotWon();
    }
    
    public void JackpotWon()
    {
            Debug.Log("You won the jackpot.");
            UiController.Instance.ShowJackpotWonBanner();
    }

    public void JackpotLose()
    {
        Debug.Log("You lose the jackpot, Better luck next time!");
        UiController.Instance.ShowJackpotLoseBanner();
    }
   

  
   
   



   
    
}
