#include <xaudio2.h>
#include <stdlib.h>
#include <stdio.h>
#include <math.h>

#pragma comment(lib, "Xaudio2.lib")

#define PI 3.14159265358979323846

class VoiceCallback : public IXAudio2VoiceCallback
{
public:
	VoiceCallback() { }
	~VoiceCallback(){ }

	//Called when the voice has just finished playing a contiguous audio stream.
	void __stdcall OnStreamEnd() 
	{
		printf("OnStreamEnd");
	}

	//Unused methods are stubs
	void __stdcall OnVoiceProcessingPassEnd() 
	{
		printf("OnVoiceProcessingPassEnd\n"); 
	}
	void __stdcall OnVoiceProcessingPassStart(UINT32 SamplesRequired) 
	{ 
		printf("OnVoiceProcessingPassStart\n"); 
	}
	void __stdcall OnBufferEnd(void * pBufferContext)    { printf("OnBufferEnd"); }
	void __stdcall OnBufferStart(void * pBufferContext) { printf("OnBufferStart"); }
	void __stdcall OnLoopEnd(void * pBufferContext) { printf("OnLoopEnd"); }
	void __stdcall OnVoiceError(void * pBufferContext, HRESULT Error) { printf("OnVoiceError"); }
};

int main()
{
	CoInitializeEx(NULL, COINIT_MULTITHREADED);

	IXAudio2* xaudio;
	IXAudio2MasteringVoice* masteringVoice;
	IXAudio2SourceVoice* sourceVoice;
	VoiceCallback callback;
	WAVEFORMATEX waveFormat;

	waveFormat.cbSize = 0;
	waveFormat.nAvgBytesPerSec = 352800;
	waveFormat.wBitsPerSample = 32;
	waveFormat.nBlockAlign = 8;
	waveFormat.nChannels = 2;
	waveFormat.wFormatTag = WAVE_FORMAT_IEEE_FLOAT;
	waveFormat.nSamplesPerSec = 44100;

	int bufferSize = 21168000 / 30; //60 sec

	HRESULT hr = XAudio2Create(&xaudio);
	hr = xaudio->StartEngine();
	hr = xaudio->CreateMasteringVoice(&masteringVoice);
	hr = xaudio->CreateSourceVoice(&sourceVoice, &waveFormat, 0, 2.0F, &callback);

	float* data = new float[bufferSize / sizeof(float)];
	int numberOfSamples = (bufferSize / sizeof(float)) / waveFormat.nBlockAlign;

	int j = 0;
	for (int i = 0; i < numberOfSamples; i++)
	{
		double vibrato = cos(2 * PI * 10.0 * i / waveFormat.nSamplesPerSec);
		float value = (float)(cos(2 * PI *(220.0 + 4.0*vibrato)*i / waveFormat.nSamplesPerSec)*0.5);
		data[j++] = value;
		data[j++] = value;
	}

	XAUDIO2_BUFFER buffer;
	ZeroMemory(&buffer, sizeof(buffer));
	buffer.AudioBytes = bufferSize;
	buffer.pAudioData = (BYTE*)data;

	hr = sourceVoice->SubmitSourceBuffer(&buffer);
	hr = sourceVoice->SubmitSourceBuffer(&buffer);

	buffer.Flags = XAUDIO2_END_OF_STREAM;

	hr = sourceVoice->SubmitSourceBuffer(&buffer);
	hr = sourceVoice->Start();

	system("pause");

	delete[] data;

	return 0;
}