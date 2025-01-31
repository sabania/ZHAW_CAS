Sub ReplaceEmailsWithPlaceholders()
    Dim doc As Document
    Dim rng As Range
    Dim regex As Object
    Dim match As Object
    Dim matches As Object
    Dim emailDict As Object
    Dim email As Variant ' Fix: Variant type for For Each loop
    Dim placeholder As String
    Dim csvFile As String
    Dim fileNum As Integer
    Dim i As Integer

    ' Initialize
    Set doc = ActiveDocument
    Set rng = doc.Range
    Set regex = CreateObject("VBScript.RegExp")
    Set emailDict = CreateObject("Scripting.Dictionary")

    ' Regex pattern for email addresses
    regex.Pattern = "[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"
    regex.Global = True

    ' Find all email addresses and store them
    i = 1
    If regex.Test(rng.Text) Then
        Set matches = regex.Execute(rng.Text)
        For Each match In matches
            email = match.Value
            ' Avoid duplicates
            If Not emailDict.Exists(email) Then
                placeholder = "{{EMAIL_" & i & "}}"
                emailDict.Add email, placeholder
                i = i + 1
            End If
        Next
    End If

    ' Replace emails with placeholders
    For Each email In emailDict.Keys ' Fix: email must be Variant
        With rng.Find
            .Text = email
            .Replacement.Text = emailDict(email)
            .Wrap = wdFindContinue
            .MatchWildcards = False
            .Execute Replace:=wdReplaceAll
        End With
    Next

    ' Create CSV file with email mappings
    csvFile = Environ("USERPROFILE") & "\Desktop\email_placeholders.csv"
    fileNum = FreeFile()
    Open csvFile For Output As fileNum
    Print #fileNum, "Email,Placeholder"

    For Each email In emailDict.Keys ' Fix: email must be Variant
        Print #fileNum, email & "," & emailDict(email)
    Next

    Close fileNum

    ' Save new Word document
    Dim newDocPath As String
    newDocPath = Environ("USERPROFILE") & "\Desktop\word_with_placeholders.docx"
    doc.SaveAs2 newDocPath, wdFormatDocumentDefault

    ' Inform user
    MsgBox "Process completed! CSV file saved to: " & csvFile & vbCrLf & "Updated Word document saved to: " & newDocPath, vbInformation, "Done"

End Sub



Sub ImportCSVAndRestoreEmails()
    Dim doc As Document
    Dim rng As Range
    Dim fileNum As Integer
    Dim csvFile As String
    Dim line As String
    Dim emailDict As Object
    Dim parts() As String
    Dim placeholder As Variant ' Fix: Must be Variant for For Each loop
    Dim email As String
    Dim fd As FileDialog

    ' Initialize
    Set doc = ActiveDocument
    Set rng = doc.Range
    Set emailDict = CreateObject("Scripting.Dictionary")

    ' Ask the user to select the CSV file
    Set fd = Application.FileDialog(msoFileDialogFilePicker)
    With fd
        .Title = "Select the CSV File"
        .Filters.Clear
        .Filters.Add "CSV Files", "*.csv"
        .AllowMultiSelect = False
        If .Show = -1 Then
            csvFile = .SelectedItems(1) ' Get the selected file path
        Else
            MsgBox "No file selected. Operation cancelled.", vbExclamation, "Cancelled"
            Exit Sub
        End If
    End With

    ' Open the selected CSV file
    fileNum = FreeFile()
    Open csvFile For Input As fileNum

    ' Read the CSV line by line
    Line Input #fileNum, line ' Skip header row
    Do While Not EOF(fileNum)
        Line Input #fileNum, line
        parts = Split(line, ",") ' Split by comma

        ' Ensure the CSV line is valid
        If UBound(parts) = 1 Then
            email = Trim(parts(0))
            placeholder = Trim(parts(1))

            ' Store in dictionary (key = placeholder, value = email)
            emailDict(placeholder) = email
        End If
    Loop

    ' Close the CSV file
    Close fileNum

    ' Replace placeholders with original emails
    For Each placeholder In emailDict.Keys ' Fix: placeholder must be Variant
        With rng.Find
            .Text = placeholder
            .Replacement.Text = emailDict(placeholder)
            .Wrap = wdFindContinue
            .MatchWildcards = False
            .Execute Replace:=wdReplaceAll
        End With
    Next

    ' Inform user
    MsgBox "Email placeholders restored successfully!", vbInformation, "Process Complete"

End Sub
