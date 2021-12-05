using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class KeyPad : MonoBehaviour
{
    [SerializeField] private List<Button> btns;
    [SerializeField] private Image activeImg;
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
        
        Transform activeImgTransform = activeImg.transform;
        
        // TODO: Fix bug where after a new matrix object is created the activeImg always stays under the UI element
        activeImgTransform.position = new Vector3( activeField.transform.position.x, activeField.transform.position.y , activeField.transform.position.z);
        activeImgTransform.rotation = activeField.transform.rotation;
        
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
