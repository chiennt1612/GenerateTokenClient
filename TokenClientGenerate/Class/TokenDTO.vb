Public Class TokenDTO
    Public Token As String '// MD5 + Salt = Private Key
    Public SesstionID As String
    Public Username As String
    Public TimeCreated As Long
    Public TimeExpired As Long
    Public FunctionName As String
    Public ActionName As String
End Class