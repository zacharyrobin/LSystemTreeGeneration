// Name: L-System Fractal Plant 
// Variables: F
// Contants: + - [ ]
// F means "draw forward, - means "turn right 25°", + means "turn left 25°"
// [ corresponds to saving the current values for position and angle which are 
// restored when the corresponding ] is executed
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LSystemGenerator : MonoBehaviour
{
    private string axiom = "F";
    private string currentString;
    private float angle;

    private  Dictionary<char, string> rules = new Dictionary<char, string>();
    private Stack<TransformInfo> transformStack = new Stack<TransformInfo>();

    private float length;
    private bool isGenerating = false;
    // Start is called before the first frame update
    void Start()
    {
        rules.Add('F', "FF+[+F-F-F]-[-F+F+F]");
        currentString = axiom;
        angle = 25f;
        length = 10f;
        StartCoroutine(GenerateLSystem());
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Basic Animation 
    IEnumerator GenerateLSystem(){
        int count = 0;
         while (count < 5) {
              if (!isGenerating) {
                  isGenerating = true;
                  StartCoroutine(Generate());
              } else {
                  yield return new WaitForSeconds(0.1f);
              }
         }
         Debug.Log("All Done :)");
    }

    IEnumerator Generate(){
        length = length/2f;
        string newString = "";

        char[] stringCharacters = currentString.ToCharArray();

        for(int i = 0; i < stringCharacters.Length; i++){
            char currentCharacter = stringCharacters[i];

            //Rules for characters 
            if (rules.ContainsKey(currentCharacter)){
                newString += rules[currentCharacter];
            } else {
                newString += currentCharacter.ToString();
            }
        }
        currentString = newString;

        stringCharacters = currentString.ToCharArray();
        for (int i = 0; i < stringCharacters.Length; i++) {
            char currentCharacter = stringCharacters[i];
            if (currentCharacter == 'F'){
                // Move forward 
                Vector3 initialPosition = transform.position;
                // *** Maybe add time variable to make it more smooth?
                transform.Translate(Vector3.forward * length);
                // Drawing the line 
                Debug.DrawLine(initialPosition, transform.position, Color.white, 10000f, false);
                yield return null;

            } else  if(currentCharacter == '+') {
                transform.Rotate(Vector3.up * angle);
            } else  if(currentCharacter == '-') {
                transform.Rotate(Vector3.up * -angle);
            } else  if(currentCharacter == '[') {
                TransformInfo ti = new TransformInfo();
                ti.position = transform.position;
                ti.rotation = transform.rotation;
                transformStack.Push(ti);
            } else if(currentCharacter == ']') {
                TransformInfo ti = transformStack.Pop();
                transform.position = ti.position;
                transform.rotation = ti.rotation;
            }
        }
        isGenerating = false; 
    }
}
