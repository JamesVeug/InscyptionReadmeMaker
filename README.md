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
- [ZergMod](https://inscryption.thunderstore.io/package/JamesGames/ZergMod/)


## How to use it

### Setup
- Install the mod (Through thunderstore is highly recommended)
- Start the game
- Wait for the console to load the ReadmeMaker mod
- Quit game
- Change **_ReadmeMaker.Enabled** to **true** in the config
- Restart the game so it can add the rest of the configs for you 

### Making a Readme
- Start Inscryption
- Start the game either vanilla or KCM until it gets to the map
- Wait for console to say that it has dumped the GENERATED_README.md


# !NOTE!
- Thunderstore will not accept a Readme with more than 32,768 characters. So if your mod has a lot of cards and sigils it's likely you'll hit this limit.


## Adding Custom Information
The Readme Maker has support for your mod to add additional information to readme dumps

### Setup
- Go to https://github.com/JamesVeug/InscyptionReadmeMaker/tree/develop/Scripts
- Download ExternalHelpers method and put it in your mod

### How to add a custom card section to your mod
- Create a new class and make it inherit from `CustomCardSection` 
- Override Initialize so it returns all the cards you want to appear in the readme dump.
- In your `Plugin.cs` script call `AddSectionToReadmeMaker`


eg:
```csharp
private void Awake()
{
    new AllBoneCardsSection().AddSectionToReadmeMaker();
}
```

```csharp
public class AllBoneCardsSection : CustomCardSection
{
    public override string SectionName() => "All Bone Cards";
    public override bool Enabled() => true;
    
    public override List<CardInfo> Initialize()
    {
        return CardManager.AllCardsCopy.FindAll(a => a.bonesCost > 0);
    }
}
```

### How to add a general custom section to your mod
[Go here for an example of what a custom section looks like](https://github.com/JamesVeug/InscyptionReadmeMaker/blob/develop/Scripts/ExternalHelpers/Examples/ExampleCustomSection.cs)
- Create a new class and make it inherit from `CustomSection`
- Fill our all methods as you need
- In your `Plugin.cs` script call `AddSectionToReadmeMaker`

eg:
```csharp
private void Awake()
{
    new ExampleCustomSection().AddSectionToReadmeMaker();
}
```

### How to add a custom cost to your mod
[Go here for an example of what a custom cost looks like](https://github.com/JamesVeug/InscyptionReadmeMaker/blob/develop/Scripts/ExternalHelpers/Examples/ExampleCustomCost.cs)
- Create a new class and make it inherit from `CustomCost`
- Fill our all methods as you need
- In your `Plugin.cs` script call `AddCostToReadmeMaker`

eg:
```csharp
private void Awake()
{
    new ExampleCustomCost().AddCostToReadmeMaker();
}
```

## 41 New Configs

<details>
  <summary>See all configs</summary>

|Section|Key|Description|
|:-|:-|:-|
|1. Readme Maker|Enabled|Should the ReadmeMaker create a GeneratedReadme?|
|1. Readme Maker|Save To|Where to save the generated readme to. If blank will be same folder as ReadmeMaker.dll. See console for exact location after making a readme.|
|2. General|Display By|Changes how the cards, abilities and special abilities are displayed.|
|2. General|Filter by JSONLoader Mod Prefix|Show .jdlr cards with a specific Mod Prefix. Disable by leaving blank.|
|2. General|Filter by Mod GUID|Only cards, sigils... etc related to this mods GUID. Disable by leaving blank.|
|2. General|Header Size|How big should the header be? (Does not work for type Foldout!|
|2. General|Header Type|How should the header be shown? (Unaffected by Size)|
|2. General|Ignore Empty Columns|True=Any columns that have no data will not be shown, False=All columns shown even if it has no data to show.|
|2. General|Ignore Mod by GUID|Ignore mods using these guids. Separate multiple guids by a comma. Disable by leaving blank.|
|2. General|Show GUIDs|Show the mod GUID for each sigils, tribes... etc.|
|2. General|Sort By|Changes the order of how rows in sections are displayed.|
|2. General|Sort by Ascending|True=Names will be ordered from A-Z, False=Z-A... etc.|
|3. Toggle Sections|Gramophone Sort Type|Order of which the Gramophone tracks will show in.|
|3. Toggle Sections|Show Boons|Show all new Boons added in its own section..|
|3. Toggle Sections|Show Cards Modified|Show a section that lists all the cards modified.|
|3. Toggle Sections|Show Configs|Should the Readme Maker show a section listing all the new configs added?|
|3. Toggle Sections|Show Consumable Items|Show all new Consumable Items added in its own section.|
|3. Toggle Sections|Show Encounters|Show all new encounters added in its own section..|
|3. Toggle Sections|Show Gramophone Tracks|Show all new Gramophone Tracks added in its own section.|
|3. Toggle Sections|Show Kaycees Mod Challenges|Show all new challenges added for Kaycee's mod.|
|3. Toggle Sections|Show Kaycees Mod Starter Decks|Show all new starter decks for Kaycee's mod.|
|3. Toggle Sections|Show Map Nodes|Show all new map nodes added in its own section..|
|3. Toggle Sections|Show Pelts|Show all new Pelts added in its own section.|
|3. Toggle Sections|Show Regions|Show all new regions.|
|3. Toggle Sections|Show Side Decks|Show a section that lists all the custom side deck cards.|
|3. Toggle Sections|Show Sigils|Show all new sigils listed on cards in its own section.|
|3. Toggle Sections|Show Special Abilities|Show all new special abilities listed on cards in its own section.|
|3. Toggle Sections|Show Tribes|Show all new tribes added in its own section.|
|4. Card Section Options|Align Cost|Centers the cost of the costs. (Adds a lot of characters)|
|4. Card Section Options|Join duplicate Sigils|If a card has 2 of the same sigil, it will show as Fledgling(x2) instead of Fledgling, Fledgling.|
|4. Card Section Options|Show Cost Min Collapse Amount|Minimum amount before costs are shown as (icon)5 instead of (icon)(icon)...etc|
|4. Card Section Options|Show Evolutions|Show what each card can evolve into when given Fledgling. (Wolf Cub -> Wolf, Elf Fawn -> Elf... etc).|
|4. Card Section Options|Show Frozen Away|Show what each card turns into when killed given the Frozen Away sigil. (Frozen Possum -> Possum... etc).|
|4. Card Section Options|Show Sigils|Show what each cards Sigils are. (Waterborne, Fledgling... etc).|
|4. Card Section Options|Show Specials|Show what each cards Special Abilities are. (Ouroboros, Mirror, CardsInHand... etc).|
|4. Card Section Options|Show Tail|Show what each card will leave behind before attacked. (Skink -> Skink Tail... etc).|
|4. Card Section Options|Show Traits|Show what Traits each card has (KillSurvivors, Ant, Goat, Pelt, Terrain... etc).|
|4. Card Section Options|Show Tribes|Show what Tribes each card has (Insect, Canine... etc).|
|4. Card Section Options|Show Unobtainable Cards|Show cards that can not be added to your deck.  (Trail cards, Frozen Away Cards, Evolutions... etc)|
|4. Card Section Options|Sort Type|Changes the order that the cards will be displayed in.|
|4. Card Section Options|Sort by Ascending|True=Names will be ordered from A-Z, False=Z-A... etc|
</details>



## Contributers
- **JamesGames**
- **TeamDoodz**
- **der Kartoffelcode**

## Special Thanks:
- **Eri** - Eris's Battery Cost icon
- **Manta Rain** - Manta's Battery Cost icon
- **Zepht** - Zepht's Battery Cost icon


# Update notes:

## `Version: 1.3.0 - 17/11/2022`
### General:
- Upgraded API to 2.7.0
 
### Added:
#### General
- Added support for new Pelts
- Added support for new Gramophone Tracks

#### Region Section
- Added Main Tribes column
- Added Opponents column
- Added Items column
- Added Encounters column

#### Encounters Section
- Added Min Difficulty column
- Added Max Difficulty column
- Added Regions column
- Added Main Tribes column
- Added Turns column

### Changes:
- Replaced Region Specific from Item Section with Available In Regions column

<details>
  <summary>See older changes</summary>


## `Version: 1.2.0 - 6/11/2022`
### General:
- The Readme Maker creates a separate GENERATED_README per mod now!

### Added:
- Added support for new Consumable Items


## `Version: 1.1.0 - 2/10/2022`
### Added:
- Support for custom Sections from mods
- Support for custom card Sections from mods
- Support for custom costs from mods


## `Version: 1.0.0 - 29/6/2022`
### General:
- New Icon!
- ReadmeMaker now loads before all other mods but after API.
- Changed mod GUID to _jamesgames.inscryption.readmemaker
- Refactored configs.

### Added:
- Added Modified Cards section
- Added GUID column to all sections. Shows when *Show GUIDs* is on.
- Added Mod Prefix column to all card sections. Shows when *Show GUIDs* is on.
- Added Card Count to Tribe section
- Added Config to show Show GUIDs for all sections. Off by default
- Added Config to filter mods by GUID
- Added Config to filter mods by Mod Prefix
- Added Config to ignore mods by GUID
- Added Config to sort everything by Ascending or Descending

### Fixed:
- Fixed Tribes incorrectly appearing
- Fixed New Cards section disabling when Ascension starer decks disabled
- Fixed Disabling Tribe column not working
- Fixed Disabling Config section not working
- Fixed MapNode Section showing wrong GUID.
- Fixed Patches running when Readme Maker is disabled
- Fixed Extra space appearing for Sigils and Special Abilities 

## `Version: 0.11.0 - 27/3/2022`
### General:
- Bumped API requirement to 2.4

### Added:
- New Boon section
- New Region section

### Fixed:
- Fixed new Map nodes not appearing using new API manager
- Fixed new challenges breaking Readme dump
- Fixed new starter deck section appearing even when disabled

## `Version: 0.10.0 - 27/3/2022`
### General:
- Bumped API to 2.1

### Added:
- New Tribe section
- New Encounter section

### Fixed:
- Fixed cards missing a displayName causing a NullReference
- Fixed Map node section not showing 

## `Version: 0.9.0 - 25/3/2022`
### General:
- Combined Summary and dropdowns
- Removed List display type

### Added:
- Added custom Map node section with Config support
- Added custom Ascension Challenge section with Config support
- Added custom Ascension Starter Deck section with Config support
- Added Money cost support
- Added Life cost support

### Fixed:
- Fixed LifeMoney cost not working
- Added cards only accessible by tail not showing in list

## `Version: 0.8.0 - 22/3/2022`
### General:
- Api v2.0/Kaycees mod support (Modified Cards not supported yet)

### Added:
- Tail column added with config

## `Version: 0.7.0 - 12/3/2022`
### General:
- Power for cards now uses baseDamage instead. No longer modified by the game.
- Health for cards now uses baseDamage instead. No longer modified by the game.

### Added:
- Frozen Away column added with config

### Fixed:
- Evolution cards not appearing in card list when `Show Unobtainable Cards` is off. 

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