param([string]$Accion = 'Preparar', [string]$Destino = 'Windows')
$ErrorActionPreference = 'Stop'
$cartalPath = 'D:\CodexWork\Cartal'
$env:TEMP = Join-Path $cartalPath 'Temp'
$env:TMP = $env:TEMP
$env:UPM_CACHE_ROOT = Join-Path $cartalPath 'cache-upm'
$env:GRADLE_USER_HOME = Join-Path $cartalPath 'cache-gradle'
if ($Destino -eq 'Android') {
    $env:CARTAL_ANDROID_TOOLS = 'D:\Android\Unity6000.3.11f1'
    $env:JAVA_HOME = Join-Path $env:CARTAL_ANDROID_TOOLS 'OpenJDK'
    $env:ANDROID_HOME = 'D:\Android\Sdk'
    $env:ANDROID_NDK_ROOT = Join-Path $env:CARTAL_ANDROID_TOOLS 'android-ndk-r27c'
}
$editorPath = 'D:\CodexWork\Snake-PGM611-20261006\Unity\6000.3.11f1\Editor\Unity.exe'
$metodo = 'PrepararCartal.' + $Accion
if ($Accion -eq 'Pruebas') { $metodo = 'PruebasCartal.Ejecutar' }
$argumentos = @('-batchmode', '-projectPath', (Join-Path $cartalPath 'Proyecto'), '-executeMethod', $metodo, '-logFile', (Join-Path $cartalPath ('logs\unity-' + $Accion + '-' + $Destino + '.log')))
if ($Accion -ne 'Pruebas') { $argumentos += '-quit' }
if ($Destino -eq 'Android') { $argumentos += @('-buildTarget', 'Android') }
if ($Destino -eq 'Windows') { $argumentos += @('-buildTarget', 'Win64') }
$unityProcess = Start-Process -FilePath $editorPath -ArgumentList $argumentos -WindowStyle Hidden -PassThru
Write-Output ('UNITY_PID=' + $unityProcess.Id)
