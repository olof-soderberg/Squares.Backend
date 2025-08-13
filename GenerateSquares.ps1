# Generate squares through the API
Write-Host "Generating squares through the API..."
$apiUrl = "http://localhost:5272/api/squares"

for ($i = 0; $i -lt 20; $i++) {
    try {
        $response = Invoke-RestMethod -Uri $apiUrl -Method POST
        Write-Host "Created square $i" -ForegroundColor Green
    } catch {
        Write-Host "Error creating square $i : $_" -ForegroundColor Red
    }
    Start-Sleep -Milliseconds 100
}

Write-Host "Done generating squares." -ForegroundColor Cyan
