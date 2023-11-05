# RequestReply

## Introduction
This project provide an abstraction and a sample implementation for **Synchonous Request-Reply Pattern over asynchronous backbone** in C#.Net6.  
I also wrote an article in the following address that completely explains **Synchonous Request-Reply Pattern over asynchronous messaging services**:  
https://www.linkedin.com/pulse/achieving-synchronous-communication-over-asynchronous-amir-doosti-nr0ff  
   
## Structure of soution
The solution contains one Console App project which is written in Visual Studio.  
When you run program, it send a message conatining a message id and a text message and ask you to reply to it. You will provide same message id and a text reply.  
If the correlation id doesn't match the message id, it repleat its question.  
You can use RabitMQ, Kafka or any messaging instead.  

## Technology stack
- OS: Windows 10 Enterprise - 64 bits
- IDE: Visual Studio Enterprise 2022 (64 bits) - version 17.2.5
- Framework: .Net 6
- Language: C#

## How to run
Open the solution in Visual Studio and run it using F5. You will see the output in a console.

