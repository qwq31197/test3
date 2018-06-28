'Imports System
'Imports System.Text
'Imports System.Buffer


'Namespace com.chang.utils
'    Public Class HmacVB

'        Private count As UInteger()
'        Private state As UInteger()
'        Private buffer As Byte()
'        Private digest As Byte()

'        Public Sub Init()
'            count(0) = 0
'            count(1) = 0
'            state(0) = &H67452301
'            state(1) = 4023233417
'            state(2) = &H98BADCFEUI
'            state(3) = &H10325476UI
'        End Sub

'        Public Function HmacSign(ByVal aValue As String, ByVal aKey As String) As String
'            count = New UInteger(1) {}
'            state = New UInteger(3) {}
'            buffer = New Byte(63) {}
'            digest = New Byte(15) {}
'            Dim k_ipad As Byte() = New Byte(63) {}
'            Dim k_opad As Byte() = New Byte(63) {}
'            Dim keyb As Byte()
'            Dim Value As Byte()
'            keyb = Encoding.UTF8.GetBytes(aKey)
'            Value = Encoding.UTF8.GetBytes(aValue)

'            For i As Integer = keyb.Length To 64 - 1
'                k_ipad(i) = 54
'            Next

'            For i As Integer = keyb.Length To 64 - 1
'                k_opad(i) = 92
'            Next

'            For i As Integer = 0 To keyb.Length - 1
'                k_ipad(i) = CByte((keyb(i) Xor &H36))
'                k_opad(i) = CByte((keyb(i) Xor &H5C))
'            Next

'            Me.Update(k_ipad, CUInt(k_ipad.Length))
'            Me.Update(Value, CUInt(Value.Length))
'            Dim dg As Byte() = Me.Finalize()
'            Me.Init()
'            Me.Update(k_opad, CUInt(k_opad.Length))
'            Me.Update(dg, 16)
'            dg = Me.Finalize()
'            Return ToHex(dg)
'        End Function

'        Public Sub Update(ByVal data As Byte(), ByVal length As UInteger)
'            Dim left As UInteger = length
'            Dim offset As UInteger = (count(0) >> 3) And &H3F
'            Dim bit_length As UInteger = CUInt((length << 3))
'            Dim index As UInteger = 0
'            If length <= 0 Then Return
'            count(0) += bit_length
'            count(1) += (length >> 29)
'            If count(0) < bit_length Then count(1) += 1

'            If offset > 0 Then
'                Dim space As UInteger = 64 - offset
'                Dim copy As UInteger = (If(offset + length > 64, 64 - offset, length))
'                System.Buffer.BlockCopy(data, 0, buffer, CInt(offset), CInt(copy))

'                If offset + copy < 64 Then Return
'                Me.Transform(buffer)
'                index += copy
'                left -= copy
'            End If

'            While left >= 64
'                System.Buffer.BlockCopy(data, CInt(index), buffer, 0, 64)
'                Me.Transform(buffer)
'                index += 64
'                left -= 64
'            End While

'            If left > 0 Then System.Buffer.BlockCopy(data, CInt(index), buffer, 0, CInt(left))
'        End Sub

'        Private Pad As Byte() = New Byte(63) {&H80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

'        Public Function Finalize() As Byte()
'            Dim bits As Byte() = New Byte(7) {}
'            Me.Encode(bits, count, 8)
'            Dim index As UInteger = CUInt(((count(0)) >> 3 And &H3F))
'            Dim padLen As UInteger = If((index < 56), (56 - index), (120 - index))
'            Me.Update(Pad, padLen)
'            Me.Update(bits, 8)
'            Me.Encode(digest, state, 16)

'            For i As Integer = 0 To 64 - 1
'                buffer(i) = 0
'            Next

'            Return digest
'        End Function

'        Public Function ToHex(ByVal input As Byte()) As String
'            If input Is Nothing Then Return Nothing
'            Dim output As StringBuilder = New StringBuilder(input.Length * 2)

'            For i As Integer = 0 To input.Length - 1
'                Dim current As Integer = input(i) And &HFF
'                If current < 16 Then output.Append("0")
'                output.Append(current.ToString("x"))
'            Next

'            Return output.ToString()
'        End Function

'        Private Const S11 As UInteger = 7
'        Private Const S12 As UInteger = 12
'        Private Const S13 As UInteger = 17
'        Private Const S14 As UInteger = 22
'        Private Const S21 As UInteger = 5
'        Private Const S22 As UInteger = 9
'        Private Const S23 As UInteger = 14
'        Private Const S24 As UInteger = 20
'        Private Const S31 As UInteger = 4
'        Private Const S32 As UInteger = 11
'        Private Const S33 As UInteger = 16
'        Private Const S34 As UInteger = 23
'        Private Const S41 As UInteger = 6
'        Private Const S42 As UInteger = 10
'        Private Const S43 As UInteger = 15
'        Private Const S44 As UInteger = 21

