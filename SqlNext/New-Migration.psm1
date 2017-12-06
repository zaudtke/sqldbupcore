function New-Migration {
  param (
	[string] $Version,
    [string] $Name
  )

  	
	$project = Get-Project
   $projectDirectory = Split-Path $project.FullName
   $scriptsDirectoryName = "migrations\" + $Version
   $scriptDirectory = $projectDirectory + "\" +  $scriptsDirectoryName  
   $fileNameBase = ((Get-Date).ToUniversalTime()).ToString("yyyyMMddHHmmss")
 
   #Get reference to Scripts project item
   $targetProjectItem = $null
   
   try
   {
	   
      $targetProjectItem = $project.ProjectItems.Item("migrations").ProjectItems.Item($Version)
   }
   catch
   {

      $project.ProjectItems.AddFolder($scriptsDirectoryName) | Out-Null
      $targetProjectItem = $project.ProjectItems.Item($scriptsDirectoryName)
   }   

   If ($name -ne ""){
      $fileNameBase = $fileNameBase + "_" + $Name
   }

   $fileNameBase = $fileNameBase.Replace(" ","")
   $fileName = $fileNameBase + ".sql"
   $filePath = $scriptDirectory + "\" + $fileName

   New-Item -path $scriptDirectory -name $fileName -type "file" | Out-Null
   try
   {
      "/* Migration Script */" | Out-File -Encoding $Encoding -FilePath $filePath
   }
   catch
   {
      "/* Migration Script */" | Out-File -Encoding ascii -FilePath $filePath
   }
 
   $targetProjectItem.ProjectItems.AddFromFile($filePath) | Out-Null

   $item = $targetProjectItem.ProjectItems.Item($fileName) 
   $item.Properties.Item("BuildAction").Value = [int]3 #Embedded Resource
   Write-Host "Created new migration: ${fileName}"
   $dte.ExecuteCommand("File.OpenFile", $scriptsDirectoryName + "\" + $fileName)


}