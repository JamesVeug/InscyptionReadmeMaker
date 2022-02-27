# Readme Maker
Creates a file that lists all new and modifier cards/abilities and special abilities to be easiy viewed or even used in your custom mods Readme.



To see what your Readme looks like when its on the store.
- Open your Readme.md in a text editor (Notepad, Sublime... etc)
- Copy everything
- Go to https://markdownlivepreview.com/
- Paste in the left column

## Supports:
- [Life Cost API](https://inscryption.thunderstore.io/package/Void_Slime/Life_Cost_API/)
- [Side Deck Selector](https://inscryption.thunderstore.io/package/Infiniscryption/Side_Deck_Selector/)

## Example mods
- [Void Life and Currency Cardpack](https://inscryption.thunderstore.io/package/Void_Slime/Void_Life_and_Currency_Cardpack/)
- [Sire MoDeers](https://inscryption.thunderstore.io/package/Sire/Sire_MoDeers/)
- [MycoBotsinAct1](https://inscryption.thunderstore.io/package/Cevin2006/MycoBotsinAct1/)


## How to use it

### Setup
- Install the mod (Through thunderstore is highly recommended)
- Start the game
- Wait for the console to load the ReadmeMaker mod
- Quit game
- Change **_ReadmeMaker.Enabled** to **true** in the config
- Restart the game so it can add the rest of the configs for you 

### Making a Readme
- Start the game
- Wait for console to say that it has dumped the GENERATED_RTEADME.md
- Y

# !NOTE!
- Thunderstore will not accept a Readme with more than 32,768 characters. So if your mod has a lot of cards and sigils it's likely you'll hit this limit.


## Configs
|Section|Key|Description|
|-|-|-|
|Cards|Align Cost|Centers the cost of the costs. (Adds a lot of characters)|
|Cards|Display By|Changes how the cards, abilities and special abilities are displayed.|
|Cards|Join duplicate Sigils|If a card has 2 of the same sigil, it will show as Fledgling(x2) instead of Fledgling, Fledgling.|
|Cards|Show Cost Min Collapse Amount|Minimum amount before costs are shown as (icon)5 instead of (icon)(icon)...etc|
|Cards|Show Evolutions|Show what each card can evolve into when given Fledgling. (Wolf Cub -> Wolf, Elf Fawn -> Elf... etc).|
|Cards|Show Sigils|Show what each cards Sigils are. (Waterborne, Fledgling... etc).|
|Cards|Show Specials|Show what each cards Special Abilities are. (Ouroboros, Mirror, CardsInHand... etc).|
|Cards|Show Traits|Show what Traits each card has (KillSurvivors, Ant, Goat, Pelt, Terrain... etc).|
|Cards|Show Tribes|Show what Tribes each card has (Insect, Canine... etc).|
|Cards|Show Unobtainable Cards|Show cards that can not be added to your deck.  (Trail cards, Frozen Away Cards, Evolutions... etc)|
|Cards|Sort Type|Changes the order that the cards will be displayed in.|
|Cards|Sort by Ascending|True=Names will be ordered from A-Z, False=Z-A... etc|
|Config|Only Show Plugin|If you only want the make to show configs from a specific Mod, put the guid of that mod here. To lsit more than 1 mod separate them with a comma. eg: "jamesgames.inscryption.readmemaker,jamesgames.inscryption.zergmod"|
|Config|Show Configs|Should the Readme Maker show a section listing all the new configs added?|
|Config|Show GUID|Do you want the Readme Maker to show a column showing the GUID of the mod that the config came from?|
|General|Header Size|How big should the header be? (Does not work for type Foldout!|
|General|Header Type|How should the header be shown? (Unaffected by Size)|
|Modified Cards|Show Modified Cards Section|Show a section that lists all the cards modified.|
|Saving|Path|Where to save this location to. If blank will be same folder as ReadmeMaker.dll. See console for exact location after making a readme|
|Side Deck|Show Side Deck Section|Show a section that lists all the custom side deck cards.|
|Sigils|Show Sigils|Show all new sigils listed on cards in its own section.|
|Special Abilities|Show Special Abilities|Show all new special abilities listed on cards in its own section.|
|_ReadmeMaker|Enabled|Should the ReadmeMaker create a GeneratedReadme?|



## Contributers
- **JamesGames**
- **TeamDoodz**

## Special Thanks:
- **Eri** - Eris's Battery Cost icon
- **Manta Rain** - Manta's Battery Cost icon
- **Zepht** - Zepht's Battery Cost icon


# Update notes:

## `Version: 0.6.0 - 27/2/2022`
### General:
- Mods can now override the names of custom tribes/traits/SpecialStatInfo to be shown correctly.
- Cards are sorted by name by default
- Unobtainable cards are now shown by default

### Added:
- Support for mods to add their own additions to this.
- Config to show a section for custom Configs from specific mods
- Config to enable/disable mod. (Starts off in case mods use this as a dependency)
- Config to change Header Size
- Config to change Header to a dropdown (Doesn't work with sizes)
- Added Bells and CardsInHand SpecialStatInfo support

### Fixed:
- Error when trying to show costs that do not have a single image to show.
- Double up costs when larger than a the largest single image.

<details>
  <summary>See older changes</summary>


## `Version: 0.5.0 - 23/2/2022`
### General:
- Duplicate Sigils on cards are combiend to be Waterborne(x2) instead of Waterborne, Waterborne.
- Added support for viewing modified cards
- Added support for cards to show vanilla stat modifiers (Ant, Mirror... etc)
- Added support for viewing side deck cards
- Readme is now dumped when starting the game instead of waiting 5 seconds
- Did some refactoring for better understand errors when reported

### Added:
- Config to show/hide a side deck card section
- Config to show/hide a modified card section
- Config to combine sigils on cards to be Waterborne(x2) instead of Waterborne, Waterborne.
- Config to show/hide sigil sections
- Config to show/hide special abilities sections

### Fixed:
- Potential fix for Readme maker not working when installed manually
- Sigils with no rulebook name are now ignored in their sections
- Special Abilities with no rulebook name are now ignored in their sections


## `Version: 0.4.0 - 18/2/2022`
### General:
- Refactored how Config works. So you'll need to delete your config so it makes a new one with the correct options.
- Greatly reduced character count

### Added:
- Support for Life Cost
- Config Disable aligning of images
- Config to disable Tribes
- Config to disable Traits
- Config to disable Special Abilites


### Removed:
- Removed Manta's Energy icon because there isn't support for multiple image types per cost yet
- Removed Zepht's Energy icon because there isn't support for multiple image types per cost yet


## `Version: 0.3.0 - 7/2/2022`
### Added:
- Config to change Display Type (List/Table). Table by Default since it uses less characters and looks better.
- Config to change where the readme is exported to.
- Support for descriptions with `[creature]`. Replaced with `A card bearing this sigil`. Thanks to TeamDoodz.


## `Version: 0.2.0 - 27/1/2022`
### Added:
- Config to change how the Readme will be shown
- Added Traits
- Added Tribes
- Added Energy Cost
- Added Mox Gem Costs
- Costs Larger than 4 will now show as (icon)X. Changeable 

### Changed:
- Compressed URLs to fit in more cards in the Readme

### Fixed:
- Icons not aligned


## `Version: 0.1.0 - 17/1/2022`
### General:
- Initial Release

</details>