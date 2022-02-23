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


## How to use it
- Install the mod (Through thunderstore is highly recommended)
- Start the game
- Wait for console to say that it has dumped the GENERATED_RTEADME.md
 
# !NOTE!
- Thunderstore will not accept a Readme with more than 32,768 characters. So if your mod has a lot of cards and sigils it's likely you'll hit this limit.

## Contributers
- **JamesGames**
- **TeamDoodz**

## Special Thanks:
- **Eri** - Eris's Battery Cost icon
- **Manta Rain** - Manta's Battery Cost icon
- **Zepht** - Zepht's Battery Cost icon


# Update notes:

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


<details>
  <summary>See older changes</summary>

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