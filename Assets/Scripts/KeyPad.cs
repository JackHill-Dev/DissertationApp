using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class KeyPad : MonoBehaviour
{
    [SerializeField] private List<Button> btns;
    private List<Button> inputFields;
    private Button activeField;

    [SerializeField] private Transform userLocation;
    
    // Start is called before the first frame update
    void Start()
    {
        // Bind a custom on pressed function to each button
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(delegate { OnBtnPresed(btn.name); } );
        }

        inputFields = new List<Button>();
        
        FindAllMatrixFieldObjects();
    }

    void OnBtnPresed(string input)
    {
        TextMeshProUGUI inputTxt = activeField.GetComponentInChildren<TextMeshProUGUI>();
        
        if(input == "Clear")
            inputTxt.text = string.Empty;
        else
        if (inputTxt.text.Length < 3)
        {
            switch (input)
            {
                case "-":
                    inputTxt.text = input + inputTxt.text;
                    break;
                default:
                    inputTxt.text += input;
                    break;
            }
        }
    }
    
    
    void SetActiveField(Button inputField)
    {
        // Set the current active field
        activeField = inputField;
        
        RectTransform rt = GetComponent<RectTransform>(); 
        rt.position = new Vector3(userLocation.position.x, rt.position.y, -14);

    }

    public void FindAllMatrixFieldObjects()
    {
        inputFields = new List<Button>();
        
        GameObject[] fieldObjs =  GameObject.FindGameObjectsWithTag("matrixField");
        foreach (GameObject obj in fieldObjs)
        {
            Button b = obj.GetComponent<Button>();
            b.onClick.AddListener(delegate { SetActiveField(b); });
            inputFields.Add(b);
         
        }
    }
}
