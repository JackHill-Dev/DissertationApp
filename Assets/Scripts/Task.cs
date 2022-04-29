using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Task
{
 
    private GameObject answer;
    private Transform currentModelTransform;
    private int taskNo;
    private string taskText;
    
    public bool isActive;
 
    public Task(int taskNum, Transform modelTransform, GameObject answerTransform, string taskTxt)
    {
        taskNo = taskNum;
        answer = answerTransform;
        currentModelTransform = modelTransform;
        taskText = taskTxt;
        isActive = false;
    }
    
    public bool CheckAnswer()
    {
        // Currently set up like this as it was returning false even when both transformes equalled each other
        bool bPos = answer.transform.position == currentModelTransform.position;
        bool bRot = answer.transform.eulerAngles == currentModelTransform.eulerAngles;
        bool bScale = answer.transform.localScale == currentModelTransform.localScale;

        bool b = bPos && bRot && bScale;
        return b;
    }

    public void EnableElements()
    {
        answer.SetActive(true);
    }
    public void DisableElements()
    {
        answer.SetActive(false);
    }

    public int GetTaskNo()
    {
        return taskNo;
    }
    public string GetText()
    {
        return taskText;
    }
}