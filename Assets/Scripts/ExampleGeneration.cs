using TMPro;
using UnityEngine;
using System;

public class ExampleGeneration : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI exampleTMP;
    private int firstNumber;
    private int secongNumber;
    public int ResultOperation;
    private string operation;
    private string example;

    public void StartGenaration()
    {
        multiplication();
    }


    private void addition()
    {
        operation = "+";
        firstNumber = UnityEngine.Random.Range(0, 21);
        secongNumber = UnityEngine.Random.Range(0, 11);
        ResultOperation = firstNumber + secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void subtraction()
    {
        operation = "-";
        while (secongNumber >= firstNumber)
        {
            firstNumber = UnityEngine.Random.Range(0, 100);
            secongNumber = UnityEngine.Random.Range(0, 100);
        }
        ResultOperation = firstNumber - secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }
    private void multiplication()
    {
        operation = "*";
        firstNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("FirstAdd", 11));
        secongNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("SecondAdd", 11));
        ResultOperation = firstNumber * secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void division()
    {
        operation = "/";
        do
        {
            firstNumber = UnityEngine.Random.Range(0, 100);
            secongNumber = UnityEngine.Random.Range(1, 11);
        } 
        while (firstNumber % secongNumber != 0);

        ResultOperation = firstNumber / secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }
}
