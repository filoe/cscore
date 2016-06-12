CSCore.Ffmpeg uses parts of https://github.com/Ruslan-B/FFmpeg.AutoGen
The author "Ruslan Balanukhin" gave the explicit permission to use its source code
within CSCore.Ffmpeg under the MS-PL or the LPGL.

In order to use this project, you need to include the ffmpeg libraries. 
You can download them from https://www.ffmpeg.org.
Important: The used ffmpeg libraries have to be compatible with 
the wrapper of CSCore.Ffmpeg. 
The current version of CSCore.Ffmpeg is compatible with the binaries
shipped with release 3.0 of FFmpeg.AutoGen which can be downloaded from
https://github.com/Ruslan-B/FFmpeg.AutoGen/releases/tag/3.0.
Make sure to place the x86 binaries into the "{path of your CSCore.Ffmpeg.dll}/ffmpeg/3.0.2/x86/" folder 
and the x64 binaries into the "{path of your CSCore.Ffmpeg.dll}/ffmpeg/3.0.2/x64/" folder.

CSCore.Ffmpeg does not contain ffmpeg in any form (compiled or source).
The author of CSCore.Ffmpeg is not responsible for any usage of ffmpeg 
in any application. The user of CSCore.Ffmpeg is responsible for not breaking the
license of ffmpeg.