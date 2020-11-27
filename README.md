# Younited.I18nMigrationTool 
Migrate the default angular xliff-based internationalization system to a json-based translation system.




## Your problem 😢
Do you struggling with your current angular xliff internationalization system ? 
Do you want to migrate to ngx-translate or another json-base internationalization system ? But you have too much translations to do this repetitive task manually !

## Our solution 😂
Younited.I18nMigrationTool 
This open-source project will migrate your application with the speed of light !

## Workflow ⚙️
read all the XLIFF files to get a list of the i18n keys and their texts
Migrate all the Angular html templates
Create the Json files with the migrated translations
Update the Xliff files to flag the migrated keys as obsolete
Create a migration report file 

## Current limitations
can't migrate text with links or interpolation (IsEligibleToMigrate flag)
can't parse inline html template
can't migrate i18n-xxx attributes, example : 
 i18n-place-holder="@@greatPlaceholder" 

## Configuration file
xlfRootPath : xliff files folder path
xlfFiles : array of your xliff files
angularProjectRootPath : folder root path of the Angular project (to find the templates to migrate)
jsonTargetPath : folder path to create the Json files created
reportFilePath : report file path

## Warnings ⚠️
Of course, the implementation of the ngx-translate library or the other translation system have to be done by your own !
The optional tag value allows you to add another dimension to a culture.
For example you can manage translations for a same culture but that diverge regarding the device target(mobile / desktop).
The migration tool will check in that case if the translations for a same culture are the same for all the tags (/devices) (method : CheckDifferenceBetweenTags).
An i18n key can be eligible to migrate, but the migration isn't done due to its usage :
- custom i18n-xxx attributes (see limitations)
- not found in the template