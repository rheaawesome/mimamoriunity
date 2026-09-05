# mimamoriunity
Mimamori — Wearable Heat Guard for Dementia Care

Mimamori is a wearable wristband concept designed to help protect people living with dementia from heat-related health risks. Because dementia can impair a person's ability to recognize or communicate that they're overheating, Mimamori monitors ambient and skin temperature and can alert a caregiver before a dangerous situation develops.

This repository contains an interactive Unity simulation of the device — built for a hackathon, without access to physical hardware — that lets you explore the wristband's design and see how a heat-alert scenario would play out in real life.

What's in the simulation
3D product tour — the camera automatically flies to each hardware feature on the wristband (speaker, help button, ambient sensor vents, PPG/skin-temp sensor + charging contacts, buckle, strap keeper), with captions explaining what each part does.
Live part highlighting — each feature glows as the camera reaches it, so it's obvious which physical component is being described.
Heat alert demo — after the tour, the simulation triggers a mock "help requested" alert, showing the pulsing warning overlay and status text a caregiver would see if the device detected a risk.
Built with
Unity 6.5 (6000.5.5f1)
C#
Universal Render Pipeline (URP)
Running it
Clone this repository.
Open the project folder in Unity Hub (Unity 6.5 or later recommended).
Open Assets/Scenes and load the main scene.
Press Play — the product tour and heat alert demo will run automatically.
Project structure
Assets/ — scripts, scenes, materials, and the wristband 3D model
ProductTourController.cs — drives the camera tour, captions, and highlight sequence
HeatAlertController.cs — handles the simulated heat-alert overlay and status text
