$serviceProjFile = "E:\Personal\ProductMadness\Calculator\Calculator\CalculatorService\CalculatorService.csproj"
$serviceTestProjFile = "E:\Personal\ProductMadness\Calculator\Calculator\CalculatorService.uTest\CalculatorService.uTest.csproj"
$publishDirectory = "E:\Personal\ProductMadness\Calculator\publish"



function Increase-Version {
    $xml = [Xml] (Get-Content $serviceProjFile)

    Write-Host $xml
    $initialVersion = [string] $xml.Project.PropertyGroup.Version
    Write-Host "Initial Version: " $initialVersion    
    
    $spliteVersion = $initialVersion.ToString().Split(".")

    $majorVersion = [int]$spliteVersion[0]
    $majorVersion = $majorVersion + 1

    $spliteVersion[0] = $majorVersion.ToString()
    
    $newVersion = [String]::Join(".", $spliteVersion)

    $xml.SelectNodes("/Project/PropertyGroup/Version")[0].InnerText = $newVersion    
    $xml.Save($serviceProjFile)
}

Clear-Host

if(Test-Path $serviceProjFile)
{
    Write-Host "Building The Application..." -ForegroundColor Cyan
    dotnet clean $serviceProjFile -c Release
    Increase-Version
    dotnet build $serviceProjFile -c Release
    if($LASTEXITCODE -eq 0)
    {
        Write-Host "Build SUCCESS" -ForegroundColor Green
        Write-Host "================================================================"

        if(Test-Path $serviceTestProjFile)
        {
            Write-Host "Building Tests..." -ForegroundColor Cyan
            dotnet build $serviceTestProjFile -c Release
            if($LASTEXITCODE -eq 0)
            {
                Write-Host "Build SUCCESS" -ForegroundColor Green
                Write-Host "================================================================"
                Write-Host "Runnign Tests..." -ForegroundColor Cyan
                dotnet test $serviceTestProjFile
                if($LASTEXITCODE -eq 0)
                {
                    Write-Host "Test Passed" -ForegroundColor Green
                    Write-Host "================================================================"

                    if(Test-Path $publishDirectory)
                    {
                        Write-Host "Publishing Service..." -ForegroundColor Cyan
                        dotnet publish $serviceProjFile -o $publishDirectory -c Release
                        if($LASTEXITCODE -eq 0)
                        {
                            Write-Host "Publish Complete" -ForegroundColor Green
                            Write-Host "================================================================"
                        }
                        
                    }
                    else
                    {
                        Write-Host "Docker file does not exist : $dockerFile" -ForegroundColor Red
                    }
                }
            }
            
        }
        else
        {
            Write-Host "Test project file does not exist : $serviceTestProjFile" -ForegroundColor Red
        }
    }
    else
    {
        Write-Host "Build Failed" -ForegroundColor Red
    }
    
}
else
{
    Write-Host "Source project file does not exist : $serviceProjFile" -ForegroundColor Red
}