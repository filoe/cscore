## Using the Project ##
Make sure that the directory with the native libraries is placed correctly within the folder with your assembly.

### Linux & Mono ###
Make sure to start your assembly with LD_LIBRARY_PATH=./ mono MyApp.exe
To debug your assembly using MonoDevelop follow these steps:
1. Open the "Project Options" of your project
2. Navigate to Run > General
3. Add a new variable with the name "LD_LIBRARY_PATH" and use your output directory (which contains your compiled assembly) as the variables value

## Important: ##

The CSCore.Ffmpeg project is licensed under the **[LGPL2.1](https://www.gnu.org/licenses/old-licenses/lgpl-2.1.html)**

CSCore.Ffmpeg uses parts of https://github.com/Ruslan-B/FFmpeg.AutoGen
The author "Ruslan Balanukhin" gave the explicit permission to use its source code
within CSCore.Ffmpeg under the MS-PL or the LPGL.
> I do not mind if you would like to reuse some results of my work in case you will keep reference to my original project.

This software uses libraries from the FFmpeg project under the LGPLv2.1

This software uses code of [FFmpeg](http://ffmpeg.org) licensed under the 
[LGPLv2.1](http://www.gnu.org/licenses/old-licenses/lgpl-2.1.html>LGPLv2.1) and 
its source can be downloaded [here](https://github.com/filoe/cscore) or [here](https://github.com/filoe/cscore/tree/ffmpeg).