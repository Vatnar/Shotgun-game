using TMPro;
using UnityEngine;

public class DynNumber : MonoBehaviour {
    
    [SerializeField] private TMP_Text numberText;
    
    private int currentNumber = 5;
    
    public void SetNumber(int number) {
        numberText.SetText(number.ToString());
        currentNumber = number;
    }
    public void IncreaseNumber() {
        currentNumber++;
        SetNumber(currentNumber);
    }
    public void DecreaseNumber() {
        currentNumber--;
        SetNumber(currentNumber);
    }
}
