using UnityEngine;

[CreateAssetMenu(fileName = "QuizQuestion", menuName = "Scriptable Objects/QuizQuestion")]
public class QuizQuestionSO : ScriptableObject
{
    [TextArea(2, 5)]
    public string questionText;

    [Tooltip("Enter exactly 4 answers here")]
    public string[] answers = new string[4];

    [Tooltip("0 for the first answer, 1 for the second, etc.")]
    public int correntAnswerIndex;
    internal int correctAnswer;
}
