using System;

[Serializable]
public class TaskRecord
{
    public string taskText;
    public int correctAnswer;
    public int playerAnswer;
    public bool isCorrect;
    public float timeSpent;
}