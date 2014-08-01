Option Strict On
Imports CSCore
Imports CSCore.SoundOut
Imports CSCore.Codecs
Imports CSCore.Streams
Imports System.Windows.Forms

Public Class Form1
    Private soundOut As ISoundOut
    Private soundSource As IWaveSource
    Private isPositionScrolling As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim source As IWaveSource = Nothing
        Try
            source = CodecFactory.Instance.GetCodec("C:\Temp\test.mp3")
        Catch ex As Exception
            MessageBox.Show("Dateityp wird nicht unterstützt.")
            Return
        End Try
        Dim sampleSource As ISampleSource = New NotificationSource(source)
        CType(sampleSource, NotificationSource).Interval = 100 '100 ms interval
        AddHandler CType(sampleSource, NotificationSource).BlockRead, AddressOf OnBlockRead
        Dim source1 = sampleSource.ToWaveSource(16)
        source1 = New LoopStream(source1)
        soundSource = source1
        soundOut = New DirectSoundOut()
        soundOut.Initialize(source1)
        soundOut.Play()
    End Sub

    Private Sub OnBlockRead(ByVal sender As Object, ByVal e As BlockReadEventArgs(Of Single))
        Dim source As IWaveSource = soundSource
        Dim text = String.Format("{0:mm\:ss} \ {1:mm\:ss}", source.GetPosition(), source.GetLength())
        Dim v As Double = CType(source.Position, Double) / CType(source.Length, Double)
        Me.BeginInvoke(Sub()
                           Label1.Text = text
                       End Sub)
    End Sub
End Class