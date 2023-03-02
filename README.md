# RenderOff

![image](https://user-images.githubusercontent.com/20557088/222347715-7c91d027-fbc0-4e5e-9fe7-8d719de1bde4.png)

Mod for Unity online games that, depending on the selected method, reducing the GPU usage.
This mod is well suited for laptops with weak integrated video graphics and is not suitable for gaming in VR mode.

## Usage
There are 2 optimizing methods:
* `Aggressive` method disables all active cameras in the game. GPU usage drops to zero. Can break some game scripts that uses cameras.
* `NonBuggy` method make cameras render only one pixel. It significantly reduces the GPU usage. Does not break game scripts.

Press `Numpad-` button to switch the method.

## Installation
* Install [latest MelonLoader](https://github.com/LavaGang/MelonLoader).
* Download mod from [release page](https://github.com/LeakyRUS/RenderOff/releases).
* Put `RenderOff.dll` into `<GameFolder>\Mods`.

## Build
* Install [latest MelonLoader](https://github.com/LavaGang/MelonLoader).
* Fix references in `RenderOff.csproj`.
* Build.
