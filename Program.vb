Module Program

    Sub Main()
        Dim tileGame As New Game()
        tileGame.ShowIntro()
        tileGame.Play()
    End Sub

    Public Class Game
        Public Levels(9) As Level
        Private Const USER_GAVE_UP = -1
        Private Const USER_WANTS_HELP = -2

        Sub New()
            For i As Integer = 0 To 9
                Levels(i) = New Level With {.Number = i + 1}
                Levels(i).SetRandomColors(i + 1)
            Next
        End Sub

        Sub ShowIntro()
            Console.Clear()
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("===[Tiles game]===" & vbCrLf)
            Console.WriteLine("Remember the numbers for each color!" & vbCrLf)
            For c As Integer = 1 To 14
                Console.BackgroundColor = c
                Console.ForegroundColor = ConsoleColor.Black
                Console.Write([Enum].GetName(GetType(ConsoleColor), c).PadRight(20))
                Console.ForegroundColor = ConsoleColor.White
                Console.BackgroundColor = ConsoleColor.Black
                Console.Write("= " & c & vbCrLf)
            Next
            Console.WriteLine(vbCrLf & "When ready press any key when ready to play!")
            Console.ReadKey()
            Console.Clear()
        End Sub

        Sub Play()
            Dim input As Integer
            Dim numberOfMoves As Integer
            Dim totalNumberOfMoves As Integer
            Dim level As Integer
            Do
                DrawLevel(level)
                Console.Write(vbCrLf &
                              "Please enter color number (1-14) that exist in map above then press [ENTER]" & vbCrLf &
                              "Press H and [ENTER] for help with color numbers. " & vbCrLf &
                              "Press any other key and [ENTER] to quit." & vbCrLf & "> ")
                input = GetUserInput(Console.ReadLine(), 14)
                If input = USER_GAVE_UP Then
                    Console.WriteLine(vbCrLf & "You gave up after {0} moves - game ended!", totalNumberOfMoves + numberOfMoves)
                    Exit Sub
                ElseIf input = USER_WANTS_HELP Then
                    ShowIntro()
                    Continue Do
                End If
                numberOfMoves += 1
                SelectColors(input, level)
                If GameFinished() Then
                    totalNumberOfMoves += numberOfMoves
                    Console.WriteLine(vbCrLf & "You finished the game in " &
                                      totalNumberOfMoves & " moves!")
                    Exit Sub
                ElseIf LevelFinished(level) Then
                    DrawLevel(level)
                    Console.WriteLine(vbCrLf & "You finished level " & level + 1 &
                                      " in " & numberOfMoves & " moves!")
                    level += 1
                    totalNumberOfMoves += numberOfMoves
                    numberOfMoves = 0
                    If level > Levels.Count - 1 Then
                        Console.WriteLine(vbCrLf & "Game Over - No more levels!")
                        Exit Sub
                    Else
                        Console.WriteLine(vbCrLf & "Press any key to advance to the next level!")
                        Console.ReadKey()
                    End If
                End If
            Loop While input < 15
        End Sub

        Private Sub SelectColors(selectColor As Integer, activeLevel As Integer)
            Dim level As Level = Levels(activeLevel)
            For y As Integer = level.Map.GetLowerBound(0) To level.Map.GetUpperBound(0)
                For x As Integer = level.Map.GetLowerBound(1) To level.Map.GetUpperBound(1)
                    If level.Map(y, x).Color = selectColor Then
                        level.Map(y, x).Selected = True
                    End If
                Next
            Next
        End Sub

        Private Sub DrawLevel(activeLevel As Integer)
            Console.Clear()
            Dim level As Level = Levels(activeLevel)
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("===[Level #{0}]===" + vbCrLf, level.Number + 1)
            For y As Integer = level.Map.GetLowerBound(0) To level.Map.GetUpperBound(0)
                For x As Integer = level.Map.GetLowerBound(1) To level.Map.GetUpperBound(1)
                    If level.Map(y, x).Selected Then
                        Console.BackgroundColor = ConsoleColor.Green
                    Else
                        Console.BackgroundColor = ConsoleColor.Black
                    End If
                    Console.ForegroundColor = level.Map(y, x).Color
                    Console.Write("* ")
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = ConsoleColor.White
                Next
                Console.WriteLine()
            Next
        End Sub

        Private Function LevelFinished(activeLevel As Integer) As Boolean
            Dim level As Level = Levels(activeLevel)
            For y As Integer = level.Map.GetLowerBound(0) To level.Map.GetUpperBound(0)
                For x As Integer = level.Map.GetLowerBound(1) To level.Map.GetUpperBound(1)
                    If Not level.Map(y, x).Selected Then Return False
                Next
            Next
            Return True
        End Function

        Private Function GameFinished() As Boolean
            For level As Integer = 0 To Levels.Count - 1
                If Not LevelFinished(level) Then
                    Return False
                End If
            Next
            Return True
        End Function

        Private Function GetUserInput(input As String, maxNum As Integer) As Integer
            If input.ToUpper() = "H" Then Return USER_WANTS_HELP
            Dim num As Integer
            If Integer.TryParse(input, num) Then
                If (num > 0 And num < (maxNum + 1)) Then Return num Else Return USER_GAVE_UP
            Else
                Return USER_GAVE_UP
            End If
        End Function

        Class Level
            Public Number As Integer
            Public Map(9, 9) As Tile

            Public Sub SetRandomColors(maxNumberOfColors As Integer)
                Randomize()
                Dim levelColors(maxNumberOfColors) As Integer
                Dim colorValue As Integer
                For c = 0 To maxNumberOfColors
                    levelColors(c) = CInt(Int(14 * Rnd()) + 1)
                Next
                For y As Integer = Map.GetLowerBound(0) To Map.GetUpperBound(0)
                    For x As Integer = Map.GetLowerBound(1) To Map.GetUpperBound(1)
                        Map(y, x) = New Tile
                        colorValue = levelColors(CInt(Int(maxNumberOfColors * Rnd())))
                        Map(y, x).Color = colorValue
                    Next
                Next
            End Sub

        End Class

        Class Tile
            Property Color As ConsoleColor
            Property Selected As Boolean
        End Class

    End Class

End Module