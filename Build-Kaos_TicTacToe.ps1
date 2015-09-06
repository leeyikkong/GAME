param(
	[parameter(Mandatory=$false)]
    [ValidateNotNullOrEmpty()]
    [String] $Kaos_TicTacToe_FolderLocation
)

$Kaos_TicTacToe_FolderLocation = 'C:\Kaos_TTT'

###############################################################################
# Configuration options for the Build script
###############################################################################
# File paths
$slnFullFilePath = "$Kaos_TicTacToe_FolderLocation\Kaos_TicTacToe\Kaos_TicTacToe.sln";
$testDllFullFilePath = """$Kaos_TicTacToe_FolderLocation\Kaos_TicTacToe\Kaos_TicTacToe_Test\bin\Debug\Kaos_TicTacToe_Test.dll""";

# Build Output paths
$consoleBuildOutputFolderPath = "$Kaos_TicTacToe_FolderLocation\Kaos_TicTacToe\Kaos_TicTacToe_Console\bin\Debug";
$formsBuildOutputFolderPath = "$Kaos_TicTacToe_FolderLocation\Kaos_TicTacToe\Kaos_TicTacToe_Forms\bin\Debug";

# Prep folders for zipping
#$consolePrepFolder = "$Kaos_TicTacToe_FolderLocation\Kaos_TicTacToe-Output";
#$formsPrepFolder = "$Kaos_TicTacToe_FolderLocation\Kaos_TicTacToe-Output";
$prepFolder = "$Kaos_TicTacToe_FolderLocation\Kaos_TicTacToe-PkgPrep";

###############################################################################
# Clear the screen while developing
###############################################################################
#Clear-Host;

###############################################################################
# Global variables
###############################################################################
$global:processOutput = "";

###############################################################################
# Function sem sér um að executa process og skila string variable með niðurstöðunni
###############################################################################
function Execute-Process
{
	param
	(
        [parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()] 
		[String] $Executable,

        [parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()] 
		[String] $Arguments,

		[parameter(Mandatory=$false)]
        [ValidateNotNullOrEmpty()] 
		[String] $WorkingDir
	)
	
	process
	{
		$pinfo = New-Object System.Diagnostics.ProcessStartInfo;
		$pinfo.FileName = $Executable;
		$pinfo.RedirectStandardError = $true;
		$pinfo.RedirectStandardOutput = $true;
		$pinfo.UseShellExecute = $false;
		$pinfo.Arguments = $Arguments;
		if($WorkingDir.Length -gt 0)
		{
			$pinfo.WorkingDirectory = $WorkingDir;
		}
		
		$p = New-Object System.Diagnostics.Process;
		$p.StartInfo = $pinfo;
		$p.Start() | Out-Null;
		
		#$output = New-Object System.Text.StringBuilder;
		$output = $p.StandardOutput.ReadToEnd();
		$output += $p.StandardError.ReadToEnd();
		#while($p.StandardOutput.ReadLine() -ne $null)
		#{
		#	$output = $output.AppendLine($p.StandardOutput.ReadLine());
		#}
		$p.WaitForExit();

		$global:processOutput = $output;#.ToString();

		return $p.ExitCode;
	}
}

