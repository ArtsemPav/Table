using UnityEngine;

public class MathTaskGenerator : MonoBehaviour
{
    public static MathTaskGenerator Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public MathTask Generate(TaskType taskType, int minValue, int maxValue)
    {
        MathTask task = new MathTask();

        int a = Random.Range(minValue, maxValue + 1);
        int b = Random.Range(minValue, maxValue + 1);

        switch (taskType)
        {
            case TaskType.Addition:
                task.a = a;
                task.b = b;
                task.op = '+';
                task.answer = a + b;
                break;
            case TaskType.Subtraction:
                // чтобы не было отрицательных
                if (a < b) { int t = a; a = b; b = t; }
                task.a = a;
                task.b = b;
                task.op = '-';
                task.answer = a - b;
                break;
            case TaskType.Multiplication:
                task.a = a;
                task.b = b;
                task.op = '×';
                task.answer = a * b;
                break;
            case TaskType.Division:
                // сделаем деление без остатка — генерим через умножение
                task.b = Mathf.Max(1, b);
                task.answer = a;
                task.a = task.answer * task.b;
                task.op = '÷';
                break;
            case TaskType.MixedAddSub:
                return Generate(Random.value < 0.5f ? TaskType.Addition : TaskType.Subtraction, minValue, maxValue);
            case TaskType.MixedMulDiv:
                return Generate(Random.value < 0.5f ? TaskType.Multiplication : TaskType.Division, minValue, maxValue);
            case TaskType.MixedAll:
                TaskType[] all = { TaskType.Addition, TaskType.Subtraction, TaskType.Multiplication, TaskType.Division };
                return Generate(all[Random.Range(0, all.Length)], minValue, maxValue);
        }

        return task;
    }
}
