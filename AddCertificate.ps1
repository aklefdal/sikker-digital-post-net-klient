##FUNCTIONS##

# This is an automatic variable set to the current file's/module's directory
$filePicker = "$PSScriptRoot\FilePicker.ps1";

. $filePicker.ToString();

"Du starter n� importering av sertifikat / privatn�kkel til dine personlige sertifikater"
Write-Host "F�rst m� du velge filen. Trykk en tast for � velge"

$n�kkelpath = Get-FileName -initialDirectory "c:\"

#$n�kkelpath = ReadHost 'Hva er filstien til privatn�kkelen?'
$enteredpwd = Read-Host 'Hva er passordet til privatn�kkelen?'

$mypwd = ConvertTo-SecureString -String $enteredpwd -Force -AsPlainText
Import-PfxCertificate -FilePath "C:\Users\Aleksander Sjafjell\Desktop\buypass_mf_test_autentisering.p12" cert:\currentUser\my -Exportable -Password $mypwd  -Confirm
Write-Host Trykk en tast for � avslutte ...


