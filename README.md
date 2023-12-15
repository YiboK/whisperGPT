# whisperGPT Repo for CS639: Capstone Fall2023

The whisperGPT folder contains the program that implements a program that is able to talk to the user. It uses GPT-4, Whisper-1, and TTS-1 models from OpenAI API.

The program in this folder is used for develop and test the whisperGPT foler, and will be integrated into the VR program [hear](https://github.com/YiboK/Holos)

## Little Demo 
Video: [https://drive.google.com/file/d/17VLDbQbgzMtXqnSV3pLMX1ZCaYyKFY12/view?usp=sharing](https://drive.google.com/file/d/1alb4bMD8qOr1Fw89V4AmEOXj5-dzuPtK/view?usp=drive_link)
![image](https://github.com/YiboK/whisperGPT/assets/94937314/0bcde39b-4dd0-45d0-947f-15282aa57da0)


In the VR program: ![image](https://github.com/YiboK/whisperGPT/assets/94937314/ab5a4536-f0ac-4d37-8da4-085bd5e35453)

# Availability
Since I changed something in packages, it is recommended to download the files at [here](https://drive.google.com/file/d/1a8Y-2t_d4rUViUQHDP_3YiNhpNKkHJrk/view?usp=sharing) rather than cloning the repo.

### Setting Up Your OpenAI Account
To use the OpenAI API, you need to have an OpenAI account. Follow these steps to create an account and generate an API key:

- Go to https://openai.com/api and sign up for an account
- Once you have created an account, go to https://beta.openai.com/account/api-keys
- Create a new secret key and save it

### Saving Your Credentials
To make requests to the OpenAI API, you need to use your API key and organization name (if applicable). To avoid exposing your API key in your Unity project, you can save it in your device's local storage.

To do this, follow these steps:

- Create a folder called .openai in your home directory (e.g. `C:User\UserName\` for Windows or `~\` for Linux or Mac)
- Create a file called `auth.json` in the `.openai` folder
- Add an api_key field to the auth.json file and save it
- Here is an example of what your auth.json file should look like:

```json
{
    "api_key": "sk-...W6yi"
}
```


**IMPORTANT:** Your API key is a secret. 
Do not share it with others or expose it in any client-side code (e.g. browsers, apps). 
If you are using OpenAI for production, make sure to run it on the server side, where your API key can be securely loaded from an environment variable or key management service.
