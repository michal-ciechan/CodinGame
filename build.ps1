$outFile=$args[0]


dir -r -filter *.cs | 
	Where {$_.FullName -notlike '*\obj\*' -and $_.FullName -notlike '*\Properties\*' -and $_.FullName -notlike '*\Original.cs'} | 
	Select-String -pattern "^using" | 
	Select-Object -expand Line -unique | 
	Format-List -property Line | 
	Out-File $outFile
	
dir -r -filter *.cs | 
	Where {$_.FullName -notlike '*\obj\*' -and $_.FullName -notlike '*\Properties\*' -and $_.FullName -notlike '*\Original.cs'} | 
	Select-String -pattern "^using" -NotMatch | 
	Select-Object -expand Line | 
	Format-List -property Line | 
	Out-File $outFile -Append

Get-Content $outFile | clip