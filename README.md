# Cybersecurity Awareness Chatbot – Part 1

## Project Overview
A C# console-based chatbot designed to educate South African citizens about cybersecurity threats such as phishing, weak passwords, and unsafe browsing. This is Part 1 of a three-part Portfolio of Evidence (POE) for a programming module.

## Features Implemented
- Voice greeting – plays a recorded `.wav` message on startup  
- ASCII art logo – “Orbitz” cybersecurity theme with coloured console output  
- Personalised interaction – asks for the user’s name and uses it in responses  
- Keyword responses – recognises: `password`, `phishing`, `safe browsing`, `how are you`, `purpose`, `help`  
- Input validation – handles empty input and unknown queries with friendly fallback messages  
- Enhanced console UI – colours, dividers, spacing, and structured layout  
- Modular code – separated into `Program.cs`, `UI.cs`, `Chatbot.cs`, `AudioPlayer.cs`  
- GitHub version control – minimum 6 meaningful commits  
- Continuous Integration – GitHub Actions workflow that builds the project on every push  

## How to Run the Program

### Prerequisites
- Windows OS (for `System.Media` SoundPlayer)
- .NET 6.0 SDK or later
- Visual Studio 2022 (or any C# IDE)

### Steps
1. Clone or download this repository.
2. Open the project file (`CyberSecurityChatbot.csproj`) in Visual Studio 2022.
3. Ensure the `greeting.wav` file is present and its Copy to Output Directory property is set to Copy Always.
4. Press `Ctrl+F5` (Start Without Debugging) to run.
4. Type `help` to see available topics, or ask about `password`, `phishing`, or `safe browsing`.
5. Type `exit` to quit.

### Example Interaction
