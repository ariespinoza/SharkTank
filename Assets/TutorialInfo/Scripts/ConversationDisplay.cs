using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class ConversationDisplay : MonoBehaviour
{
    public TextMeshProUGUI textUI; 
    public ScrollRect scrollRect;  

    public string absoluteJsonPath = "/Users/milanguzman/Documents/5thSemester/MultiAgentes/FinalProyect/PythonConection/agents/conversation.json";

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
        StartCoroutine(LoadAndDisplayConversation());
    }

    IEnumerator LoadAndDisplayConversation()
    {
        if (!File.Exists(absoluteJsonPath))
        {
            textUI.text = "ERROR: Archivo JSON no encontrado\n" + absoluteJsonPath;
            yield break;
        }

        string json = File.ReadAllText(absoluteJsonPath);
        ConversationRoot data = JsonUtility.FromJson<ConversationRoot>(json);

        textUI.text = "";

        foreach (Message msg in data.conversation_history)
        {
            string speaker = $"<b>{msg.role}:</b>\n";
            yield return StartCoroutine(TypeText(speaker + msg.content + "\n\n"));
        }
    }

    IEnumerator TypeText(string fullText)
    {
        foreach (char c in fullText)
        {
            textUI.text += c;

            // Auto scroll
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
            Canvas.ForceUpdateCanvases();

            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
