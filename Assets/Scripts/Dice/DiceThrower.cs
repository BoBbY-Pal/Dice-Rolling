using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceThrower : MonoBehaviour
{
    public Dice dicePrefab;
    public int amountOfDice = 3;
    public float throwForce = 5f;
    public float rollForce = 10f;

    public Button rollDiceButton;
    public static UnityAction<Dictionary<int, int>> diceRolled;
    
    private Dictionary<int, int> diceResults = new Dictionary<int, int>();

    private ObjectPooler<Dice> dicePool;

    void Start()
    {
        dicePool = new ObjectPooler<Dice>(dicePrefab.gameObject, transform);
        rollDiceButton.onClick.AddListener(ButtonClick);
    }
    
    private void OnEnable()
    {
        Dice.onDiceResult += HandleDiceResult;
    }
    
    private void OnDisable()
    {
        Dice.onDiceResult -= HandleDiceResult;
        dicePool.ClearPool();
    }
     
    // This function executes when the Button is clicked.
    void ButtonClick()
    {
       RollDice();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0)  && !EventSystem.current.IsPointerOverGameObject())
        {
            RollDice();
        }
    }

    private async void RollDice()
    {
        if (dicePrefab == null )
            return;
        
        diceResults.Clear();    // Clear the previous dice results.
        dicePool.ClearPool();   // Clear the pooled objects so that they can be reused.
        
        for (int i = 0; i < amountOfDice; i++)
        {
            // Get a object from pool and reset it's position & rotation.
            Dice dice = dicePool.GetPooledObject();    
            dice.transform.position = transform.position;
            dice.transform.rotation = transform.rotation;
            
            // Roll the dice with force & torque.
            dice.RollDice(throwForce, rollForce, i);
            await Task.Yield();
        }
    }
    
    private void HandleDiceResult(int diceIndex, int result)
    {
        diceResults[diceIndex] = result; // Store result with the dice index as the key

        // Check if all dice have finished rolling
        if (diceResults.Count == amountOfDice)
        {
            diceRolled?.Invoke(diceResults);
        }
    }
}
