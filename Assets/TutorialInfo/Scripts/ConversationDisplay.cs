using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class ConversationDisplay : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public ScrollRect scrollRect;

    public AudioSource audioSource;    
    public AudioClip typeSound;    
  
    public Animator judge1Animator;
    public Animator judge2Animator;
    public Animator entrepreneurAnimator;
  

    //public string absoluteJsonPath = "/Users/milanguzman/Documents/5thSemester/MultiAgentes/FinalProyect/PythonConection/agents/conversation.json";
    public string absoluteJsonPath = "C:\\Users\\aries\\OneDrive\\Desktop\\RetoSharkTank\\agents\\conversation.json";



    private ConversationRoot data;

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class BusinessIdea
    {
        public string name;
        public string description;
        public string target_market;
        public string revenue_model;
        public string current_traction;
        public string investment_needed;
        public string use_of_funds;
    }

    [System.Serializable]
    public class ConversationRoot
    {
        public BusinessIdea business_idea;
        public Message[] conversation_history;
        public string timestamp;
        public int total_messages;
    }

    void Start()
    {
        LoadJSON();
        StartCoroutine(DisplayConversation());
    }

    // ========================
    //       LOAD JSON
    // ========================
    void LoadJSON()
    {
        if (!File.Exists(absoluteJsonPath))
        {
            textUI.text = "ERROR: Archivo JSON no encontrado\n" + absoluteJsonPath;
            return;
        }

        string json = File.ReadAllText(absoluteJsonPath);
        data = JsonUtility.FromJson<ConversationRoot>(json);
    }

    // ========================
    //      CLEAR TEXT
    // ========================
    public void ClearConversation()
    {
        StopAllCoroutines();
        textUI.text = "";
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f; // subir al inicio
    }

    // ========================
    //      RELOAD JSON
    // ========================
    public void ReloadConversation()
    {
        ClearConversation();
        LoadJSON();
        StartCoroutine(DisplayConversation());
    }

    // ========================
    //    TYPEWRITER EFFECT
    // ========================
    IEnumerator DisplayConversation()
    {
        textUI.text = "";

        foreach (Message msg in data.conversation_history)
        {
            

            string role = msg.role.ToLower();

            // Activa animación con Trigger
            if (role.Contains("judge 1"))
                judge1Animator.SetTrigger("Talk");

            else if (role.Contains("judge 2"))
                judge2Animator.SetTrigger("Talk");

            else if (role.Contains("entrepreneur"))
                entrepreneurAnimator.SetTrigger("Talk");


            string speaker = $"<b>{msg.role}:</b>\n";
            yield return StartCoroutine(TypeText(speaker + msg.content + "\n\n"));

            // Espera antes de apagar animación
            yield return new WaitForSeconds(0.5f);



        }

        

    }


    void ResetAllTriggers()
    {
        judge1Animator.ResetTrigger("Talk");
        judge2Animator.ResetTrigger("Talk");
        entrepreneurAnimator.ResetTrigger("Talk");
    }

    IEnumerator TypeText(string fullText)
    {
        int soundFrequency = 0;

        foreach (char c in fullText)
        {
            textUI.text += c;

            soundFrequency++;
            if (soundFrequency >= 150) // sonido cada 2 letras
            {
                audioSource.PlayOneShot(typeSound);
                soundFrequency = 0;
            }

            // Auto-scroll
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
            Canvas.ForceUpdateCanvases();

            yield return new WaitForSeconds(0.01f);

        }
    }
}

