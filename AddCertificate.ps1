##FUNCTIONS##
""

# Dette er en automatisk variabel satt til file's/module's directory
$filePicker = "$PSScriptRoot\FilePicker.ps1";

# Legge til filvelger-script
. $filePicker.ToString();

"Du starter n� importering av sertifikat / privatn�kkel til dine personlige sertifikater"
Read-Host "F�rst m� du velge filen. Trykk en tast for � fortsette"

$n�kkelpath = Get-FileName -initialDirectory "c:\"

#$n�kkelpath = ReadHost 'Hva er filstien til privatn�kkelen?'
$enteredpwd = Read-Host 'Hva er passordet til privatn�kkelen?'

$mypwd = ConvertTo-SecureString -String $enteredpwd -Force -AsPlainText
Import-PfxCertificate -FilePath "C:\Users\Aleksander Sjafjell\Desktop\buypass_mf_test_autentisering.p12" cert:\currentUser\my -Exportable -Password $mypwd  -Confirm
Read-Host Trykk en tast for � avslutte ...


