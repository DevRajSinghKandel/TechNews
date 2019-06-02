Imports Windows.Web.Http 'Important for understanding the HTTP Protocols.
Imports Windows.Data.Json 'Important for JSON Handling

' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
''' 
Public NotInheritable Class MainPage
    Inherits Page
    Dim viewmodel As New ObservableCollection(Of singlenews)

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Nav1.SelectedItem = A

        getjson()

    End Sub

    Public Async Sub getjson()
        Dim mainnews As String
        Dim imageuristring As String

        Dim f As New HttpClient()
        Dim vmoan As New HttpRequestMessage

        vmoan.Method = HttpMethod.Get

        Dim imagecontent As BitmapImage = Nothing
        vmoan.RequestUri = New Uri("https://newsapi.org/v2/top-headlines?country=US&category=Science&apikey=2f78b3e10d614f7f8e2fc98fbbd37481")


        Dim rep2 As HttpResponseMessage = Await f.SendRequestAsync(vmoan)


        Dim jsona As String = Await rep2.Content.ReadAsStringAsync()


        Dim de As JsonObject = Windows.Data.Json.JsonObject.Parse(jsona)

        Dim va As JsonValue = de.Item("articles")

        For innerloop = 0 To va.GetArray.Count - 1
            Dim fn As JsonValue = va.GetArray.Item(innerloop)

            mainnews = fn.GetObject.GetNamedString("title")


            Dim ewrm As JsonValue = Nothing


            If fn.GetObject.TryGetValue("urlToImage", ewrm) Then

                If ewrm.ValueType = JsonValueType.String Then

                    imageuristring = fn.GetObject.GetNamedString("urlToImage")

                    If imageuristring <> "" Then
                        imagecontent = New BitmapImage(New Uri(imageuristring)
                                                     )

                    End If

                Else
                    imagecontent = New BitmapImage(New Uri("ms-appx:///Assets/StoreLogo.png"))
                End If

            End If

            Dim fnas As New singlenews
            fnas.Title = mainnews
            fnas.image = imagecontent
            viewmodel.Add(fnas)
        Next
    End Sub

    Private Sub RefButton_Click(sender As Object, e As RoutedEventArgs) Handles RefButton.Click
        viewmodel.Clear()
        getjson()

    End Sub
End Class

'Class File to represent a model
Public Class singlenews
    Private ftitle As String
    Public Property Title As String
        Get
            Return ftitle
        End Get
        Set(value As String)
            ftitle = value
        End Set
    End Property
    Property fimage As BitmapImage
    Public Property image As BitmapImage

        Get
            Return fimage
        End Get
        Set(value As BitmapImage)
            fimage = value
        End Set
    End Property
End Class