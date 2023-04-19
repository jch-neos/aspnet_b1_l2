Param(
	[Parameter(Mandatory=$false)][string[]] $scopes
)
if(-not $scopes) { $scopes = @("ReadData") }

$scp = $scopes | join-string -Separator " "

Write-Host "Scopes $scp"

$ClientID = '32687000-a6ef-4a36-9858-f949b3bddf7d'
$TenantID = '50ccda96-bb37-4cfc-be3f-a79cfbcd8647'
$Resource = "97eba5e6-0ca6-46fb-b0c2-329b706bb235"
#$ClientSecret = 'bnp8Q~G8p2bi4j4RscMeQF4eb10~RDpeTJWZXcD3'

$DeviceCodeRequestParams = @{
    Method = 'POST'
    Uri    = "https://login.microsoftonline.com/$TenantID/oauth2/devicecode"
    Body   = @{
	    scope = $scp 
        client_id = $ClientId
        resource  = $Resource
    }
}

$DeviceCodeRequest = Invoke-RestMethod @DeviceCodeRequestParams
Write-Host $DeviceCodeRequest.message -ForegroundColor Yellow

Read-Host

$TokenRequestParams = @{
    Method = 'POST'
    Uri    = "https://login.microsoftonline.com/$TenantId/oauth2/v2.0/token"
    Body   = @{
        grant_type = "urn:ietf:params:oauth:grant-type:device_code"
        code       = $DeviceCodeRequest.device_code
        client_id  = $ClientId
    }
}
$TokenRequest = Invoke-RestMethod @TokenRequestParams
$TokenRequest

