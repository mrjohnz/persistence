# Replace connection strings for AppVeyor SQLServer 2012 Express
$startPath = "$($env:appveyor_build_folder)\src\Atlas.Persistence.EntityFramework.Tests"
$config = join-path $startPath "app.config"
$doc = (gc $config) -as [xml]
$doc.SelectSingleNode('//connectionStrings/add[@name="Persistence"]').connectionString = "Server=(local)\SQL2012SP1;Database=Persistence;User ID=sa;Password=Password12!;MultipleActiveResultSets=True"
$doc.SelectSingleNode('//connectionStrings/add[@name="PersistenceEF"]').connectionString = "Server=(local)\SQL2012SP1;Database=PersistenceEF;User ID=sa;Password=Password12!;MultipleActiveResultSets=True"
$doc.Save($config)
