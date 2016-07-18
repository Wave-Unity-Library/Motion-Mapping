# Motion-Mapping
Open-source Unity asset library

<h2> Note: </h2>
  * We're creating an open-source Unity asset library.
  * We'll be releasing a completed version by the end of this month.

<h2> Gyroscopic Camera Controller </h2>
  ![alt tag](http://res.cloudinary.com/jjcodepen/image/upload/v1468705961/GyroscopeController_mu7qac.gif)   
  In the GIF above, we're setting up an environment to use a <b>mobile phone's</b> gyroscope to control the in-game camera. Get started by following the steps below:
  * Download our scripts.
  * Include them in the <b>Assets folder</b> of your Unity library.
  * Select the <b>Main Camera</b> object in your scene.
  * Add our <b>Camera Controller script</b> as a component in the inspector panel.
  * Drag a player object into the Camera Controller component.  
   

<h3> Setting up Unity Remote </h3>
 To use a phone with the Unity editor, download Unity Remote 4 from the app store. Also be sure to modify the mobile device Unity expects. In the Unity editor menubar, visit Edit > Project settings > Editor, and under Unity Remote, change the tab pulldown to your phone. (Android users -- you still have more to configure for unity remote. Find any of the tutorials on google).
 
<h2> Real-Time Audio </h2>
  Allows you to talk with your mates by providing a way to transmit your voice across a multiplayer network.
  
  To get started you'll want to add the following things to any game object you want to be able to talk in real-time:
   * AudioSource object
   * our <b>VoiceController.cs script</b>
  
 Currently, our audio feature is configured to only work with Unity's networking through their HLAPI and was written with Unity 5 in mind. To see an example of this feature being used with the HLAPI check out the 'demo' branch of this repository.

 Configurable Options:
   * Compression (defaults to Zlib)
   * Recording Frequency (defaults to 16000Hz)
   * Packet Sizes
   ...everything really

 We provide multiple compression options such as: Zlib, Opus, NSpeex, A-law with Zlib. 
 
 It's easy to adjust any of the settings you desire for your game. Just go to the <b>VoiceController.cs script</b> and change the variables at the top of the file. For example, if you want to record at higher frequencies, then just change the recordFrequency variable.

<h2> Our Team: </h2>
  * Jessica Ayunani / j.ayunani@gmail.com
  * Michael Laythe / mrlaythe24@aol.com
  * Jeremy Yip / jeremy.yip7@gmail.com
