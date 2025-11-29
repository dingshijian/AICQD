AICQD: Your Personal AI Puff Coach 🚭🤖

Note: This project is a submission to the Edge AI Contest hosted by Edge Impulse. It demonstrates a Proof of Concept (PoC) integrating Edge AI gesture recognition into a cross-platform .NET MAUI application designed to monitor vaping usage in real time.

📖 Overview

AICQD is an intelligent, privacy-preserving edge application designed to track puff counts from vaping devices. By analyzing micro-movements captured by onboard sensors, AICQD detects individual vape usage gestures and translates them into puff counts—helping users understand, monitor, and eventually reduce their nicotine consumption.

Unlike current vape devices that store usage data in proprietary firmware or require cloud-linked apps, AICQD runs entirely on the edge, meaning:

✔️ No user data leaves the device
✔️ No cloud dependency
✔️ No battery-draining connectivity requirements

This empowers users who want awareness and control without sacrificing privacy.

🚀 Key Features

Cross-Platform Deployment: Built with .NET MAUI, runs on Android, iOS, and macOS.

Real-Time Puff Tracking: Detects vaping gestures and increments puff counts instantly.

Edge AI Inference: Uses a quantized TensorFlow Lite model trained with Edge Impulse to classify puff motions locally—no internet required.

Privacy First: User data never leaves the device; no raw sensor streams or behavioral logs are uploaded.

Habit Awareness Tools (Planned): Insights, usage analytics, and optional self-set limits.

🧠 Machine Learning Model

AICQD uses Edge Impulse to train a lightweight ML classifier that recognizes puff gestures based on motion signatures.

What We Detect

The model distinguishes vape-related lifting, inhalation, and repositioning motions from unrelated hand movements, allowing accurate puff counting in real time.

Project Link

Edge Impulse Public Project: VIEW MODEL

(Verify visibility before submitting.)

Data Pipeline

Data Collection: Raw accelerometer data was captured at [insert sampling frequency] during vaping-like puff motions.

Signal Processing: Applied [e.g., Spectral Analysis / MFCC features for motion] to isolate signature puff patterns.

Model Architecture: Trained a [e.g., neural network classifier or anomaly detector] using Edge Impulse.

Performance: Initial prototype achieved ~60% accuracy, with improvements planned after expanding the dataset.

🛠️ Tech Stack & Hardware
Component	Choice
Framework	.NET MAUI (.NET 8.0)
Language	C#
ML Platform	Edge Impulse + TensorFlow Lite
Devices	Android and iOS phones (tested on [your device model])
Sensors	3-axis Accelerometer
📂 Repository Structure
AICQD/
├── Models/      # TFLite model files exported from Edge Impulse
├── Services/    # Sensor data acquisition and inference engine
├── Views/       # User interface for real-time puff tracking
└── Data/        # Sample datasets for experimentation

🏃‍♂️ Getting Started
Prerequisites

Visual Studio 2022 with .NET MAUI workload

.NET 8.0 SDK

Clone the Repository
git clone https://github.com/dingshijian/AICQD.git

Build & Deploy

Open AICQD.sln in Visual Studio

Select Android Emulator or physical device

Press F5 to build and run

🌱 Why Puff Counting Matters

Nicotine addiction is reinforced by frequency-based behavior loops—not just dosage. Most users underestimate their actual consumption.

Accurate puff tracking enables:

🟢 Awareness of daily habits
🟡 Recognition of triggers
🔴 Reduction strategies based on real numbers

Future integration with behavioral nudges may support cessation efforts—without shaming, surveillance, or cloud tracking.

🚧 Roadmap

Upcoming milestones for AICQD:

 Integrate C++ Edge Impulse SDK for faster inference

 Expand motion dataset to improve model confidence

 Add "Training Mode" so users can personalize puff signature data

 Implement optional cloud sync for progress sharing

 Connect to Bluetooth-enabled vaping devices (stretch goal)

🤝 Acknowledgements

Huge thanks to Edge Impulse and HackerEarth for providing the tools and opportunity to bring edge-based digital wellness solutions to life.

Built with ❤️ by Shawn Ding / AICQD Team
November 2025