'        Private Sub Transform(ByVal data As Byte())
'            Dim a As UInteger = state(0)
'            Dim b As UInteger = state(1)
'            Dim c As UInteger = state(2)
'            Dim d As UInteger = state(3)
'            Dim x As UInteger() = New UInteger(15) {}
'            Me.Decode(x, data, 64)
'            Me.FF(a, b, c, d, x(0), S11, &HD76AA478UI)
'            Me.FF(d, a, b, c, x(1), S12, &HE8C7B756UI)
'            Me.FF(c, d, a, b, x(2), S13, &H242070DB)
'            Me.FF(b, c, d, a, x(3), S14, &HC1BDCEEEUI)
'            Me.FF(a, b, c, d, x(4), S11, &HF57C0FAFUI)
'            Me.FF(d, a, b, c, x(5), S12, &H4787C62A)
'            Me.FF(c, d, a, b, x(6), S13, &HA8304613UI)
'            Me.FF(b, c, d, a, x(7), S14, &HFD469501UI)
'            Me.FF(a, b, c, d, x(8), S11, &H698098D8)
'            Me.FF(d, a, b, c, x(9), S12, &H8B44F7AFUI)
'            Me.FF(c, d, a, b, x(10), S13, &HFFFF5BB1UI)
'            Me.FF(b, c, d, a, x(11), S14, &H895CD7BEUI)
'            Me.FF(a, b, c, d, x(12), S11, &H6B901122)
'            Me.FF(d, a, b, c, x(13), S12, &HFD987193UI)
'            Me.FF(c, d, a, b, x(14), S13, &HA679438EUI)
'            Me.FF(b, c, d, a, x(15), S14, &H49B40821)
'            Me.GG(a, b, c, d, x(1), S21, &HF61E2562UI)
'            Me.GG(d, a, b, c, x(6), S22, &HC040B340UI)
'            Me.GG(c, d, a, b, x(11), S23, &H265E5A51)
'            Me.GG(b, c, d, a, x(0), S24, &HE9B6C7AAUI)
'            Me.GG(a, b, c, d, x(5), S21, &HD62F105DUI)
'            Me.GG(d, a, b, c, x(10), S22, &H2441453)
'            Me.GG(c, d, a, b, x(15), S23, &HD8A1E681UI)
'            Me.GG(b, c, d, a, x(4), S24, &HE7D3FBC8UI)
'            Me.GG(a, b, c, d, x(9), S21, &H21E1CDE6)
'            Me.GG(d, a, b, c, x(14), S22, &HC33707D6UI)
'            Me.GG(c, d, a, b, x(3), S23, &HF4D50D87UI)
'            Me.GG(b, c, d, a, x(8), S24, &H455A14ED)
'            Me.GG(a, b, c, d, x(13), S21, &HA9E3E905UI)
'            Me.GG(d, a, b, c, x(2), S22, &HFCEFA3F8UI)
'            Me.GG(c, d, a, b, x(7), S23, &H676F02D9)
'            Me.GG(b, c, d, a, x(12), S24, &H8D2A4C8AUI)
'            Me.HH(a, b, c, d, x(5), S31, &HFFFA3942UI)
'            Me.HH(d, a, b, c, x(8), S32, &H8771F681UI)
'            Me.HH(c, d, a, b, x(11), S33, &H6D9D6122)
'            Me.HH(b, c, d, a, x(14), S34, &HFDE5380CUI)
'            Me.HH(a, b, c, d, x(1), S31, &HA4BEEA44UI)
'            Me.HH(d, a, b, c, x(4), S32, &H4BDECFA9)
'            Me.HH(c, d, a, b, x(7), S33, &HF6BB4B60UI)
'            Me.HH(b, c, d, a, x(10), S34, &HBEBFBC70UI)
'            Me.HH(a, b, c, d, x(13), S31, &H289B7EC6)
'            Me.HH(d, a, b, c, x(0), S32, &HEAA127FAUI)
'            Me.HH(c, d, a, b, x(3), S33, &HD4EF3085UI)
'            Me.HH(b, c, d, a, x(6), S34, &H4881D05)
'            Me.HH(a, b, c, d, x(9), S31, &HD9D4D039UI)
'            Me.HH(d, a, b, c, x(12), S32, &HE6DB99E5UI)
'            Me.HH(c, d, a, b, x(15), S33, &H1FA27CF8)
'            Me.HH(b, c, d, a, x(2), S34, &HC4AC5665UI)
'            Me.II(a, b, c, d, x(0), S41, &HF4292244UI)
'            Me.II(d, a, b, c, x(7), S42, &H432AFF97)
'            Me.II(c, d, a, b, x(14), S43, &HAB9423A7UI)
'            Me.II(b, c, d, a, x(5), S44, &HFC93A039UI)
'            Me.II(a, b, c, d, x(12), S41, &H655B59C3)
'            Me.II(d, a, b, c, x(3), S42, &H8F0CCC92UI)
'            Me.II(c, d, a, b, x(10), S43, &HFFEFF47DUI)
'            Me.II(b, c, d, a, x(1), S44, &H85845DD1UI)
'            Me.II(a, b, c, d, x(8), S41, &H6FA87E4F)
'            Me.II(d, a, b, c, x(15), S42, &HFE2CE6E0UI)
'            Me.II(c, d, a, b, x(6), S43, &HA3014314UI)
'            Me.II(b, c, d, a, x(13), S44, &H4E0811A1)
'            Me.II(a, b, c, d, x(4), S41, &HF7537E82UI)
'            Me.II(d, a, b, c, x(11), S42, &HBD3AF235UI)
'            Me.II(c, d, a, b, x(2), S43, &H2AD7D2BB)
'            Me.II(b, c, d, a, x(9), S44, &HEB86D391UI)
'            state(0) += a
'            state(1) += b
'            state(2) += c
'            state(3) += d

