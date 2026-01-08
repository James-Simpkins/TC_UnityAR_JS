using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("UI Refrences")]
    // The 'quizPanel' reference is not needed anymore if the whole prefab is the quiz
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text feedbackText;

    [Header("Data")]
    public List<QuizQuestionSO> questions;

    private int currentQuestionIndex = 0;
    private bool isQuizActive = false;

    // --- CHANGED: Use OnEnable so the quiz restarts when the image is found again
    void OnEnable()
    {
        StartQuiz();
    }

    public void StartQuiz()
    {
        currentQuestionIndex = 0;
        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            EndQuiz();
            return;
        }

        isQuizActive = true;
        QuizQuestionSO currentQ = questions[currentQuestionIndex];

        questionText.text = currentQ.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TMP_Text btnText = answerButtons[i].GetComponentInChildren<TMP_Text>();

            if (i < currentQ.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                btnText.text = currentQ.answers[i];
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }

            // Reset button visual state
            answerButtons[i].interactable = true;
            answerButtons[i].image.color = Color.white;
        }

        if (feedbackText) feedbackText.text = "";
    }

    public void OnAnswerClicked(int buttonIndex)
    {
        if (!isQuizActive) return;

        QuizQuestionSO currentQ = questions[currentQuestionIndex];
        bool isCorrect = (buttonIndex == currentQ.correntAnswerIndex);

        StartCoroutine(HandleAnswerFeedback(isCorrect, buttonIndex));
    }

    IEnumerator HandleAnswerFeedback(bool isCorrect, int buttonIndex)
    {
        isQuizActive = false;

        // FIX: Retrieve the current question so we can find the correct answer index
        QuizQuestionSO currentQ = questions[currentQuestionIndex];


        if (isCorrect)
        {
            answerButtons[buttonIndex].image.color = Color.green;
            if (feedbackText) feedbackText.text = "Correct!";
        }
        else
        {
            answerButtons[buttonIndex].image.color = Color.red;
            // Highlight the correct answer
            answerButtons[currentQ.correctAnswer].image.color = Color.green;
            if (feedbackText) feedbackText.text = "Wrong!";
        }

        yield return new WaitForSeconds(1.5f);

        currentQuestionIndex++;
        LoadQuestion();
    }

    void EndQuiz()
    {
        // Optional: Disable the content or show a "Game Over" text
        questionText.text = "Quiz Complete!";
        foreach (var btn in answerButtons) btn.gameObject.SetActive(false);
        if (feedbackText) feedbackText.text = "";
    }
}