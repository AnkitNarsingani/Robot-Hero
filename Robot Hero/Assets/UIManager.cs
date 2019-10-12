using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text stepText;
    [SerializeField] Text GameOverText;
    [SerializeField] FloatReference stepCounter;

    private void Start()
    {
        stepText.text = stepCounter.Variable.Value.ToString();
    }

    public void UpdateSteps()
    {
        stepCounter.Variable.ApplyChange(-1);
        stepText.text = stepCounter.Variable.Value.ToString();
        CheckLoseConditions();
    }

    void CheckWinConditions()
    {
        if(stepCounter.Variable.Value == 0)
        {
            GameOverText.gameObject.SetActive(true);
        }
    }

    void CheckLoseConditions()
    {
        if (stepCounter.Variable.Value == 0)
        {
            GameOverText.gameObject.SetActive(true);
        }
    }
}
