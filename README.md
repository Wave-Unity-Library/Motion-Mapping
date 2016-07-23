![alt tag](http://res.cloudinary.com/jjcodepen/image/upload/v1469118381/Wave_LogoType_ynoxdo.jpg)  

# Wave: A Unity Asset Library
We want <b>mobile gaming</b> to be a more immersive and interactive experience, so we designed Wave. It's an open source <b>Unity game engine Asset</b> that makes 2 game dev features tremendously more accessible: <i>elements of motion tracking</i> for character control and <i>real-time audio streaming</i> between multiple devices. Check out a 2-minute demo video <a href="https://www.youtube.com/watch?v=56wYgaHN8r8&feature=youtu.be">here</a>. 

## Table of Contents  
[Prerequisites](#prerequisites) </br>
[Getting Started](#getting-started) </br>
[Gyroscopic Camera Controller](#gyro-cam) </br>
[Translational Movement with Accelerometer](#translational-movement) </br>
[Real Time Audio](#real-time-audio) </br>
[Assets Directory](#assets-directory) </br>
[Contribute](#contribute) </br>
[Team](#team)

<h2> <a name="prerequisites"></a> Prerequisites </h2>

   * ![alt tag](http://res.cloudinary.com/jjcodepen/image/upload/c_scale,w_25/v1469119124/20100523235954_Unity_logo_va96rl.png) <a href="https://unity3d.com/get-unity/download">Unity 5</a>
   * ![alt tag](http://res.cloudinary.com/jjcodepen/image/upload/c_scale,w_20/v1469118908/Windows_logo_-_2012_byfabw.png) <b>Windows</b>: 7 SP1+, 8, 10
   * ![alt tag](http://res.cloudinary.com/jjcodepen/image/upload/c_scale,w_25/v1469118917/Apple-icon_lo0q8q.png) <b>iOS</b>: Mac OSX 10.8+ with <a href="https://itunes.apple.com/us/app/xcode/id497799835?mt=12">Xcode</a>
   * ![alt tag](http://res.cloudinary.com/jjcodepen/image/upload/c_scale,w_20/v1469123670/Android_robot.svg_o9anin.png) <b>Android</b>: <a href="https://developer.android.com/studio/index.html">Android SDK</a>

<h2> <a name="getting-started"></a> Getting Started </h2>
  <h3> Setting up Unity Remote </h3>
    To use a phone with the Unity editor, download Unity Remote 4 from the app store. Also be sure to modify the mobile device Unity expects. In the Unity editor menubar, visit Edit > Project settings > Editor, and under Unity Remote, change the tab pulldown to your phone. (Android users -- you still have more to configure for unity remote. Find any of the tutorials on google).
  
<h2> <a name="gyro-cam"></a> Gyroscopic Camera Controller </h2>
   ![alt tag](http://res.cloudinary.com/jjcodepen/image/upload/v1468705961/GyroscopeController_mu7qac.gif)   
   In the GIF above, we're setting up an environment to use a <b>mobile phone's</b> gyroscope to control the in-game camera. Get started by following the steps below:
   * Download our scripts.
   * Include them in the <b>Assets folder</b> of your Unity library.
   * Select the <b>Main Camera</b> object in your scene.
   * Add our <b>Camera Controller script</b> as a component in the inspector panel.
   * Drag a player object into the Camera Controller component.  
  
<h2> <a name="translational-movement"></a> Translational Movement with Accelerometer  </h2>
   ![alt tag](http://res.cloudinary.com/jjcodepen/image/upload/v1469215988/Accelerometer_oh2uj8.gif)  
   The idea of this feature is that when you move in the physical world, your in-game character moves as well. Using information from the mobile phone's accelerometer, we attempt to simulate walking movements in the Unity game engine.  It's still experimental, but it works and may require tweaking depending on the mobile phone. Get started by following the steps below:
   * Download our scripts.
   * Include them in the <b>Assets folder</b> of your Unity library.
   * Select the <b>Player</b> object in your scene.
   * Add our <b>Player Controller script</b> as a component in the inspector panel.
   * Drag a player object into the Camera Controller component.  
  
<h2> <a name="real-time-audio"></a> Real-Time Audio </h2>
   Allows you to talk with your mates by providing a way to transmit your voice across a multiplayer network.
   
   To get started you'll want to add the following things to any game object you want to be able to talk in real-time:
    * AudioSource object
    * our <b>VoiceController.cs script</b>

![alt tag](http://res.cloudinary.com/dhwokgvxt/image/upload/v1469118991/Screen_Shot_2016-07-21_at_9.33.06_AM_v7tqw6.png)  
   
 Currently, our audio feature is configured to only work with Unity's networking through their HLAPI and was written with Unity 5 in mind. To see an example of this feature being used with the HLAPI check out the 'demo' branch of this repository.
  
 Configurable Options:
 * Compression (defaults to Zlib)
 * Recording Frequency (defaults to 16000Hz)
 * Packet Sizes
 ...everything really
 
We provide multiple compression options such as: Zlib, Opus, NSpeex, A-law with Zlib. 

![alt tag](http://res.cloudinary.com/dhwokgvxt/image/upload/v1469118985/Screen_Shot_2016-07-21_at_9.27.54_AM_aho1lh.png)  
  
 It's easy to adjust any of the settings you desire for your game. Just go to the <b>VoiceController.cs script</b> and change the variables at the top of the file. For example, if you want to record at higher frequencies, then just change the recordFrequency variable.
 
<h2> <a name="assets-directory"></a> Assets Directory Structure and Descriptions</h2>
| Name                                       | Description                                                       |
| ------------------------------------------ | ----------------------------------------------------------------- |
| <b>Scripts</b>/CameraController.js         | Use the phone's gyroscope to control the in-game camera.          |
| <b>Scripts</b>/PlayerController.js         | Use the phone's accelerometer to guide in-game movement.          |
| <b>Scripts</b>/VoiceController.cs          | Facilitates real-time voice chat.                                 |
| <b>Scripts</b>/VoiceUtils.cs               | Various compression and typecasting methods for audio info.       |
| <b>Scripts</b>/VoiceCompression.cs         | Provides a list of audio codecs used in VoiceSettings.cs.         |
| <b>Scripts</b>/VoiceSettings.cs            | <b>Edit this file</b> to select and implement different codecs.   |
| <b>Scripts</b>/CircularBuffer.cs           | Used to prevent dynamic memory allocation when sending audio data.|
| <b>Plugins</b>/Ionic.Zlib.dll              | Codec for Zlib audio compression and decompression.               |
| <b>Plugins</b>/NSpeex.dll                  | Codec for Nspeex audio compression and decompression.             |
| <b>Plugins</b>/Snappy                      | Codec for Snappy audio compression and decompression.             |

<h2> <a name="contribute"></a> Contribute </h2> 
   If anything is unclear or unintuitive, feel free to contact us. We heartily welcome pull requests because, again, this is an open source project. Any contributions will be documented.
 
<h2> <a name="team"></a> Our Team </h2>
   * Jessica Ayunani - github.com/jayunani
   * Michael Laythe - github.com/mlaythe
   * Jeremy Yip - github.com/jeyip
