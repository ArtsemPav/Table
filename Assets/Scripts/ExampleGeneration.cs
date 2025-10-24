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

    void Start()
    {
        Multiplication();
        exampleTMP = exampleTMP.GetComponent<TextMeshProUGUI>();
    }

    private void Multiplication()
    {
        operation = "*";
        firstNumber = Random.Range(1, 11);
        secongNumber = Random.Range(1, 11);
        ResultOperation = firstNumber * secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void Addition()
    {
        operation = "+";
        firstNumber = Random.Range(0, 100);
        secongNumber = Random.Range(0, 100);
        ResultOperation = firstNumber + secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void Subtraction()
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
    private void Division()
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