'            For i As Integer = 0 To 16 - 1
'                x(i) = 0
'            Next
'        End Sub

'        Private Sub Encode(ByRef output As Byte(), ByVal input As UInteger(), ByVal len As UInteger)
'            Dim i, j As UInteger

'            If System.BitConverter.IsLittleEndian Then
'                i = 0
'                j = 0

'                While j < len
'                    output(j) = CByte((input(i) And &HFF))
'                    output(j + 1) = CByte(((input(i) >> 8) And &HFF))
'                    output(j + 2) = CByte(((input(i) >> 16) And &HFF))
'                    output(j + 3) = CByte(((input(i) >> 24) And &HFF))
'                    i += 1
'                    j += 4
'                End While
'            Else
'                i = 0
'                j = 0

'                While j < len

'                    output(j + 3) = CByte((input(i) And &HFF))
'                    output(j + 2) = CByte(((input(i) >> 8) And &HFF))
'                    output(j + 1) = CByte(((input(i) >> 16) And &HFF))
'                    output(j) = CByte(((input(i) >> 24) And &HFF))
'                    i += 1
'                    j += 4
'                End While
'            End If
'        End Sub

'        Private Sub Decode(ByRef output As UInteger(), ByVal input As Byte(), ByVal len As UInteger)
'            Dim i, j As UInteger

'            If System.BitConverter.IsLittleEndian Then
'                i = 0
'                j = 0

'                While j < len
'                    output(i) = (CUInt(input(j))) Or ((CUInt(input(j + 1))) << 8) Or ((CUInt(input(j + 2))) << 16) Or ((CUInt(input(j + 3))) << 24)
'                    i += 1
'                    j += 4
'                End While
'            Else
'                i = 0
'                j = 0

'                While j < len
'                    output(i) = (CUInt(input(j + 3))) Or ((CUInt(input(j + 2))) << 8) Or ((CUInt(input(j + 1))) << 16) Or ((CUInt(input(j))) << 24)
'                    i += 1
'                    j += 4
'                End While
'            End If
'        End Sub

'        Private Sub FF(ByRef a As UInteger, ByVal b As UInteger, ByVal c As UInteger, ByVal d As UInteger, ByVal x As UInteger, ByVal s As UInteger, ByVal ac As UInteger)
'            a += Me.F(b, c, d) + x + ac

'            Dim temValue As Double = Double.p(Me.Rotate_left(a, s) + b)

'            If temValue > UInteger.MaxValue Then
'                a = temValue - UInteger.MaxValue
'            Else
'                a = temValue
'            End If
'        End Sub

'        Private Sub GG(ByRef a As UInteger, ByVal b As UInteger, ByVal c As UInteger, ByVal d As UInteger, ByVal x As UInteger, ByVal s As UInteger, ByVal ac As UInteger)
'            a += Me.G(b, c, d) + x + ac
'            a = Me.Rotate_left(a, s) + b
'        End Sub

'        Private Sub HH(ByRef a As UInteger, ByVal b As UInteger, ByVal c As UInteger, ByVal d As UInteger, ByVal x As UInteger, ByVal s As UInteger, ByVal ac As UInteger)
'            a += Me.H(b, c, d) + x + ac
'            a = Me.Rotate_left(a, s) + b
'        End Sub

'        Private Sub II(ByRef a As UInteger, ByVal b As UInteger, ByVal c As UInteger, ByVal d As UInteger, ByVal x As UInteger, ByVal s As UInteger, ByVal ac As UInteger)
'            a += Me.I(b, c, d) + x + ac
'            a = Me.Rotate_left(a, s) + b
'        End Sub

'        Private Function F(ByVal x As UInteger, ByVal y As UInteger, ByVal z As UInteger) As UInteger
'            Return (x And y) Or (Not x And z)
'        End Function

'        Private Function G(ByVal x As UInteger, ByVal y As UInteger, ByVal z As UInteger) As UInteger
'            Return (x And z) Or (y And Not z)
'        End Function

'        Private Function H(ByVal x As UInteger, ByVal y As UInteger, ByVal z As UInteger) As UInteger
'            Return x Xor y Xor z
'        End Function

'        Private Function I(ByVal x As UInteger, ByVal y As UInteger, ByVal z As UInteger) As UInteger
'            Return y Xor (x Or Not z)
'        End Function

'        Private Function Rotate_left(ByVal x As UInteger, ByVal n As UInteger) As UInteger
'            Return (x << CInt(n)) Or (x >> CInt((32 - n)))
'        End Function
'    End Class
'End Namespace
