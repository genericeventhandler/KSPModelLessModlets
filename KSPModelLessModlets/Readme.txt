GOAL:
	Group a load of model less module manager configs into a single repository.

Project:
This project is just a tool to copy the files from the working directory into your KSP Game Data folder. 

You need to set up an evironment variable called KSPGameData and past the url to your gamedata folder for the process to work.

ie.   KSPGameData        D:\Steam\SteamApps\common\Kerbal Space Program\GameData

If you compile and run the app from visual studio, it will check the environment variable is correct and pointed to the correct folder, 
create the folder and then copy all the ModuleManager configs to this directory.

If you don't want the console app to run on your machine, or you are running on linux, just copy the generic event handler folder into the game data folder. 

It is assumed that the latest version of module manager is installed, and no warrenty is given for how this affects your game or machine. 



Modulemanager Syntax  https://github.com/sarbian/ModuleManager/wiki/Module-Manager-Syntax