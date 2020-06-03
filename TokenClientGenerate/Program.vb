Imports System
Imports Newtonsoft.Json

Module Program
    Sub Main(args As String())
        Console.WriteLine("Welcom to generate Token Client -> Call API Server Check Token!")
        '// Gen token o client nham dam bao du lieu ko bi thay doi
        Dim sessionID = Guid.NewGuid().ToString
        Dim UserID = Guid.NewGuid().ToString
        testGenerateToken1(sessionID, UserID)

        sessionID = Guid.NewGuid().ToString
        UserID = Guid.NewGuid().ToString
        testGenerateToken2(sessionID, UserID)
    End Sub

    Sub testGenerateToken1(ByVal ParamArray args() As Object)
        Dim FunctionName As String = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name.ToString() & "_" &
                                            System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()
        '// Generate token
        Dim Token = TokenHelper.GenerateToken(args(0), args(1), FunctionName, "testCheckToken1")
        Console.WriteLine("Generate Token with param: " & Join(args, vbCrLf))
        Console.WriteLine("Token: " & JsonConvert.SerializeObject(Token).ToString())

        Console.WriteLine("Check token with API 1")
        testCheckToken1(Token)
        Console.WriteLine("Check token with API 2")
        testCheckToken2(Token)
    End Sub

    Sub testGenerateToken2(ByVal ParamArray args() As Object)
        Dim FunctionName As String = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name.ToString() & "_" &
                                            System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()
        '// Generate token
        Dim Token = TokenHelper.GenerateToken(args(0), args(1), FunctionName, "testCheckToken2")
        Console.WriteLine("Generate Token with param: " & Join(args, Environment.NewLine))
        Console.WriteLine("Token: " & JsonConvert.SerializeObject(Token).ToString())

        Console.WriteLine("Check token with API 1")
        testCheckToken1(Token)
        Console.WriteLine("Check token with API 2")
        testCheckToken2(Token)
    End Sub

    Sub testCheckToken1(ByVal token As TokenDTO)
        Dim actionName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()
        If (TokenHelper.CheckToken(token, actionName)) Then
            Console.WriteLine("Check Token: " & JsonConvert.SerializeObject(token).ToString() & Environment.NewLine & " Is OK")
        Else
            Console.WriteLine("Check Token: " & JsonConvert.SerializeObject(token).ToString() & Environment.NewLine & " Is Fail")
        End If
    End Sub

    Sub testCheckToken2(ByVal token As TokenDTO)
        Dim actionName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString()
        If (TokenHelper.CheckToken(token, actionName)) Then
            Console.WriteLine("Check Token: " & JsonConvert.SerializeObject(token).ToString() & Environment.NewLine & " Is OK")
        Else
            Console.WriteLine("Check Token: " & JsonConvert.SerializeObject(token).ToString() & Environment.NewLine & " Is Fail")
        End If
    End Sub
End Module


