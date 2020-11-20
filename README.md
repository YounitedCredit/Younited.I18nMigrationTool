# Younited.I18nMigrationTool
migrate the default angular xliff-base internationalization system to a json-base translation system.


#WIP

Younited.I18nMigrationTool
migrate the default angular xliff-base internationalization system to a json-base translation system.
do you have more than 60 translations to maintains ?
do you have to maintain your translations in more than 2 languages ?
do your stakeholders always want to update the translations but ?
are you bad in German and German is one of your language to maintain ?
do you want to migrate with the speed of light :

<span class="wrapper-text small-text" i18n="@@seeMyCommercialOffer">Voir mon offre personnalisée</span>
=>
<span class="wrapper-text small-text" [innerHTML]="'see_my_commercial_offer' | translate"></span>


TLTR
The i18nMigratioTool migrate the default angular i18n Internationalization system to a json-based translation system.

current limitations :
- work with XLIFF files
- can't migrate text with links or interpolation (IsEligibleToMigrate flag)
- can't parse inline html template
- can't migrate i18n-xxx attributes (ex : i18n-place-holder="@@greatPlaceholder")
configuration file, what are the input parameters required ?
how does it work ?
- it scan all the translation files
the tag value allows you to add another dimension to a culture, for example you can manage translations for a same culture but that diverge on the device,
the migration tool will check in that case of the text for a same culture are the same on all tags (/devices).
a i18n kay can be eligible to migrate, but the migration isn't done due to the usage :
- custom i18n-xxx attributes (see limitation)
- not found in the template
Warning
of course, the implementation of the ngx-translate or the other translation system have to be done ! Have fun (ngx link)
Shout-out
Just wnat to let knwo that we are using phrase and we are happy so far !