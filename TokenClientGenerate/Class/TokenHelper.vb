Imports System.Security.Cryptography
Imports System.Text

Public Class TokenHelper
    Public Shared Property OnlineToken As Dictionary(Of String, TokenDTO)
    Private Shared ReadOnly saltValue As String = "MD5_TEST_Salt-Key"
    Private Shared ReadOnly TokenParam As Integer = 3 ' Count Param for token
    Private Shared ReadOnly TokenTimeLive As Integer = 3 ' 1 Minute

    Private Shared Function GenerateMd5Hash(ByVal ParamArray args() As Object) As String
        Dim x = New MD5CryptoServiceProvider()
        Dim input As String = saltValue & args(0).ToString()
        For i As Integer = 1 To UBound(args)
            input = input & "." & args(i).ToString()
        Next
        'input = saltValue & String.Join(args, ".")
        Dim computeHash = Encoding.UTF8.GetBytes(input.ToLower)
        computeHash = x.ComputeHash(computeHash)
        Dim stringBuilder = New StringBuilder()
        For Each var As Byte In computeHash
            stringBuilder.Append(var.ToString("x2").ToLower())
        Next
        Return stringBuilder.ToString()
    End Function

    Private Shared Function CreateToken(ByVal ParamArray args() As Object) As TokenDTO
        Return New TokenDTO With {
            .SesstionID = args(0).ToString(),
            .TimeCreated = args(1).ToString(),
            .Username = args(2).ToString(),
            .TimeExpired = args(3).ToString(),
            .FunctionName = args(4).ToString(),
            .ActionName = args(5).ToString()}
    End Function

    Public Shared Function GetToken(ByVal ParamArray args() As Object) As TokenDTO
        Dim SesstionID As String = args(0).ToString() '// Sesstion ID .No = 0
        Dim Username As String = args(1).ToString() '// Username .No = 1
        Dim functionName As String = args(2).ToString() '// Function .No = 2 - Function/ Class client
        Dim actionName As String = args(3).ToString() '// Action .No = 3 - Function/ Class API
        '// Time Expired
        Dim timeExpired As String = DateTime.Now.AddMinutes(TokenTimeLive).ToString("yyyyMMddHHmmss")
        Dim timeNow As Long = Long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"))

        Try
            '// Check khoi tao token Cache
            If OnlineToken Is Nothing Then
                OnlineToken = New Dictionary(Of String, TokenDTO)
            End If

            If Not OnlineToken.ContainsKey(SesstionID & "." & functionName) Then
                Return CreateToken(SesstionID, timeNow, Username, timeExpired, functionName, actionName)
            Else
                If (OnlineToken(SesstionID & "." & functionName).TimeExpired < timeNow) Then
                    Return CreateToken(SesstionID, timeNow, Username, timeExpired, functionName, actionName)
                Else
                    Return OnlineToken(SesstionID & "." & functionName)
                End If
            End If
        Catch ex As Exception
            Return CreateToken(SesstionID, timeNow, Username, timeExpired, functionName, actionName)
        End Try
    End Function

    Public Shared Function GenerateToken(ByVal ParamArray args() As Object) As TokenDTO
        Dim SesstionID As String = args(0).ToString() '// Sesstion ID .No = 0
        Dim Username As String = args(1).ToString() '// Username .No = 1
        Dim functionName As String = args(2).ToString() '// Function .No = 2
        Dim actionName As String = args(3).ToString() '// Action .No = 3

        Dim _token As TokenDTO = GetToken(SesstionID, Username, functionName, actionName)
        If (_token.Token Is Nothing) Then
            _token.Token = GenerateMd5Hash(SesstionID, _token.TimeCreated, Username, _token.TimeExpired, functionName, actionName)
            If OnlineToken.ContainsKey(SesstionID & "." & functionName) Then OnlineToken.Remove(SesstionID & "." & functionName)
            OnlineToken.Add(SesstionID & "." & functionName, _token)
        End If

        Return _token
    End Function

    Public Shared Function CheckToken(ByVal _token As TokenDTO, ByVal sFunctionName As String) As Boolean '// Check token tai API server
        Try
            '// Token is not valid
            If Not OnlineToken.ContainsKey(_token.SesstionID & "." & _token.FunctionName) Then
                Return False
            End If

            Dim Token As String = GenerateMd5Hash(_token.SesstionID, _token.TimeCreated, _token.Username, _token.TimeExpired, _token.FunctionName, _token.ActionName)
            If (Token <> _token.Token) Then
                Return False
            End If

            Dim timeNow As Long = Long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"))
            '// Time is false
            If (_token.TimeExpired < timeNow) Then
                OnlineToken.Remove(_token.SesstionID & "." & _token.FunctionName)
                Return False
            End If

            '// Function is false
            If (sFunctionName <> _token.ActionName) Then
                Return False
            End If

            Return True
        Catch ex As Exception
            CheckToken = False
        End Try
    End Function

End Class
