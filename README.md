# AICQD
AICQD App
# AICQD: Your Personal AI Edge Coach 🏋️‍♂️🤖

[![Edge Impulse](https://raw.githubusercontent.com/edgeimpulse/otp-firmware-mbed/master/assets/logo-small.png)](https://studio.edgeimpulse.com/studio/811836)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform](https://img.shields.io/badge/.NET%20MAUI-8.0-purple)](https://dotnet.microsoft.com/en-us/apps/maui)

> **Note:** This project is a submission for the **Edge AI contest by Edge Impulse**. It serves as a Proof of Concept (PoC) demonstrating the integration of Edge AI motion classification into a cross-platform .NET MAUI application.

## 📖 Overview

**AICQD** is an intelligent edge application designed to assist users in [Insert specific goal, e.g., monitoring physical exercises / analyzing gestures] without the need for constant cloud connectivity.

By leveraging **Edge Impulse**, we have trained a lightweight machine learning model capable of running on mobile devices (Android/iOS) to detect anomalies and classify movements in real-time using accelerometer data.

## 🚀 Key Features

* **Cross-Platform Architecture:** Built with **.NET MAUI** to run seamlessly on Android, iOS, and macOS.
* **Edge AI Inference:** Uses a quantized TensorFlow Lite model trained via Edge Impulse for low-latency offline inference.
* **Real-time Analysis:** Processes sensor data (Accelerometer/Gyroscope) instantly to provide user feedback.
* **Privacy First:** All data processing happens on the device (Edge); no raw sensor data is sent to the cloud.

## 🧠 The Machine Learning Model

The core intelligence of AICQD is powered by Edge Impulse. We focused on [mention the type of data, e.g., motion/vibration] data to distinguish between different states.

### Project Links
* **Edge Impulse Public Project:** [**CLICK HERE TO VIEW THE MODEL**](https://studio.edgeimpulse.com/studio/811836) 
    *(Please ensure this link is correct)*

### Data Pipeline
1.  **Data Collection:** We collected raw data using [e.g., Mobile Phone Accelerometer @ 62.5Hz].
2.  **Signal Processing:** Applied [e.g., Spectral Analysis] to extract frequency and power characteristics.
3.  **Model Architecture:** Trained a [e.g., Neural Network / K-means Anomaly Detection] to classify the input.
4.  **Results:** Achieved an accuracy of 60% on the test set.

## 🛠️ Tech Stack & Hardware

* **Framework:** .NET MAUI (.NET 8.0)
* **Language:** C#
* **ML Platform:** Edge Impulse
* **Target Devices:** Android Phones, iOS Devices (tested on [Your Device Name])
* **Sensors Used:** Accelerometer (3-axis)

## 📂 Repository Structure

* `AICQD/` - Main solution folder.
* `Models/` - Contains the `.tflite` model files exported from Edge Impulse.
* `Services/` - Handles sensor data acquisition and inference logic.
* `Views/` - UI implementation for the mobile app.
* `Data/` - Sample datasets used for local testing.

## 🏃‍♂️ How to Run (Development)

To run this prototype locally:

1.  **Prerequisites:**
    * Visual Studio 2022 (with .NET MAUI workload installed).
    * .NET 8.0 SDK.
2.  **Clone the Repo:**
    ```bash
    git clone [https://github.com/dingshijian/AICQD.git](https://github.com/dingshijian/AICQD.git)
    ```
3.  **Build:**
    * Open `AICQD.sln` in Visual Studio.
    * Select your target (e.g., Android Emulator or Local Device).
    * Press F5 to build and deploy.

## 🚧 Roadmap & Future Improvements

This submission represents the **Phase 1** prototype. Future development plans include:

* [ ] Full integration of the C++ inference SDK for higher performance.
* [ ] Expanding the dataset to include [mention other exercises/movements].
* [ ] Adding a "Training Mode" to allow users to collect their own data within the app.
* [ ] Cloud synchronization for user progress tracking (optional).

## 🤝 Acknowledgements

* Thanks to **Edge Impulse** and **HackerEarth** for organizing the Edge AI Contest.
* Built with ❤️ using .NET MAUI.

---
*Created by [Shawn Ding/AICQD Team] - Nov 2025*
