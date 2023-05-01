$variables = "VRA_TENANT", "VRA_HOST", "VRA_USER", "VRA_PASS"
$values = @{}

$variables | ForEach-Object {
    
    $current = [System.Environment]::GetEnvironmentVariable($_, "Machine") 

    if ($null -ne $current) {
        $display = "($current)"
    }

    if ($_ -ne "VRA_PASS") {        
        $values[$_] = (Read-Host "Enter value for $_ $display"), $current | Where-Object { $_ -ne $null -and $_ -ne '' } | Select-Object -First 1 
    }
    else {
        if ([console]::CapsLock) { 
            Write-Warning 'CAPSLOCK is ON' 
        }
        
        $pass = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(([System.Runtime.InteropServices.Marshal]::SecureStringToBSTR((Read-Host "Enter value for $_" -AsSecureString))))
        $values[$_] = [System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($pass))
    }
}

Write-Host "Configuring environment variables..."

$variables | ForEach-Object {
    [System.Environment]::SetEnvironmentVariable($_, $values[$_])
    [System.Environment]::SetEnvironmentVariable($_, $values[$_], "Machine")

    Write-Host "$_=$([System.Environment]::GetEnvironmentVariable($_, "Machine"))"
}

Write-Host "Configuration complete."
