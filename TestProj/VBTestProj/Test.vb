Option Strict On

Imports CSCore
Imports CSCore.Codecs
Imports CSCore.DSP
Imports System.Drawing

Module Test

    Sub Main()
        Console.WriteLine(CheckNearest().ToString())
        Console.ReadKey()
    End Sub

    Private Function CheckNearest() As Point
        Dim loc = New Point(5, 5)
        Dim points = New List(Of Point)
        points.Add(New Point(25, 0))
        points.Add(New Point(34, 0))
        points.Add(New Point(44, 0))
        points.Add(New Point(22, 7))
        points.Add(New Point(51, 7))

        Dim pts = points.Where(Function(x) x <> loc).OrderBy(Function(x) Math.Pow(x.Y - loc.Y, 2)).ThenBy(Function(x) Math.Pow(x.X - loc.X, 2))

        Return pts.First()
    End Function
End Module