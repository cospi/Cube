# Cube

Puzzle game where you roll a cube to reach the goal. Work in progress.

## Setup

* Enter PlayMode from the `_Start` scene to run the game.

## Scene Management 

* The `_Start` scene preloads all scenes.
* Scene switches are performed simply by deactivating/activating scenes.

## Level Editor

* The active tool can be selected using the dropdown in the bottom left corner.
* The Paint tool is used to build the level. A level should have a Start and a Goal.
* The Pan tool can be used to move the camera.
* Level size can be adjusted via the Size fields. 
* To load/save a level, press Load/Save. To make levels part of the game, they must be copied over from the `Application.persistentDataPath` directory to the project.

## TODO

* Add move counter.
* Add collectibles and enemies.
* Support safe areas for mobile.
* Create more levels. Currently, there are only a couple of placeholder levels.
* Improve graphics. Current visuals are temporary programmer art.
* Add undo functionality to the level editor.
