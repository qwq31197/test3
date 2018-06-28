Imports System
Imports ConsoleApp1.com.chang.utils

Module Program
    Sub Main(args As String())
        Dim dig As String = "11&@&22"
        Dim dig2() As String = dig.Split("&@&")
        'dig.HmacSign("MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAIXs4wIYCf", "4f34aeb280bf4c5b917d1d23a0e55230")
        Dim a As String = dig2(0)
        Dim b As String = dig2(1)
    End Sub
End Module
