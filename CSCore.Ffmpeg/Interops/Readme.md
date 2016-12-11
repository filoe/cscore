## NOTE: ##

The bindings for the ffmpeg libraries are taken from https://github.com/Ruslan-B/FFmpeg.AutoGen.
The author Ruslan-B gave the explicit permission to include these files to CSCore.
> I do not mind if you would like to reuse some results of my work in case you will keep reference to my original project.

Changes made to FFmpeg.AutoGen sources:
- included the binding files for avutil, swresample, avcodec and avformat in the CSCore.Ffmpeg project
- changed the namespace from FFmpeg.AutoGen to CSCore.Ffmpeg.Interops
- replaced all "public" modifier with the "internal" keyword
- changed AVFrame->@extended_data from sbyte** to byte**
- included the InteropHelper class (of the FFmpeg.AutoGen.Example project) in the CSCore.Ffmpeg project
- marked the InteropHelper class as internal and replaced the string.IsNullOrWhitespace method call with string.IsNullOrEmpty

# THANKS to Ruslan! #