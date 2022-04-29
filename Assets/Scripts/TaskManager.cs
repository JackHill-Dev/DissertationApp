using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private List<Task> tasks;

    Task currentTask;
    private Transform currentModelTransform;
    private int finalTaskNo = 5;
    
  
    [SerializeField] private TextMeshProUGUI correctAnswerTxt;
    [SerializeField] private  TextMeshProUGUI taskTitleTxt;
    [SerializeField] private TextMeshProUGUI taskTxt;

    [SerializeField] private GameObject correctAnswerObj;
    [SerializeField] private GameObject calculationsSlide;
    [SerializeField] private GameObject taskSlide;
    // Answers
    [SerializeField] private GameObject answerTransformPosObj;          // translate
    [SerializeField] private GameObject answerTransformScaleObj;        // scale
    [SerializeField] private GameObject answerTransform_TR;             // translate rotate
    [SerializeField] private GameObject answerTransform_TSX;            // translate, scale in x
    [SerializeField] private GameObject answerTransform_TRS;            // translate, rotate, uniform scale,
    
    // Start is called before the first frame update
    void Start()
    {
        tasks = new List<Task>();
        
        // Get current model transform
        currentModelTransform = FindObjectOfType<MatrixModel>().transform;
        SetupTasks();
       
       correctAnswerObj.SetActive(false);
    }

    void SetupTasks()
    {
        // Final position task
        Task t1 =  new Task(1, currentModelTransform,  answerTransformPosObj,
            "Look at the preview model in green, transform the given model to this position");
    
        // Final scale task
        Task t2 = new Task(2,currentModelTransform, answerTransformScaleObj,
        "Scale the model by a factor of 3 in all axes");

        // Translate roate task
        Task t3 = new Task(3, currentModelTransform, answerTransform_TR,
         "Translate the model to the preview model in green and rotate by 30 degrees anticlockwise in Z ");
        // Translate, scale in x
        Task t4 = new Task(4, currentModelTransform, answerTransform_TSX,
         "Translate the model to the preview model in green and scale it by 2 in X ");
        // Translate, rotate, scale
        Task t5 = new Task(5, currentModelTransform, answerTransform_TRS,
         "Translate the model to the preview model in green, rotate by 90 degrees clockwise in Z and scale by 2 in all axes");

        tasks.Add(t1);
        tasks.Add(t2);
        tasks.Add(t3);
        tasks.Add(t4);
        tasks.Add(t5);
        
        // Set the current task to task 1
        t1.isActive = true;
        currentTask = tasks[0];
        taskTxt.text = currentTask.GetText();
        taskTitleTxt.text = "Task " + currentTask.GetTaskNo();
        taskSlide.SetActive(true);
        calculationsSlide.SetActive(false);
        
    }
    
    public void DisplayCorrectAnswer()
    {
        taskSlide.SetActive(false);
        calculationsSlide.SetActive(true);
        // Check the current model against the answer to the current task
        if (currentTask.CheckAnswer())
        {
            // Display "Correct answer to user"
            correctAnswerTxt.color = Color.green; 
            correctAnswerTxt.text = "Correct!";
            correctAnswerObj.SetActive(true);
            
            // Proceed to next task
            NextTask(ref currentTask);
            
            
        }
        else
        {
            // Display "Incorrect answer, please try again to user"
            correctAnswerTxt.color = Color.red; 
            correctAnswerTxt.text = "Incorrect, reset the model and try again!";
            correctAnswerObj.SetActive(true);
            // Reset the current model transform (Get the user to press the reset button)
        }
    }

    public void DisplayNextTask()
    {
        taskSlide.SetActive(true);
        taskTitleTxt.text = "Task " + currentTask.GetTaskNo();
        calculationsSlide.SetActive(false);
        currentTask.EnableElements();
    }
    void NextTask(ref Task t)
    {
        // Set the completed task to be inactive
        t.isActive = false;
        // Disable all that tasks text elements
        t.DisableElements();
        
        // Select next task or go into "Sandbox mode" 
        if (t.GetTaskNo() < finalTaskNo)
        {
            int index = t.GetTaskNo();
            currentTask = tasks[index];
            currentTask.isActive = true;
            taskTxt.text = currentTask.GetText();
            
        }
        else
        {
            taskTitleTxt.text = "Sandbox";
            taskTxt.text = "You have entered sandbox mode!!";
        }
    }


}
