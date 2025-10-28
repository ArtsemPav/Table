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

    private int addMaxFactor1 = 10;
    private int addMaxFactor2 = 10;
    private int subMaxFactor1 = 20;
    private int subMaxFactor2 = 10;
    private int multyMaxFactor1 = 10;
    private int multyMaxFactor2 = 10;
    private int divMaxFactor1 = 100;
    private int divMaxFactor2 = 10;

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
        firstNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("FirstAdd", addMaxFactor1)+1);
        secongNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("SecondtAdd", addMaxFactor2)+1);
        ResultOperation = firstNumber + secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void subtraction()
    {
        operation = "-";
        while (secongNumber >= firstNumber)
        {
            firstNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("FirstSub", subMaxFactor1)+1);
            secongNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("SecondSub", subMaxFactor2)+1);
        }
        ResultOperation = firstNumber - secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }
    private void multiplication()
    {
        operation = "*";
        firstNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("FirstMulty", multyMaxFactor1)+1);
        secongNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("SecondMulty", multyMaxFactor2)+1);
        ResultOperation = firstNumber * secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }

    private void division()
    {
        operation = "/";
        do
        {
            firstNumber = UnityEngine.Random.Range(0, PlayerPrefs.GetInt("FirstDiv", divMaxFactor1)+1);
            secongNumber = UnityEngine.Random.Range(1, PlayerPrefs.GetInt("SecondDiv", divMaxFactor2)+1);
        } 
        while (firstNumber % secongNumber != 0);

        ResultOperation = firstNumber / secongNumber;
        example = $"{firstNumber} {operation} {secongNumber} = ";
        exampleTMP.text = example;
    }
}
