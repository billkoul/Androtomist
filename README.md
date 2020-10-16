# Androtomist

Androtomist is a novel hybrid analysis toolcapable of performing both static and dynamic analysis of applications on the Android platform. Unlike similar tools, Androtomist makes use of a plethora of features stemming from static analysis along with dynamic instrumentation to analyse applications and decide if they are benign or not.

The application itself is dual mode, i.e., fully automated for the novice user and configurable for the expert one.

## Licence 
Androtomist's source code is offered under the European Union Public Licence (https://ec.europa.eu/info/european-union-public-licence_en)

Please cite our paper:<br />
<a href="https://www.mdpi.com/2073-8994/12/7/1128">Kouliaridis, V.; Kambourakis, G.; Geneiatakis, D.; Potha, N. Two Anatomists Are Better than One—Dual-Level Android Malware Detection. Symmetry 2020, 12, 1128.</a>

## Dependencies 
.NET CORE 2.1 : https://dotnet.microsoft.com/download/dotnet-core/2.1 <br />
PostgreSQL Database :https://www.postgresql.org/ <br />
VirtualBox: https://www.virtualbox.org/ <br />
SDK Platform Tools: https://developer.android.com/studio/releases/platform-tools <br />
Apktool: https://ibotpeaches.github.io/Apktool/ <br />
aapt: https://androidaapt.com/
Android OS for VB, preferably android-x86_64-7.1: https://www.android-x86.org/ <br />


## Installation
*Folder DB contains the create scripts to build the database (PostgreSQL).*<br />
*Folder Tools will contain the frida scripts, Android platform-tools and Apktool.*<br />
*Folder AndroidOS will contain the Androidx86 to load with Virtualbox.*<br />
*Folder App contains the .net core 2.1 web app which automates the process.*<br />

Steps:
1. Run the create scripts to build the database<br />
2. Download the Android platform-tools, aapt, Apktool (place in the same folder as the ADB exe), and install Frida<br />
3. Download an Androidx86 image<br />
4. Install VirtualBox<br />
5. load your Androidx86 image, after loading the OS create a snapshot which will be used as a reset point and add the name of the VB image and snapshot to Models/Processing/Terminal.cs<br />
6. Run the App with Visual Studio after filling in all columns of the A_INFO table<br />


## Environment
![home](https://raw.githubusercontent.com/billkoul/Androtomist/master/1.PNG)

![report](https://raw.githubusercontent.com/billkoul/Androtomist/master/2.PNG)