###############################################################################
# Function sem buildar Visual Studio .sln skrá og prentar út á skjáinn niðurstöðuna
###############################################################################
function Build-VisualStudioSolution
{
	param
	(
		[parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()] 
		[String] $SlnFullFilePath
	)
    
    process
    {
        # Local Variables
        $MsBuild = "$env:systemroot\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe";
		
		if((Test-Path -Path $MsBuild) -ne $true)
		{
			Write-Host "ERROR: Can't find the file: $MsBuild";
			Exit 1;
		}
		else
		{
			# Local Variables
			$SlnFileParts = $SlnFullFilePath.Split("\");
			$SlnFileName = $SlnFileParts[$SlnFileParts.Length - 1];
			$cleanArguments = '"' + $SlnFullFilePath + '"', "/t:clean", "/p:Configuration=Debug", "/v:minimal";
			$runArguments = '"' + $SlnFullFilePath + '"', "/t:rebuild", "/p:Configuration=Debug", "/v:minimal";
			$cleanOutput = "";
			$runOutput = "";

			try
			{
			    # Start the build
				Write-Host "Executing: $MsBuild $runArguments";
				$runExitCode = Execute-Process -Executable $MsBuild -Arguments $runArguments;
				Write-Host "$global:processOutput";
				$global:processOutput = "";

				if($runExitCode -ne 0)
				{
					Write-Error "ERROR: The build failed...";
					Exit 1;
				}
			}
			catch
			{
			    Write-Error ("Unexpect error occured while building " + $SlnFileName + ": " + $_.Message);
				Exit 1;
			}

		    # Show if build succeeded or failed...
		    $failures = Select-String -InputObject $runOutput -Pattern "FAILED" -SimpleMatch
		    
		    if($failures -ne $null)
		    {
		        Write-Error ($SlnFileName + ": A build failure occured. Please check the build log for details.");
				Exit 1;
		    }
		}
    }
}

function Execute-Tests
{
	param
	(
		[parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()] 
		[String] $TestDll
	)

	process
	{
		$MsTest = "";
		if(Test-Path -Path 'C:\Program Files (x86)')
		{
			$MsTest = 'C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\MSTest.exe';
		}
		else
		{
			$MsTest = 'C:\Program Files\Microsoft Visual Studio 11.0\Common7\IDE\MSTest.exe';
		}
		
		if((Test-Path -Path $MsTest) -ne $true)
		{
			Write-Error "ERROR: Can't find the file: $MsTest";
			Exit 1;
		}
		else
		{
			$runArguments = "/testcontainer:" + $TestDll;
			
			Write-Host "Executing: ""$MsTest"" $runArguments";
			
			$testExitCode = Execute-Process -Executable $MsTest -Arguments $runArguments | Write-Output -OutVariable $testOutput;
			Write-Host "$global:processOutput";
			$global:processOutput = "";
			if($testExitCode -ne 0)
			{
				Write-Error "ERROR: Some tests failed...";
				Exit 1;
			}
		}
	}
}

function Test-FolderAndCreateIfNotExists
{
	param
	(
		[parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()] 
		[String] $folderToTest
	)

	process
	{
		if ((Test-Path -Path $folderToTest -PathType Container) -eq $false)
		{
			New-Item -Path $folderToTest -ItemType Container | Out-Null;
		}
	}
}

function Prepare-Output
{
	# Prepare Console files
	Test-FolderAndCreateIfNotExists -folderToTest $prepFolder;

	Write-Host "-- Copying Console executables to ""$prepFolder""";
	Copy-Item -Path "$consoleBuildOutputFolderPath\Kaos_TicTacToe_Console.exe" -Destination "$prepFolder\Kaos_TicTacToe_Console.exe";
	Copy-Item -Path "$consoleBuildOutputFolderPath\Kaos_TicTacToe_Console.exe.config" -Destination "$prepFolder\Kaos_TicTacToe_Console.exe.config";
	#Copy-Item -Path "$consoleBuildOutputFolderPath\Kaos_TicTacToe_Library.dll" -Destination "$prepFolder\Kaos_TicTacToe_Library.dll";

	# Prepare Windows Forms files

	Write-Host "-- Copying Windows Forms executables to ""$prepFolder""";
	Copy-Item -Path "$formsBuildOutputFolderPath\Kaos_TicTacToe_Forms.exe" -Destination "$prepFolder\Kaos_TicTacToe_Forms.exe";
	Copy-Item -Path "$formsBuildOutputFolderPath\Kaos_TicTacToe_Forms.exe.config" -Destination "$prepFolder\Kaos_TicTacToe_Forms.exe.config";
	Copy-Item -Path "$formsBuildOutputFolderPath\Kaos_TicTacToe_Library.dll" -Destination "$prepFolder\Kaos_TicTacToe_Library.dll";
}

function Package-Output
{
	$zipFile = "$Kaos_TicTacToe_FolderLocation\Tic-Tac-Toe-Setup.zip"
	Write-Host "-- Packaging executables to ""$zipFile""";
	Package-OutputToZip -prepFolder $prepFolder -destinationZipFile $zipFile | Out-Null;
}

function Package-OutputToZip
{
	param
	(
		[parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()] 
		[String] $prepFolder,

		[parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()] 
		[String] $destinationZipFile
	)

	process
	{
		$WinRAR = "";
		if(Test-Path -Path 'C:\Program Files\WinRAR\WinRAR.exe')
		{
			$WinRAR = 'C:\Program Files\WinRAR\WinRAR.exe';
		}
		elseif (Test-Path -Path 'C:\Program Files (x86)\WinRAR\WinRAR.exe')
		{
			$WinRAR = 'C:\Program Files (x86)\WinRAR\WinRAR.exe';
		}
		else
		{
			Write-Host "WinRAR was not found, please install or modify this script to point to it...";
			Exit 1;
		}

		Execute-Process -Executable $WinRAR -Arguments "A ""$destinationZipFile"" .\" -WorkingDir $prepFolder;
	}
}

function Pull-Git
{

	$gitExitCode = Execute-Process -Executable "git" -Arguments "pull" -WorkingDir "$Kaos_TicTacToe_FolderLocation\Kaos_TicTacToe\"
	Write-Host "$global:processOutput";
	$global:processOutput = "";

	if($gitExitCode -ne 0)
	{
		Write-Error "ERROR: The unable to pull from git...";
		Exit 1;
	}
}

function Execute-Main
{
	###############################################################################
	# Keyrslan á sjálfu scriptinu
	###############################################################################

	Write-Host "*******************************************************************************";
	Write-Host "Step 1/5: Pulling a newer version from Git";
	Write-Host "*******************************************************************************";
	Write-Host;

	Pull-Git;

	Write-Host;
	Write-Host "*******************************************************************************";
	Write-Host "Step 2/5: Building Tic Tac Toe";
	Write-Host "*******************************************************************************";
	Write-Host;
	
	Build-VisualStudioSolution -SlnFullFilePath $slnFullFilePath

	Write-Host;
	Write-Host "*******************************************************************************";
	Write-Host "Step 3/5: Running Unit Tests";
	Write-Host "*******************************************************************************";
	Write-Host;
	
	Execute-Tests -TestDll $testDllFullFilePath
	
	Write-Host;
	Write-Host "*******************************************************************************";
	Write-Host "Step 4/5: Preparing build output for packaging";
	Write-Host "*******************************************************************************";
	Write-Host;

	Prepare-Output;

	Write-Host;
	Write-Host "*******************************************************************************";
	Write-Host "Step 5/5: Packaging the build output";
	Write-Host "*******************************************************************************";
	Write-Host;
	
	Package-Output;
}

Execute-Main
