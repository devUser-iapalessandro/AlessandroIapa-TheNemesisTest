# Alessandro Iapà - The Nemesis Test

> Alessandro Iapà - The Nemesis Test is a small Unity project made by [Alessandro Iapà](https://alessandroiapo99.wixsite.com/gameprogrammer) which features a small, sort of, online soccer multiplayer gameplay

## Project Requirements

* Engine Version: **Unity 2021.3.3f1**
* Any text/code editor/IDE in order to view the scripts 
* Internet Connection (it is an online game)
* (A PC... I guess...)

## External Tools and Packages

* TextMesh Pro - [Documentation](https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/index.html)
* Input System - [Documentation](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.3/manual/index.html)

## Project Structure

Apart from Unity's default folders and assets, you will be able to take a look at what I've made by looking at these folders:

* **Data**
    * **Teams**
* **Materials**
    * **GoalSkins**
    * **Physics**
    * **PlayerSkins**
* **Prefabs**
* **Resources**
* **Scenes**
* **Scripts**
    * **Editor**
    * **Runtime**
        * **Arena**
        * **Data**
        * **NetworkSystems**
        * **Player**
        * **UI**
        * **Utility**

> Note: by taking a look at the *Scripts* folder you will be able to know the structure of the namespaces I've created for the project, in order to have a 1:1 correspondence, and for further expandability.

## How to use it

If you want to take a look at the code or the assets I produced, you can just freely move through the folders of the project.

If your will is to play the game, you have different options to do so:

1. Launch the game from inside the editor by pressing the Play button and ask someone else to do the same on their machine (obviously you will need to install the project and its requirements on the other's machine).
Start from the **MainMenu** scene and have fun.

2. Build the game's executable (*File>Build Settings>Build*) or by pressing *CTRL+B* to do a quick build and run, and in the editor press the play button. It might be useful if you have no one to play the game with, but the experience can't be the same as for humans it is very hard to do two things at once.

3. Build the game and hand the built game to someone to play it with on another machine, on your machine you can either launch the built version itself or, again, play it in the editor.

## Current Features

Following the client's instructions, here you have the main features of the game: 

* Starting from the Main Menu, you will be able to choose your team as soon as an opponent is found.
When choosing a team, you and your opponent will be able to see each other choice.
Once you are done, you can press the *ready* button only if both of you have chosen a different team.

* By entering the Game scene, you will be able to control your character and hit the ball to send it into the opponent's goal. Be aware of not send your ball to your goal or you will give the other player a free point.

* As soon as a goal is scored, proper messages are displayed in the HUD and players are respawned as well as the ball and the goals.

* As soon as a player reaches three points, it is declared winner and the game ends. By pressing the *Back to the menu* button you will be able to start a new game from the teams' choice.

## Known issues

* Depending on your connection, or on the traffic on the network, you might experience a laggy movement of both players and the ball, it might be improved by implementing a custom solution for serializing and sending data across the network in the most optimized way.

* Disconnecting from a match might trigger callbacks a bit late due to the network system, so it might be useful to implement systems that avoid the player to take further actions until the disconnection procedure is done *(i.e. loading screens that block inputs, etc...)*

* Goals might spawn being overlapped or even just under the ball's spawn point, so that when the ball falls down at the start of a round it instantly scores a goal and a new round starts. It is a bit annoying but it might be resolved by implementing a system that spawns a goal avoiding certain areas, as well as a systems that checks for overlaps either spawns the goals again or moves them away from each other.
