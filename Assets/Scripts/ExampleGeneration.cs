using TMPro;
using UnityEngine;

public class ExampleGeneration : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI exampleTMP;
    private int firstNumber;
    private int secongNumber;
    public int ResultOperation;
    private string operation;
    private string example;

    public void StartGenaration(int n)
    {

        switch (n)
        {
            case 1:
                addition();
                break;
            case 2:
                subtraction();
                break;
            case 3:
                multiplication();
                break;
            case 4:
                division();
                break;
            default:
                multiplication();
                break;
        }
    }

    private void addition()
    {
        operation = "+";
        firstNumber = Random.Range(0, 21);
        secongNumber = Random.Range(0, 11);
        ResultOperation = firstNumber + secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void subtraction()
    {
        operation = "-";
        while (secongNumber >= firstNumber)
        {
            firstNumber = Random.Range(0, 100);
            secongNumber = Random.Range(0, 100);
        }
        ResultOperation = firstNumber - secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }
    private void multiplication()
    {
        operation = "*";
        firstNumber = Random.Range(1, 11);
        secongNumber = Random.Range(1, 11);
        ResultOperation = firstNumber * secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void division()
    {
        operation = "/";
        do
        {
            firstNumber = Random.Range(0, 100);
            secongNumber = Random.Range(1, 11);
        } 
        while (firstNumber % secongNumber != 0);

        ResultOperation = firstNumber / secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }
}
