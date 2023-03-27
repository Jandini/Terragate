# Configure values for VRA_USER and VRA_PASS environment variables
$userName = Read-Host "Enter user name to be set for VRA_USER variable"
$userPass = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(([System.Runtime.InteropServices.Marshal]::SecureStringToBSTR((Read-Host "Enter password for $userName" -AsSecureString))))
$userPass = [System.Convert]::ToBase64String([System.Text.Encoding]::Unicode.GetBytes($userPass))

if ($userName -and $userPass) {

    Write-Host "Configuring VRA_USER and VRA_PASS variables..."
    [System.Environment]::SetEnvironmentVariable("VRA_USER", $userName, "Machine")
    [System.Environment]::SetEnvironmentVariable("VRA_PASS", $userPass, "Machine")

    Write-Host "Configuration complete."
}
