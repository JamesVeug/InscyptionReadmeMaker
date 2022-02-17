# Readme Maker
Creates a file that lists all new cards/abilities and special abilities to be used in your Readme.



To see what your Readme looks like when its on the store.
- Open your Readme.md in a text editor (Notepad, Sublime... etc)
- Copy everything
- Go to https://markdownlivepreview.com/
- Paste in the left column

## How to use it
- Install the mod (Through thunderstore is highly recommended)
- Start the game
- Wait for console to say that it has dumped the GENERATED_RTEADME.md


## Notes:
- Supports Void_Slime's Life Cost
- Thunderstore will not accept a Readme with more than 32,768 characters. So if your mod has a lot of cards and sigils it's likely you'll hit this limit.

## Contributers
- **JamesGames**
- **TeamDoodz**

## Special Thanks:
- **Eri** - Eris's Battery Cost icon
- **Manta Rain** - Manta's Battery Cost icon
- **Zepht** - Zepht's Battery Cost icon


# Update notes:

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