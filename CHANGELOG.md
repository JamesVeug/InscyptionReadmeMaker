## `Version: 1.4.0 - 5/02/2023`
### Added:
- Added Meta Categories column to all card sections
- Added Config to make the Modified Cards section show only changed columns.

### General:
- Modified Cards section now shows all of a cards columns instead of only changes.
- Modified Cards section no longer shows a '/' for sigils, special abilities... etc if they have no entries.
- Fixed sometimes traits showing a number instead of their text name.


## `Version: 1.3.3 - 25/01/2023`
### General:
- Updated to API requirement to 2.9.1
- Fixed Descriptions not populating [Define] correctly.


## `Version: 1.3.2 - 04/01/2023`
### General:
- Fixed Custom Traits showing as a number instead of a name


## `Version: 1.3.1 - 20/11/2022`
### General:
- Fixed error with API 2.7.1's changes to the Pelt Manager


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
