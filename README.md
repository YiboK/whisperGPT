# WhisperGPT Repo for CS639: Capstone Fall2023

The whisperGPT folder contains the program that implements a program that is able to talk to the user. It uses GPT-4, Whisper-1, and TTS-1 models from OpenAI API. The Unity version is 2022.3.10f1.

The program in this folder is used for develop and test the whisperGPT foler, and will be integrated into the VR program [here](https://github.com/YiboK/Holos)

## Little Demo 
[Video demo](https://drive.google.com/file/d/1TmFH24tqAkT4I1eGQh69J3QSAJs-fBQg/view?usp=sharing)

Photo of WhisperGPT UI: ![image](https://github.com/YiboK/whisperGPT/assets/94937314/f2fde9dc-87a4-4072-9248-dbdc5c795ada)


In the VR program: ![image](https://github.com/YiboK/whisperGPT/assets/94937314/31567423-7bea-4791-abef-4effe78a36b0)


# Availability
Since the OpenAI is constantly updating their API, I changed something in the package (The lastest update: 12/15/2023). Therefore, it is recommended to download the files at [here](https://drive.google.com/file/d/1a8Y-2t_d4rUViUQHDP_3YiNhpNKkHJrk/view?usp=sharing) rather than cloning the repo.

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
