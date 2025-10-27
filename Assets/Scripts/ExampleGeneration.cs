using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;

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
        randomGeneration();
    }

    private void randomGeneration()
    {
        List<Action> functions = new List<Action>();
        if (PlayerPrefs.GetInt("AddBool", 1) == 1)
        {
            functions.Add(addition);
        }
        if (PlayerPrefs.GetInt("SubBool", 1) == 1)
        {
            functions.Add(subtraction);
        }
        if (PlayerPrefs.GetInt("MultyBool", 1) == 1)
        {
            functions.Add(multiplication);
        }
        if (PlayerPrefs.GetInt("DivBool", 1) == 1)
        {
            functions.Add(division);
        }
        if (functions.Count > 0)
        {
            System.Random random = new System.Random();
            int index = random.Next(functions.Count);
            functions[index]();
        }
        else
        {
            Debug.Log("Нет доступных функций для выполнения");
        }
    }

    private void addition()
    {
        operation = "+";
        firstNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("FirstAdd", 11));
        secongNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("SecondtAdd", 11));
        ResultOperation = firstNumber + secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void subtraction()
    {
        operation = "-";
        while (secongNumber >= firstNumber)
        {
            firstNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("FirstSub", 21));
            secongNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("SecondSub", 11));
        }
        ResultOperation = firstNumber - secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }
    private void multiplication()
    {
        operation = "*";
        firstNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("FirstMulty", 11));
        secongNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("SecondMulty", 11));
        ResultOperation = firstNumber * secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void division()
    {
        operation = "/";
        do
        {
            firstNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("FirstDiv", 101));
            secongNumber = UnityEngine.Random.Range(1, PlayerPrefs.GetInt("SecondDiv", 11));
        } 
        while (firstNumber % secongNumber != 0);

        ResultOperation = firstNumber / secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }
}
