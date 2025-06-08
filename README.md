# MacSimSquash

MacSimSquash is a physics-based simulation tool developed in Unity for modeling and analyzing the projectile motion of a squash ball after being launched from a robotic paddle machine. It incorporates realistic parameters including gravity, mass, COR (coefficient of restitution), spring constants, and angular zones on squash court maps.

## Features

### 1. Simulation & Physics Engine

* Calculates projectile trajectory using customizable physics parameters.
* Supports collision simulation, paddle impulse, and real-time ball animation.
* Adjustable input: spring constant, angle, COR, and drop height.
* Calculates optimal paddle angle and time to impact.

### 2. Target Zone & Map Support

* Supports 3 different squash court map configurations:

  * Map 1: Linear zones with fixed distances.
  * Map 2: Radial zones with dual color bands (millimeter precision).
  * Map 3: Angular target zones with scoring sectors.
* Visualization of target zones, impact zones, and marker placement.

### 3. UI & Interaction

* Input forms for target parameters (X, Y), COR, spring constant.
* Real-time simulation visualization and time tracking.
* Top-view camera auto-fits the map dynamically.
* User navigates through Start Menu → Map Selection → Simulation View.

### 4. Camera & Control

* FPS movement (WASD + Mouse).
* Toggle top-view camera.
* Simulation trigger via key input (e.g., press `E` to start).
* Dynamic camera resizing based on map geometry.

---

## Getting Started

### Prerequisites

* Unity Editor (2021.3+ recommended)
* .NET 4.x scripting runtime
* Input System package enabled

### Installation

1. Clone or download the repository.
2. Open the Unity project.
3. Make sure all scenes (`StarterScene`, `MapSelectScene`, `MainGameScene`, `SettingsScene`) are added to the Build Settings.

### Running the Project

* Launch `StarterScene` and click “Start”.
* Select a map in the `MapSelectScene`.
* Proceed to the simulation in `MainGameScene`.

---

## File Structure

```
Assets/
│
├── Scripts/
│   ├── UIManager.cs
│   ├── BallDropper.cs
│   ├── TrajectorySimulator.cs
│   ├── BallTimeTracker.cs
│   ├── MapSelectManager.cs
│   ├── MapLoader.cs
│   ├── StarterMenu.cs
│   └── TopViewCameraController.cs
│
├── Models/
│   └── Map/
│       ├── Map1.prefab
│       ├── Map2.prefab
│       └── Map3.prefab
│
├── Scenes/
│   ├── StarterScene.unity
│   ├── MapSelectScene.unity
│   ├── MainGameScene.unity
│   └── SettingsScene.unity
```

---
