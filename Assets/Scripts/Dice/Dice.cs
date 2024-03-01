using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [SerializeField] private Transform[] diceFaces;
    [SerializeField] private Rigidbody rb;
    
    [SerializeField] private float _diceThreshold = 0.001f; 
    
    private int _diceIndex = -1;
    private bool _hasStoppedRolling;
    private bool _delayFinished;
    
    public int result { get; private set; }
    
    public static UnityAction<int, int> onDiceResult;

    private void OnDisable()
    {
        Reset();
    }

    void Update()
    {
        if (!_delayFinished)
            return;

        if (!_hasStoppedRolling && rb.velocity.sqrMagnitude < _diceThreshold)
        {
            _hasStoppedRolling = true;
            result = GetNumberOnTopFace();
        }
    }

    [ContextMenu("Get Top Face")]
    private int GetNumberOnTopFace()
    {
        if (diceFaces == null)
            return -1;
        int topFace = 0;
        float lastYPosition = diceFaces[0].position.y;

        for (int i = 0; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }
        
        Debug.Log($"Dice Result: {topFace + 1}");
        onDiceResult?.Invoke(_diceIndex,topFace + 1);
        return topFace + 1;
    }

    public void RollDice(float throwForce, float rollForce, int index)
    {
        _diceIndex = index;
        var randomVal = Random.Range(-1f, 1f);
        rb.AddForce(transform.forward * (throwForce + randomVal), ForceMode.Impulse);

        float randX = Random.Range(0, 1f);
        float randY = Random.Range(0, 1f);
        float randZ = Random.Range(0, 1f);
        
        rb.AddTorque(new Vector3(randX, randY, randZ) * (rollForce + randomVal), ForceMode.Impulse);

        DelayResult();
    }

    private async void DelayResult()
    {
        await Task.Delay(1000);
        _delayFinished = true;
    }

    private void Reset()
    {
        _hasStoppedRolling = false;
        _delayFinished = false;
        _diceIndex = -1;
        gameObject.SetActive(false);
    }
}
