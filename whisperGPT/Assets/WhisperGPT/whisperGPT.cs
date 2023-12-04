using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private Button recordButton;
        [SerializeField] private Image progressBar;
        [SerializeField] private Dropdown dropdown;

        [SerializeField] private TMP_Dropdown modelDropdown;
        [SerializeField] private TMP_Dropdown voiceDropdown;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private TextMeshProUGUI speedLabel;
        [SerializeField] private TTSManager ttsManager;

        private readonly string fileName = "output.wav";
        private readonly int duration = 10;

        private AudioClip clip;
        private bool isRecording;
        private bool isTyping;
        private float time;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        private RectTransform tempMessageItem;

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = @"As an advisor, you want to design a study plan based on the student's requirements that meets their needs and background.You need to learn their requirements by asking the following questions one by one: 

        1.	What field of study would you like to learn this time?
        2.	Do you have any knowledge of the field of study before?
        If yes: What topic have you learned before? Do you want to include a review session about those topics?
        3.	What specific topic would you like to learn?
        4.	Do you want to include some assignments?
        If yes: which difficulty would you like to start with? (Easy, Medium, Hard)
        5.	How long do you plan to complete this study? (e.g., Hours, Weeks, Months)
            
        Remember: ask question one by one.

        Once you have received the answers from your students, you need to complete the following sentences as a summary based on the answers:

        I would like to study [subject/topic]. I currently have (some/no) knowledge in this area (including [terms]).There are some specific terms I would like to learn, such as [terms]. I would like the study program to include (no/some [difficulty level]) assignments, and I can dedicate [learning duration] to completing it.'
        
        After completing the summary, you will use the summary as a prompt to generate a study plan based on the answers and ask the student if they are satisfied with the generated plan and if they need to make any adjustments.

        Don't break character. Don't ever mention that you are an AI model. Start the conversation by asking students and do not reply to this message.
        ";

        private void Start()
        {
            
            foreach (var device in Microphone.devices)
            {
                dropdown.options.Add(new Dropdown.OptionData(device));
            }
            recordButton.onClick.AddListener(StartRecording);
            dropdown.onValueChanged.AddListener(ChangeMicrophone);

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            dropdown.SetValueWithoutNotify(index);

            SendReply();
            button.onClick.AddListener(SendReply);
        }


        private void AppendMessage(ChatMessage message, bool isTemporary = false)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
            
           if (isTemporary)
            {
                tempMessageItem = item;
            }
        }

        private void RemoveTemporaryMessage()
        {
            if (tempMessageItem != null)
            {
                var item = Instantiate(received, scroll.content);
                item.GetChild(0).GetChild(0).GetComponent<Text>().text = "...";
                item.anchoredPosition = new Vector2(0, -height);
                LayoutRebuilder.ForceRebuildLayoutImmediate(item);
                height -= item.sizeDelta.y;

                Destroy(tempMessageItem.gameObject);
                tempMessageItem = null;
            }
        }
         

        private async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };


            if (messages.Count != 0) AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text;

            messages.Add(newMessage);

            var tempMessage = new ChatMessage()
            {
                Role = "system",
                Content = "..."
            };

            
            AppendMessage(tempMessage,true);

            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            recordButton.enabled = false;
            


            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4",
                Messages = messages
            });

            messages.Remove(tempMessage);

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);
                RemoveTemporaryMessage();
                if (ttsManager) ttsManager.SynthesizeAndPlay(message.Content, (TTSModel)modelDropdown.value, (TTSVoice)voiceDropdown.value, speedSlider.value);
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
            recordButton.enabled = true;
        }

        // whisper part
        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }

        private void StartRecording()
        {
            isRecording = true;
            //recordButton.enabled = false;
            recordButton.GetComponentInChildren<Text>().text = "Stop recording";
            recordButton.onClick.RemoveListener(StartRecording);
            recordButton.onClick.AddListener(EndRecording);
            
            var index = PlayerPrefs.GetInt("user-mic-device-index");

            clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
        }

        private async void EndRecording()
        {
            time = 0;
            isRecording = false;
            progressBar.fillAmount = 0;

            Microphone.End(null);

            byte[] data = SaveWav.Save(fileName, clip);

            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() { Data = data, Name = "audio.wav" },
                Model = "whisper-1",
                Language = "en"
            };
            var res = await openai.CreateAudioTranscription(req);

            
            inputField.text = res.Text;
            SendReply();
            
            recordButton.onClick.RemoveListener(EndRecording);
            recordButton.onClick.AddListener(StartRecording);
            recordButton.GetComponentInChildren<Text>().text = "Start recording";

            // recordButton.enabled = true;
        }


        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progressBar.fillAmount = time / duration;
                if (time >= duration)
                {
                    EndRecording();
                }
            }
        }

        public void UpdateSpeedLabel(Single value)
        {
            speedLabel.text = value.ToString("0.00");
        }
    }
}
